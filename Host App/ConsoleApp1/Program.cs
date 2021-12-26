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
            bool res = (true && true && false && true);
            Console.WriteLine(res);
        }
    }
}
