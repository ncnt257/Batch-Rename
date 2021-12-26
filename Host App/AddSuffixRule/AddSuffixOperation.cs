using Contract;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace AddSuffixRule
{
    public class AddSuffixOperation : IStringOperation
    {
        public event IStringOperation.Trigger PreviewTriggerEvent;
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

        public AddSuffixOperation()//đừng quên cài đặt constructor
        {
            Args = new AddSuffixArgs();
            ConfigUC = new AddSuffixRuleUC(this);
            ConfigUC.DataContext = this.Args;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsValidParams => true;//do luật này k có tham số, trigger preview name cũng chẳng sao, những luật khác thì nhớ xem xét

        public IStringOperation Clone()
        {
            var oldArgs = Args as AddSuffixArgs;
            var clone = new AddSuffixOperation()
            {
                Args = new AddSuffixArgs()
                {
                    Suffix = oldArgs.Suffix
                },
                IsChecked = true,
                PreviewTriggerEvent = this.PreviewTriggerEvent
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
            PreviewTriggerEvent?.Invoke();
        }

        public List<string> GetStringAgrs()
        {
            List<string> resultList = new List<string>();
            var args = Args as AddSuffixArgs;
            resultList.Add(args.Suffix);
            return resultList;
        }
        //luật nào không cần reset sau khi đổi tên thì để không như này
        public void ResetRule()
        {
        }
        public string Operate(string origin, bool isFile)
        {
            //if (!IsValidParams) return origin;
            //do IsValidParams ở luật này luôn true nên k cần thiết, những luật khác nhớ thêm dòng trên
            var args = Args as AddSuffixArgs;
            string suffix = args.Suffix;
            if (!isFile)
            {
                return origin + suffix;
            }
            Regex ext = new Regex("[.]\\w{2,}$");
            string extend = ext.Match(origin).ToString();
            int idx = origin.IndexOf(extend);
            if(idx == 0)
            {
                return origin + suffix;
            }
            string newName = origin.Substring(0, idx) + suffix + extend;
            return newName;
        }

    }
}
