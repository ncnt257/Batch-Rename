using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using Contract;

namespace AddCounterRule
{
    public class AddCounterOperation : IStringOperation
    {
        public AddCounterOperation()
        {
            Args = new AddCounterArgs();
            ConfigUC = new AddCounterRuleUC(this);
            ConfigUC.DataContext = this.Args;
        }
        public event IStringOperation.Trigger PreviewTriggerEvent;
        public StringArgs Args { get; set; }
        public string Name => "Add Counter";
        public string Description
        {
            get
            {
                if (!IsValidParams) return "";
                var args = Args as AddCounterArgs;
                return $"Add {args.Digit} digit counter from {args.Start} with step of {args.Step}";
            }
        }
        public UserControl ConfigUC { get; set; }
        public bool IsChecked { get; set; }

        public bool IsValidParams
        {
            get
            {
                var args = Args as AddCounterArgs;
                return (args.Start is not null || args.Step is not null || args.Digit is not null);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public IStringOperation Clone()
        {
            var oldArgs = Args as AddCounterArgs;
            var clone = new AddCounterOperation()
            {
                Args = new AddCounterArgs()
                {
                    Start = oldArgs.Start,
                    Step = oldArgs.Step,
                    Digit = oldArgs.Digit
                },
                IsChecked = true,
                PreviewTriggerEvent = this.PreviewTriggerEvent
            };
            return clone;
        }

        

        public static int? ToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        public void CreateFromRaw(RawRule input)
        {
            IsChecked = input.IsChecked;
            Args = new AddCounterArgs()
            {
                Start = ToNullableInt(input.AgrList[0]),
                Step = ToNullableInt(input.AgrList[1]),
                Digit = ToNullableInt(input.AgrList[2])
            };
            ConfigUC.DataContext = this.Args;
        }

        public List<string> GetStringAgrs()
        {
            List<string> resultList = new List<string>();
            var args = Args as AddCounterArgs;
            resultList.Add(args.Start.ToString());
            resultList.Add(args.Start.ToString());
            resultList.Add(args.Start.ToString());
            return resultList;
        }

        public void DescriptionChangedNotify()
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs("Description"));
            PreviewTriggerEvent?.Invoke();
        }
        public void ResetRule()
        {
            var args = Args as AddCounterArgs;
            args.Counter = args.Start;
        }
        public string Operate(string origin, bool isFile)
        {
            var args = Args as AddCounterArgs;
            if (!IsValidParams) return origin;
            if (isFile)
            {
                int index = origin.LastIndexOf(".");
                string count = args.Counter.ToString();
                if (count.Length < args.Digit)
                {
                    for (int i = count.Length; i < args.Digit; i++)
                    {
                        count = count.Insert(0, "0");
                    }
                }
                origin = origin.Insert(index, count);
            }
            else
            {
                int index = origin.Length;
                string count = args.Counter.ToString();
                if (count.Length < args.Digit)
                {
                    for (int i = count.Length; i < args.Digit; i++)
                    {
                        count = count.Insert(0, "0");
                    }
                }
                origin = origin.Insert(index, count);
            }
            args.Counter += args.Step;
            return origin;
        }

        
    }
}
