using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        void test() {
            string origin = "     day là một bài hát hay   .txt";
            Regex ext = new Regex("[.]\\w");
            Match m3 = ext.Match(origin);
            int extIdx = origin.LastIndexOf(m3.ToString());
            string extend = origin.Substring(extIdx);
            //string extend = ext.Replace(origin, "");

            Regex pattern = new Regex("\\b\\w");
            Match m = pattern.Match(origin);
            int startIdx = origin.IndexOf(m.ToString());
            string removedBeginSpaces = origin.Substring(startIdx);

            int lastPoint = removedBeginSpaces.LastIndexOf(".");
            string removeExtended = removedBeginSpaces.Substring(0, lastPoint);

            Regex pattern2 = new Regex("\\w\\b");
            Match m2 = pattern2.Match(removeExtended);
            int endIdx = removeExtended.LastIndexOf(m2.ToString());
            string result = removeExtended.Substring(0, endIdx + 1);

            Console.WriteLine(result + extend);
        }
        static void Main(string[] args)
        {
            string underLine = "_";
            string space = " ";
            string origin = "   day laf 1 bai_hat_ hay_";
            Regex from = new Regex($"[{space}{underLine}]");
            string result = from.Replace(origin, "-");
        }
    }
}
