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
            string origin ="test.a.s.txt";
            string pattern = @"(\.[^.]+)$";
            var regex = new Regex(pattern);
            if (regex.IsMatch(origin))
            {
                origin = regex.Replace(origin, $".bruh");

            }
            else
            {
                origin += ".bruh";
            }
            Console.WriteLine(origin);
        }
    }
}
