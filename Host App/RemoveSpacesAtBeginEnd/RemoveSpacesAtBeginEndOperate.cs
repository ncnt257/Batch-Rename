using System;
using System.Text.RegularExpressions;

namespace RemoveSpacesAtBeginEnd
{
    public class RemoveSpacesAtBeginEndOperate
    {
        string origin;
        public string Operate()
        {
            //string origin = "     day là một bài hát hay   .txt";
            Regex ext = new Regex("[.]\\w");
            Match m3 = ext.Match(origin);
            int extIdx = origin.LastIndexOf(m3.ToString());
            string extend = origin.Substring(extIdx);

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

            return result + extend;
        }

    }
}
