namespace SimpleBitcoinWallet
{
    using System;
    using System.Globalization;
    using System.Text;
    using HBitcoin;
    using HBitcoin.KeyManagement;
    using NBitcoin;
    using QBitNinja;
    using QBitNinja.Client;

    public class Startup
    {
        private static string[] Commands = new[] { "create", "recover", "balance", "history", "receive", "send", "exit" };

        private static string EnterCommand = $@"Enter one of the command after \: {string.Join(", ", Commands)}";

        private static string EnterPassword = "Enter password: ";
        private static string ConfirmPassword = "Confirm password: ";

        private static string EnterMnemonic = "Enter mnemonic phrase: ";
        private static string EnterDate = "Enter date (yyyy-MM-dd): ";

        private static string EnterWalletName = "Enter wallet name: ";
        private static string EnterWalletAddress = "Enter wallet address: ";

        private static string EnterPrivateKey = "Enter private key: ";

        private static string EnterAddressToSend = "Enter address to send to: ";
        private static string EnterAmountToSend = "Enter amount to send: ";
        private static string EnterAmountToGetBack = "Enter amount to get back: ";
        private static string EnterMessage = "Enter message: ";

        private static string TransactionSent = "Transaction sent!";
        private static string SomethingWentWrong = "Something went wrong :(";

        private static string WalletExists = "Wallet already exists.";
        private static string PasswordDoesntMatch = "Passwords does not match. Try again.";
        private static string WalletNameDoesntExists = "Wallet name does not exist!";
        private static string WrongPrivateKey = "Wrong private key!";
        private static string WrongWalletOrPassword = "Wrong wallet or password!";

        private static string CreatedWallet = $"Wallet created successfully.{Environment.NewLine}Write down the following mnemonic words.{Environment.NewLine}With the mnemonic words and the password you can recover this wallet.";
        private static string PrivateKeys = "Write down and keep in secure place your private keys. Only through them you can access your coins!";

        private static string RecoverCaution = $"Please not the wallet cannot check if your password is correct or not.{Environment.NewLine}If you provide a wrong password a wallet will be recovered with your provided mnemonic and password pair:";
        private static string RecoveredWallet = "Wallet successfully recovered.";

        private static string CoinsReceived = "-----COINS RECEIVED-----";
        private static string CoinsReceivedEnd = "------------------------";

        private static Network currNetwork = Network.TestNet;
        private static string walletFilePath = @"Wallets\";

        public static void Main()
        {
            Console.WriteLine(EnterCommand);
            var input = Console.ReadLine().ToLower().Trim();

            while (input != "exit")
            {
                string password;
                string walletName;

                switch (input)
                {
                    case "create":
                        Create();
                        break;
                    case "recover":
                        Console.WriteLine(RecoverCaution);

                        Console.Write(EnterPassword);
                        password = Console.ReadLine();

                        Console.Write(EnterMnemonic);
                        var inputMnemonic = Console.ReadLine();

                        Console.Write(EnterDate);
                        var date = Console.ReadLine();

                        var mnemonic = new Mnemonic(inputMnemonic);

                        Recover(password, mnemonic, date);
                        break;
                    case "receive":
                        Console.Write(EnterWalletName);
                        walletName = Console.ReadLine();

                        Console.Write(EnterPassword);
                        password = Console.ReadLine();

                        Receive(password, walletName);
                        break;
                    case "balance":
                        var result = GetPasswordWalletNameAndAddress();
                        Balance(result.walletName, result.password, result.walletAddress);
                        break;
                    case "history":
                        result = GetPasswordWalletNameAndAddress();
                        History(result.walletName, result.password, result.walletAddress);
                        break;
                    case "send":
                        result = GetPasswordWalletNameAndAddress();

                        Console.Write("Select outpoint (transaction ID): ");
                        var outPoint = Console.ReadLine();

                        Send(result.walletName, result.password, result.walletAddress, outPoint);
                        break;
                }

                Console.WriteLine(EnterCommand);
                input = Console.ReadLine().ToLower().Trim();
            }
        }

        private static void Create()
        {          
            string password;
            string confirmedPassword;

            do
            {
                Console.Write(EnterPassword);
                password = Console.ReadLine();
                Console.Write(ConfirmPassword);
                confirmedPassword = Console.ReadLine();

                if (password != confirmedPassword)
                {
                    Console.WriteLine(PasswordDoesntMatch);
                }

            } while (password != confirmedPassword);

            var availableJsonName = true;

            while (availableJsonName)
            {
                try
                {
                    Console.Write(EnterWalletName);
                    var walletName = Console.ReadLine();

                    // Create mnemonic phrase
                    Mnemonic mnemonic;
                    var safe = Safe.Create(out mnemonic, password, walletFilePath + walletName + ".json", currNetwork);

                    Console.WriteLine(CreatedWallet);
                    Console.WriteLine("---------");
                    Console.WriteLine(mnemonic);
                    Console.WriteLine("---------");
                    Console.WriteLine(PrivateKeys);

                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine($"Address: {safe.GetAddress(i)} -> Private key: {safe.FindPrivateKey(safe.GetAddress(i))}");
                    }

                    availableJsonName = false;
                }
                catch
                {
                    Console.WriteLine(WalletExists);
                }
            }
        }

        private static void Recover(string password, Mnemonic mnemonic, string date)
        {
            var random = new Random();
            var safe = Safe.Recover(
                mnemonic, 
                password, 
                walletFilePath + "RecoveredWalletNum" + random.Next() + ".json", 
                currNetwork, 
                DateTimeOffset.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture));

            Console.WriteLine(RecoveredWallet);
        }

        private static void Receive(string password, string walletName)
        {
            try
            {
                var loadedSafe = Safe.Load(password, walletFilePath + walletName + ".json");

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(loadedSafe.GetAddress(i));
                }
            }
            catch
            {
                Console.WriteLine(WalletNameDoesntExists);
            }
        }

        private static void Balance(string password, string walletName, string walletAddress)
        {
            LoadWallet(password, walletName);
            
            var client = new QBitNinjaClient(currNetwork);
            var totalBalance = 0m;
            var balance = client.GetBalance(BitcoinAddress.Create(walletAddress), true).Result;

            foreach (var entry in balance.Operations)
            {
                foreach (var coin in entry.ReceivedCoins)
                {
                    var amount = (Money)coin.Amount;
                    var currAmount = amount.ToDecimal(MoneyUnit.BTC);
                    totalBalance += currAmount;
                }
            }

            Console.WriteLine($"Balance of wallet: {totalBalance}");
        }

        private static void History(string password, string walletName, string walletAddress)
        {
            LoadWallet(password, walletName);

            var client = new QBitNinjaClient(currNetwork);
            var coinsReceived = client.GetBalance(BitcoinAddress.Create(walletAddress), true).Result;

            Console.WriteLine(CoinsReceived);

            foreach (var entry in coinsReceived.Operations)
            {
                foreach (var coin in entry.ReceivedCoins)
                {
                    var amount = (Money)coin.Amount;
                    Console.WriteLine($"Transaction ID: {coin.Outpoint}; Received coins: {amount.ToDecimal(MoneyUnit.BTC)}");
                }
            }

            Console.WriteLine(CoinsReceivedEnd);
        }

        private static (string walletName, string password, string walletAddress) GetPasswordWalletNameAndAddress()
        {
            Console.Write(EnterWalletName);
            var walletName = Console.ReadLine();

            Console.Write(EnterPassword);
            var password = Console.ReadLine();

            Console.Write(EnterWalletAddress);
            var walletAddress = Console.ReadLine();

            return (walletName, password, walletAddress);
        }

        private static void LoadWallet(string password, string walletName)
        {
            try
            {
                var loadedSafe = Safe.Load(password, walletFilePath + walletName + ".json");
            }
            catch
            {
                Console.WriteLine(WrongWalletOrPassword);
            }
        }

        private static void Send(string walletName, string password, string walletAddress, string outPoint)
        {
            BitcoinExtKey privateKey = null;

            try
            {
                var loadedSave = Safe.Load(password, walletFilePath + walletName + ".json");

                for (int i = 0; i < 10; i++)
                {
                    if (loadedSave.GetAddress(i).ToString() == walletAddress)
                    {
                        Console.Write(EnterPrivateKey);
                        privateKey = new BitcoinExtKey(Console.ReadLine());

                        if (!privateKey.Equals(loadedSave.FindPrivateKey(loadedSave.GetAddress(i))))
                        {
                            Console.WriteLine(WrongPrivateKey);
                            return;
                        }

                        break;
                    }
                }
            }
            catch
            {
                Console.WriteLine(WrongWalletOrPassword);
            }

            var client = new QBitNinjaClient(currNetwork);
            var balance = client.GetBalance(BitcoinAddress.Create(walletAddress), false).Result;
            OutPoint outPointToSpend = null;

            foreach (var entry in balance.Operations)
            {
                foreach (var coin in entry.ReceivedCoins)
                {
                    if (coin.Outpoint.ToString().Substring(0, coin.Outpoint.ToString().Length - 2) == outPoint)
                    {
                        outPointToSpend = coin.Outpoint;
                        break;
                    }
                }
            }

            // Build a transaction! :))
            var transaction = new Transaction();
            transaction.Inputs.Add(new TxIn
            {
                PrevOut = outPointToSpend
            });

            Console.Write(EnterAddressToSend);
            var addressToSendTo = Console.ReadLine();

            var hallOfTheMakersAddress = BitcoinAddress.Create(addressToSendTo);

            Console.Write(EnterAmountToSend);
            var amountToSend = decimal.Parse(Console.ReadLine());

            var hallOfTheMarkesTxOut = new TxOut
            {
                Value = new Money(amountToSend, MoneyUnit.BTC),
                ScriptPubKey = hallOfTheMakersAddress.ScriptPubKey
            };

            Console.Write(EnterAmountToGetBack);
            var amountToGetBack = decimal.Parse(Console.ReadLine());

            var changeBackTxOut = new TxOut
            {
                Value = new Money(amountToGetBack, MoneyUnit.BTC),
                ScriptPubKey = privateKey.ScriptPubKey
            };

            transaction.Outputs.Add(hallOfTheMarkesTxOut);
            transaction.Outputs.Add(changeBackTxOut);

            Console.Write(EnterMessage);
            var message = Console.ReadLine();
            var bytes = Encoding.UTF8.GetBytes(message);

            transaction.Outputs.Add(new TxOut
            {
                Value = Money.Zero,
                ScriptPubKey = TxNullDataTemplate.Instance.GenerateScriptPubKey(bytes)
            });

            // Sign the transaction and check everything went smoothly
            transaction.Inputs[0].ScriptSig = privateKey.ScriptPubKey;
            transaction.Sign(privateKey, false);
            var broadcastResponse = client.Broadcast(transaction).Result;

            if (broadcastResponse.Success)
            {
                Console.WriteLine(TransactionSent);
            }
            else
            {
                Console.WriteLine(SomethingWentWrong);
            }
        }
    }
}