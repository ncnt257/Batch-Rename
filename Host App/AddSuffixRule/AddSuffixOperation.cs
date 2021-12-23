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

        public string Name => "Add Suffix";

        public string Description
        {
            get
            {
                var args = Args as AddSuffixArgs;
                return $"Add suffix of {args.Suffix}";
            }
        }

        public UserControl ConfigUC
        {
            get; set;
        }
        public bool IsChecked
        {
            get;
            set;
        }

        public AddSuffixOperation()
        {
            Args = new AddSuffixArgs();
            ConfigUC = new AddSuffixRuleUC(this);
            ConfigUC.DataContext = this.Args;
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
                },
                IsChecked = true
            };
            return clone;
        }

        public void CreateFromRaw(RawRule input)
        {
            IsChecked = input.IsChecked;
            Args = new AddSuffixArgs()
            {
                Suffix = input.AgrList[0]
            };
            ConfigUC.DataContext = this.Args;
        }

        public void DescriptionChangedNotify()
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs("Description"));
        }

        public List<string> GetStringAgrs()
        {
            List<string> resultList = new List<string>();
            var args = Args as AddSuffixArgs;
            resultList.Add(args.Suffix);
            return resultList;
        }

        public string Operate(string origin, bool isFile)
        {
            var args = Args as AddSuffixArgs;
            string suffix = args.Suffix;
            if (!isFile)
            {
                return origin + suffix;
            }
            Regex ext = new Regex("[.]\\w{2,}$");
            string extend = ext.Match(origin).ToString();
            int idx = origin.IndexOf(extend);
            string newName = origin.Substring(0, idx) + suffix + extend;
            return newName;
        }

        List<string> IStringOperation.GetStringAgrs()
        {
            List<string> resultList = new List<string>();
            var args = Args as AddSuffixArgs;
            resultList.Add(args.Suffix);
            return resultList;
        }
    }
}
