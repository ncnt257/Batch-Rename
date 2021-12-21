using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string origin = "    day la   1 bai hat hay    .txt";
            Regex ext = new Regex("[.]\\w");
            Match m3 = ext.Match(origin);
            int extIdx = origin.LastIndexOf(m3.ToString());
            string extend = origin.Substring(extIdx);

            string removedExtend = origin.Substring(0, extIdx);
            

            /*Regex pattern = new Regex("\\b\\w");
            Match m = pattern.Match(origin);
            int startIdx = origin.IndexOf(m.ToString());
            string removedBeginSpaces = origin.Substring(startIdx);

            int lastPoint = removedBeginSpaces.LastIndexOf(".");
            string removeExtended = removedBeginSpaces.Substring(0, lastPoint);*/

            Console.WriteLine(removedExtend.Trim()+extend);
        }
    }
}
