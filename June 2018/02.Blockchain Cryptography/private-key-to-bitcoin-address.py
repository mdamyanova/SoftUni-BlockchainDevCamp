import bitcoin, hashlib, binascii


# Returns public key from given private key in hex format
def private_key_to_public_key(privateKeyHex: str) -> (int, int):
    privateKey = int(privateKeyHex, 16)
    return bitcoin.fast_multiply(bitcoin.G, privateKey)


# Returns address from given public key using Bitcoin hash algorithms
def public_key_to_address(publicKey: str, magic_byte=0) -> str:
    publicKeyBytes = binascii.unhexlify(publicKey)
    sha256hash = hashlib.sha256(publicKeyBytes).digest()
    ripemd160hash = hashlib.new('ripemd160', sha256hash).digest()
    return bitcoin.bin_to_b58check(ripemd160hash, magic_byte)


# Get random private key
privateKey = bitcoin.random_key()

# Calculate the public key coordinates
publicKey = private_key_to_public_key(privateKey)

# Compress the public key
compressedPublicKey = bitcoin.compress(publicKey)

# Generate the Bitcoin address - Base58CheckEncode(RIPEMD160(SHA256(P_compressed)))
address = public_key_to_address(compressedPublicKey)
print(address)
