using Contract;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ReplaceCharacter
{
    public class ReplaceCharacterOperation : IStringOperation
    {
        public event IStringOperation.Trigger PreviewTriggerEvent;
        public StringArgs Args { get; set; }
        public string Name => "Replace character";

        public string Description
        {
            get
            {
                var args = Args as ReplaceCharacterArgs;
                string result = "Change from: ";
                foreach (var character in args.From)
                {
                    result += "\"" + character + "\",";
                }

                result += "to: " + "\"" + args.To + "\"";
                return result;
            }
        }

        public UserControl ConfigUC { get; set; }
        public bool IsChecked { get; set; }

        public bool IsValidParams
        {
            get
            {
                var args = Args as ReplaceCharacterArgs;
                return args.From.Count >= 1;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public ReplaceCharacterOperation()
        {
            Args = new ReplaceCharacterArgs();
            ConfigUC = new ReplaceCharacterUC(this);
            ConfigUC.DataContext = this.Args;
        }
        public IStringOperation Clone()
        {
            var oldArgs = Args as ReplaceCharacterArgs;
            var clone = new ReplaceCharacterOperation()
            {
                Args = new ReplaceCharacterArgs()
                {
                    To = oldArgs.To,
                    From = oldArgs.From
                },
                IsChecked = true,
                PreviewTriggerEvent = this.PreviewTriggerEvent
            };
            return clone;
        }

        public string Operate(string origin, bool isFile)
        {
            if (!IsValidParams) return origin;
            var args = Args as ReplaceCharacterArgs;
            char toCharacter = args.To[0];
            if (!isFile)
            {
                string result = origin;
                foreach (var character in args.From)
                {
                    result = result.Replace(character, args.To);
                }
                return result;
            }
            Regex ext = new Regex("[.]\\w{2,}$");
            string extend = ext.Match(origin).ToString();
            int idx = origin.IndexOf(extend);
            string nameWithoutExtend = origin.Substring(0, idx);

            foreach (var character in args.From)
            {
                char temp = character[0];
                nameWithoutExtend = nameWithoutExtend.Replace(temp, args.To[0]);
            }
            string newName = nameWithoutExtend + extend;
            return newName;
        }

        public void CreateFromRaw(RawRule input)
        {
            IsChecked = input.IsChecked;
            int i = 0;
            string tempTo = "";
            BindingList<string> tempFrom = new BindingList<string>();
            foreach (var info in input.AgrList)
            {
                if (i == 0)
                {
                    tempTo = info;
                    i++;
                    continue;
                }
                tempFrom.Add(info);
                i++;
            }

            Args = new ReplaceCharacterArgs()
            {
                To = tempTo,
                From = tempFrom
            };
            ConfigUC.DataContext = this.Args;
        }

        public List<string> GetStringAgrs()
        {
            List<string> resultList = new List<string>();
            var args = Args as ReplaceCharacterArgs;
            resultList.Add(args.To.ToString());
            foreach (var character in args.From)
            {
                resultList.Add(character.ToString());
            }
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
        }
    }
}
