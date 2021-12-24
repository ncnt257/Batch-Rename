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
            int Start = 1;
            int Step = 1;
            int Digit = 5;
            string counter=Start.ToString();
            if (counter.Length < Digit)
            {
                for (int i = counter.Length; i < Digit; i++)
                {
                    counter = counter.Insert(0,"0");
                }
            }
            string origin = "tung.txt";
            int index = origin.LastIndexOf('.');

            origin=  origin.Insert(index, counter);
            Console.WriteLine(origin);
        }
    }
}
