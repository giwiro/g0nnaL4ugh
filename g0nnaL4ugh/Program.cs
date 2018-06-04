using System;
using g0nnaL4ugh.Crypto;

namespace g0nnaL4ugh
{
    class Program
    {
        public static void Main(string[] args)
        {
            Encrypter encrypter = new Encrypter();
            string[] paths = { "/home/giwiro/CLionProjects" };
            Spider spider = new Spider(paths, encrypter);
            spider.BuildNodesFromPath();
            Console.WriteLine("H3llo W0rld!");
        }
    }
}
