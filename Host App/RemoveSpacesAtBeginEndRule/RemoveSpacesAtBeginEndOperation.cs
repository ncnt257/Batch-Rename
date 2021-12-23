using Contract;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace RemoveSpacesAtBeginEndRule
{
    public class RemoveSpacesAtBeginEndOperation : IStringOperation
    {
        public event IStringOperation.Trigger PreviewTriggerEvent;
        public StringArgs Args { get; set; }
        public string Name { get => "Trim"; }
        public string Description
        {
            get
            {
                return "Remove spaces at begin and end of file name";
            }

        }
        public UserControl ConfigUC { get; set; }
        public bool IsChecked 
        { 
            get;
            set;
        }
        public RemoveSpacesAtBeginEndOperation()
        {
            Args = new RemoveSpacesAtBeginEndAgrs();
            ConfigUC = new RemoveSpacesAtBeginEndUC();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IStringOperation Clone()
        {
            var clone = new RemoveSpacesAtBeginEndOperation()
            {
                IsChecked = true,
                PreviewTriggerEvent = this.PreviewTriggerEvent
            };
            return clone;
        }

        public void CreateFromRaw(RawRule input)
        {
            IsChecked = input.IsChecked;
            Args = new RemoveSpacesAtBeginEndAgrs();
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
            var args = Args as RemoveSpacesAtBeginEndAgrs;
            resultList.Add(args.Argument);
            return resultList;
        }

        public string Operate(string origin, bool isFile)
        {
            if (!isFile)
            {
                return origin.Trim();
            }

            Regex ext = new Regex("[.]\\w");
            Match m3 = ext.Match(origin);
            int extendIdx = origin.LastIndexOf(m3.ToString());
            string extend = origin.Substring(extendIdx);

            string removedExtendName = origin.Substring(0, extendIdx);

            return removedExtendName.Trim() + extend;
        }
    }
}
