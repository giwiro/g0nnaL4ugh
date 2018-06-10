#! /usr/bin/env python3

import os
from Crypto.PublicKey import RSA

rootDir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
keysDir = os.path.join(rootDir, "keys")

publicKeyPath = os.path.join(keysDir, "ransom_public.key")
privateKeyPath = os.path.join(keysDir, "ransom_private.key")

key = RSA.generate(2048)
with open(privateKeyPath, "wb") as private_file:
    os.chmod(privateKeyPath, 0o0600)
    private_file.write(key.exportKey(format="PEM"))

publicKey = key.publickey()

with open(publicKeyPath, "wb") as public_file:
    public_file.write(publicKey.exportKey(format="PEM"))

