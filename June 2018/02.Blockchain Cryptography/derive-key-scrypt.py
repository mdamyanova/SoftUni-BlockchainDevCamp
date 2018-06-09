# TODO
import scrypt, os, binascii

password = 'p@$$w0rD~3'
salt = os.urandom(32)
print('Salt: ', binascii.hexlify(salt))

key = scrypt.hash(password, salt, 16384, 16, 1, 32)
print('Derived key: ', binascii.hexlify(key))