using System;
using System.IO;
using System.Security.Cryptography;

namespace g0nnaL4ugh.Crypto
{
    public class Decrypter
    {      
        public byte[] ExtractSalt(byte[] cipherText)
        {
            return Utilities.TakeFirstsBytes(cipherText, Utilities.SaltSizeBytes);
        }

        public byte[] ExtractBytes2Dec(byte[] cipherText)
        {
            return Utilities.TakeFromBytes(cipherText, Utilities.SaltSizeBytes);
        }

        public byte[] DecryptBytes(byte[] cipherText, byte[] password, byte[] salt)
        {
            byte[] plainText = null;
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
                    using (var cs = new CryptoStream(ms, RJM.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherText, 0, cipherText.Length);
                        cs.Close();
                    }
                    /* TODO: We need to append the salt to the encrypted bytes */
                    plainText = ms.ToArray();
                }
            }
            return plainText;
        }
    }
}
