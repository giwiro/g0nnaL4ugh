﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace g0nnaL4ugh.Crypto
{
    public class Utilities
    {
        public const int SaltSizeBytes = 32;
        public const int BlockSizeBits = 256;
      
        public static byte[] ConvertUTF2Bytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
      
        public static string ConvertBytes2UTF(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string ConvertBytes2B64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] TakeFirstsBytes(byte[] a, int length)
        {
            IEnumerable<byte> firstXBytes = a.Take(length);
            return firstXBytes.ToArray();
        }

        public static byte[] TakeFromBytes(byte[] a, int from)
        {
            IEnumerable<byte> lastBytes = a.Skip(from);
            return lastBytes.ToArray();
        }

        public static byte[] ConcatBytes(byte[] a, byte[] b)
        {
            /* The waes to concat 2 or more byte arrays are:
            *  - System.Arra.Copy
            *  - System.Buffer.BlockCopy
            *  - IEnumerable<byte> with yield operator
            *  - IEnumerable<byte> with LINQ's Concat
            * 
            *  The LINQ's one is the fastest
            *  
            *  https://stackoverflow.com/a/415396
            */
            IEnumerable<byte> rv = a.Concat(b);
            return rv.ToArray();
        }
    }
}
