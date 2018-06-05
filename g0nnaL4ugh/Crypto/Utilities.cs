using System;
using System.Text;

namespace g0nnaL4ugh.Crypto
{
	public class Utilities
	{
		public static string ConvertBytes2UTF(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}

        public static string ConvertBytes2B64(byte[] bytes)
		{
			return Convert.ToBase64String(bytes);
		}
	}
}
