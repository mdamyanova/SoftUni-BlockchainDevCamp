var bitcoin = require('bitcoinjs-lib');

var keyPair = bitcoin.ECPair.makeRandom();

var privateKey = keyPair.toWIF();
console.log(privateKey);

var publicKey = keyPair.getAddress();
console.log(publicKey);