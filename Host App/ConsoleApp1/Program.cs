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
            int? a = null;
            string b = a.ToString();
            Console.WriteLine(b);

            int? c = ToNullableInt(b);
            Console.WriteLine(c);
        }
    }
}
