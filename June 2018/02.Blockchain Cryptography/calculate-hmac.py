# Import hashlib and hmac for hash algorithms, binascii for converting between binary and ASCII
import hashlib, hmac, binascii


# Declare function and return HMAC hash
def hmac_sha512(key, msg):
    return hmac.new(key, msg, hashlib.sha512).digest()


key = 'devcamp'.encode('utf8')
msg = 'blockchain'.encode('utf8')
result = binascii.hexlify(hmac_sha512(key, msg))

print(result)