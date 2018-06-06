using System;
using System.IO;

namespace g0nnaL4ugh.Snitch
{
    public class Writter
    {
        public Writter()
        {
        }

        public static void WriteBytes2File(byte[] data, string filePath)
		{
			File.WriteAllBytes(filePath, data);
		}
    }
}
