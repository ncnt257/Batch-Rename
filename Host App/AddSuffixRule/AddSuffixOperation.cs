using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace AddSuffixRule
{
    public class AddSuffixOperation : IStringOperation
    {
        public StringArgs Args { get; set; }

        public string Name => "Add Suffix Rule";

        public string Description
        {
            get
            {
                var args = Args as AddSuffixArgs;
                return $"Add suffix of {args.Suffix}";
            }
        }

        public UserControl ConfigUC { get; set; }
        public bool IsChecked { get; set; }

        public AddSuffixOperation()
        {
            Args = new AddSuffixArgs();
            ConfigUC = new AddSuffixRuleUC(this);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public IStringOperation Clone()
        {
            var oldArgs = Args as AddSuffixArgs;
            var clone = new AddSuffixOperation()
            {
                Args = new AddSuffixArgs()
                {
                    Suffix = oldArgs.Suffix
                }
            };
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
            var args = Args as AddSuffixArgs;
            string suffix = args.Suffix;
            //string origin = "day la mot bai hat vui.txt";
            Regex ext = new Regex("[.]\\w{2,}$");
            //Match m = ext.Match(origin);
            string extend = ext.Match(origin).ToString();
            int idx = origin.IndexOf(extend);
            string originNonExt = origin.Substring(0, idx) + suffix + extend;
            return originNonExt;
        }

        List<string> IStringOperation.GetStringAgrs()
        {
            throw new NotImplementedException();
        }
    }
}
