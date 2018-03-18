var bitcoin = require('bitcoinjs-lib');

var tx = new bitcoin.TransactionBuilder();

var txId = 'aa94ab02c182214f090e99a0d57021caffd0f195a81c24602b1028b130b63e31';

// input - who is paying - prev transaction hash and index of the output
tx.addInput(txId, 0);

// output - who to pay to - payee's address, amount in satoshis
tx.addOutput("1Gokm82v6DmtwKEB8AiVhm82hyFSsEvBDK", 15000);

var privateKeyWIF = 'L1uyy5qTuGrVXrmrsvHWHgVzW9kKdrp27wBC7Vs6nZDTF2BRUVwy';
var keyPair = bitcoin.ECPair.fromWIF(privateKeyWIF);

// sign the first input with the new key
tx.sign(0, keyPair);

// build transaction, convert to hexadecimal and print it
console.log(tx.build().toHex());