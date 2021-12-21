using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Contract;

namespace RemoveSpacesAtBeginEndRule
{
    public class RemoveSpacesAtBeginEndOperation : IStringOperation
    {
        public StringArgs Args { get; set; }
        public string Name {get => "Remove Spaces At Begin And End Rule";}
        public string Description
        {
            get
            {
                return "Remove spaces at begin and end of file name";
            }
        
        }
        public UserControl ConfigUC { get; set; }
            
        public RemoveSpacesAtBeginEndOperation()
        {
            Args = new RemoveSpacesAtBeginEndAgrs();
            ConfigUC = new RemoveSpacesAtBeginEndUC();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IStringOperation Clone()
        {
            var clone = new RemoveSpacesAtBeginEndOperation();
            return clone;
        }

        public void CreateFromRaw(RawRule input)
        {
            throw new NotImplementedException();
        }

        public void DescriptionChangedNotify()
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs("Description"));
        }

        public List<string> GetStringAgrs()
        {
            throw new NotImplementedException();
        }

        public string Operate(string origin)
        {
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
