# Import hashlib for hash algorithms, binascii for converting between binary and ASCII,
# whirlpool and keccak libraries
import hashlib
import binascii
import whirlpool
from Crypto.Hash import keccak


# Encode given text to UTF-8
text = 'blockchain'
data = text.encode('utf-8')

# Get the hash of our encoded text and print it on the console
# Older algorithms were called message digests. The modern term is secure hash.

sha384hash = hashlib.sha384(data).digest()
print('SHA-384: ', binascii.hexlify(sha384hash))

sha512hash = hashlib.sha512(data).digest()
print('SHA-512: ', binascii.hexlify(sha512hash))

sha3512hash = hashlib.sha3_512(data).digest()
print('SHA3-512: ', binascii.hexlify(sha3512hash))

keccak512hash = keccak.new(data=data, digest_bits=512).digest()
print('Keccak-512: ', binascii.hexlify(keccak512hash))

whirlpool512hash = whirlpool.new(data).hexdigest()
print('Whirlpool-512: ', whirlpool512hash)
