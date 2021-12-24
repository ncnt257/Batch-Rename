using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using Contract;

namespace ChangeExtensionRule
{
    public class ChangeExtensionOperation:IStringOperation
    {
        public event IStringOperation.Trigger PreviewTriggerEvent;

        public StringArgs Args { get; set; }
        public string Name => "Change Extension";

        public string Description
        {
            get
            {
                var args = Args as ChangeExtensionArgs;
                return $"Change file extension to {args.NewExtension}";
            }
        }
        public UserControl ConfigUC { get; set; }
        public bool IsChecked { get; set; }
        public ChangeExtensionOperation()
        {
            Args = new ChangeExtensionArgs();
            ConfigUC = new ChangeExtensionRuleUC(this);
            ConfigUC.DataContext = this.Args;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public IStringOperation Clone()
        {
            var oldArgs = Args as ChangeExtensionArgs;
            var clone = new ChangeExtensionOperation()
            {
                Args = new ChangeExtensionArgs()
                {
                    NewExtension = oldArgs.NewExtension
                },
                IsChecked = true,
                PreviewTriggerEvent = this.PreviewTriggerEvent
            };
            return clone;
        }

        

        public void CreateFromRaw(RawRule input)
        {
            IsChecked = input.IsChecked;
            Args = new ChangeExtensionArgs()
            {
                NewExtension = input.AgrList[0]
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
            var args = Args as ChangeExtensionArgs;
            resultList.Add(args.NewExtension);
            return resultList;
        }
        public string Operate(string origin, bool isFile)
        {
            throw new NotImplementedException();
        }



    }
}
