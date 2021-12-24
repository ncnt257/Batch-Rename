using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        public static int? ToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
        static void Main(string[] args)
        {
            string origin = "sadsa.dsad.ds";
            string NewExtension = ".hihi";
            string pattern = @"(\.[^.]+)$";
            var regex = new Regex(pattern);
            origin = regex.Replace(origin, NewExtension);
            Console.WriteLine(origin);
        }
    }
}
