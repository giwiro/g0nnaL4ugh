using System;
using System.Security.Cryptography;

namespace g0nnaL4ugh.Crypto
{
	public class Encrypter
	{
		public Encrypter()
		{
		}

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

		public string GenerateRandomPrivateKey()
		{
			// The length will be random between 23 and 28
			Random random = new Random();
			int length = random.Next(23, 28);
			byte[] randomBytes = Encrypter.GenerateTrulyRandom(length);
            return Convert.ToBase64String(randomBytes);
		}

        public static string GenerateRandomSalt(int length)
		{
			byte[] randomBytes = Encrypter.GenerateTrulyRandom(length);
            return Convert.ToBase64String(randomBytes);
		}
    }
}
