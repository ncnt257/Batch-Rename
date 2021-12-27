using Contract;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
namespace AddPrefixRule
{
    public class AddPrefixOperation : IStringOperation
    {
        public StringArgs Args { get; set; }
        public string Name => "Add Prefix";

        public string Description
        {
            get
            {
                var args = Args as AddPrefixArgs;
                return $"Add prefix of {args.Prefix}";
            }
        }

        public UserControl ConfigUC { get; set; }
        public bool IsChecked { get; set; }

        public event IStringOperation.Trigger PreviewTriggerEvent;

        public event PropertyChangedEventHandler? PropertyChanged;
        public bool IsValidParams => true;


        public AddPrefixOperation()//đừng quên cài đặt constructor
        {
            Args = new AddPrefixArgs();
            ConfigUC = new AddPrefixRuleUC(this);
            ConfigUC.DataContext = this.Args;
        }
        public IStringOperation Clone()
        {
            var oldArgs = Args as AddPrefixArgs;
            var clone = new AddPrefixOperation()
            {
                Args = new AddPrefixArgs()
                {
                    Prefix = oldArgs.Prefix
                },
                IsChecked = true,
                PreviewTriggerEvent = this.PreviewTriggerEvent
            };
            return clone;
        }
        public void ResetRule()
        {
        }
        public string Operate(string origin, bool isFile)
        {
            var args = Args as AddPrefixArgs;
            return args.Prefix + origin;
            
        }

        public void CreateFromRaw(RawRule input)
        {
            IsChecked = input.IsChecked;
            Args = new AddPrefixArgs()
            {
                Prefix = input.AgrList[0]
            };
            ConfigUC.DataContext = this.Args;
        }

        public List<string> GetStringAgrs()
        {
            List<string> resultList = new List<string>();
            var args = Args as AddPrefixArgs;
            resultList.Add(args.Prefix);
            return resultList;
        }

        public void DescriptionChangedNotify()
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs("Description"));
            PreviewTriggerEvent?.Invoke();
        }
    }
}
