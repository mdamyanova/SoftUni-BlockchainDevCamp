namespace BitcoinAddressGenerator
{
    using System;
    using System.Globalization;
    using System.Numerics;
    using System.Security.Cryptography;

    public class Startup
    {
        public static void Main()
        {
            var hexHash = "0450863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B23522CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6";
            var publicKey = HexToByte(hexHash);
            Console.WriteLine("Public Key: " + ByteToHex(publicKey));

            var publicKeySHA256 = SHA256(publicKey);
            Console.WriteLine("SHA Public Key: " + ByteToHex(publicKeySHA256));

            var publicKeySHARIPE = RIPEMD160(publicKeySHA256);
            Console.WriteLine("RIPE SHA Public Key: " + ByteToHex(publicKeySHARIPE));

            var preHashNetwork = AppendBitcoinNetwork(publicKeySHARIPE, 0);
            var publicHash = SHA256(preHashNetwork);
            Console.WriteLine("Public Hash: " + ByteToHex(publicHash));

            var publicHashHash = SHA256(publicHash);
            Console.WriteLine("Public HashHash: " + ByteToHex(publicHashHash));

            Console.WriteLine("Checksum: " + ByteToHex(publicHashHash).Substring(0, 4));

            var address = ConcatAddress(preHashNetwork, publicHashHash);
            Console.WriteLine("Address: " + ByteToHex(address));

            Console.WriteLine("Human Address: " + Base58Encode(address));
        }

        private static byte[] HexToByte(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new InvalidOperationException("Invalid HEX");
            }

            var array = new byte[hexString.Length / 2];

            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = byte.Parse(hexString.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return array;
        }

        private static string ByteToHex(byte[] publicKeySHA)
        {
            var hex = BitConverter.ToString(publicKeySHA);
            return hex;
        }

        private static byte[] SHA256(byte[] array)
        {
            var hash = new SHA256Managed();
            return hash.ComputeHash(array);
        }

        private static byte[] RIPEMD160(byte[] array)
        {
            var hash = new RIPEMD160Managed();
            return hash.ComputeHash(array);
        }

        private static byte[] AppendBitcoinNetwork(byte[] ripeHash, byte network)
        {
            var extended = new byte[ripeHash.Length + 1];
            extended[0] = network;

            Array.Copy(ripeHash, 0, extended, 1, ripeHash.Length);

            return extended;
        }

        private static byte[] ConcatAddress(byte[] ripeHash, byte[] checkSum)
        {
            var array = new byte[ripeHash.Length + 4];

            Array.Copy(ripeHash, array, ripeHash.Length);
            Array.Copy(checkSum, 0, array, ripeHash.Length, 4);

            return array;
        }

        private static string Base58Encode(byte[] array)
        {
            const string Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            var result = string.Empty;

            BigInteger encodeSize = Alphabet.Length;
            BigInteger arrayToInt = 0;

            for (int i = 0; i < array.Length; ++i)
            {
                arrayToInt = arrayToInt * 256 + array[i];
            }

            while (arrayToInt > 0)
            {
                var reminder = (int)(arrayToInt % encodeSize);
                arrayToInt /= encodeSize;
                result = Alphabet[reminder] + result;
            }

            for (int i = 0; i < array.Length && array[i] == 0; ++i)
            {
                result = Alphabet[0] + result;
            }

            return result;
        }
    }
}