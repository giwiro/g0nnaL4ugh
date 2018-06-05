using System;
namespace g0nnaL4ugh.Snitch
{
	public class Sender
	{
        public Sender()
        {
        }

        public static void SnitchPwd(string pwd)
        {
            Console.WriteLine("Pwd snitch: " + pwd);
        }

#if DEBUG      
        public static void FakeSnitchPwd(string pwd)
        {
            Console.WriteLine("Fake pwd snitch: " + pwd);
        }
#endif
	}
}