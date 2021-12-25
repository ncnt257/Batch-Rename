using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Lowercase_RemoveSpaces
{
    public class Lowercase_RemoveSpacesOperation: IStringOperation
    {
        public event IStringOperation.Trigger PreviewTriggerEvent;
        public StringArgs Args { get; set; }
        public string Name { get => "Lowercase & Remove spaces"; }
        public string Description
        {
            get
            {
                return "Convert all characters to lowercase and remove all spaces";
            }

        }
        public UserControl ConfigUC { get; set; }
        public bool IsChecked
        {
            get;
            set;
        }
        public Lowercase_RemoveSpacesOperation()//đừng quên cài đặt constructor
        {
            Args = new Lowercase_RemoveSpacesArgs();
            ConfigUC = new Lowercase_RemoveSpacesUC();
            ConfigUC.DataContext = this.Args;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsValidParams => true; //do luật này k có tham số, trigger preview name cũng chẳng sao, những luật khác thì nhớ xem xét

        public IStringOperation Clone()
        {
            var clone = new Lowercase_RemoveSpacesOperation()
            {
                IsChecked = true,
                PreviewTriggerEvent = this.PreviewTriggerEvent
            };
            return clone;
        }

        public void CreateFromRaw(RawRule input)
        {
            IsChecked = input.IsChecked;
            Args = new Lowercase_RemoveSpacesArgs();
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
            var args = Args as Lowercase_RemoveSpacesArgs;
            resultList.Add(args.Argument);
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
            if (!isFile)
            {
                origin=Regex.Replace(origin, @"\s+", "");
                return origin.ToLower();
            }

            Regex ext = new Regex("[.]\\w$");
            Match m3 = ext.Match(origin);
            int extendIdx = origin.LastIndexOf(m3.ToString());
            string extend = origin.Substring(extendIdx);

            string removedExtendName = origin.Substring(0, extendIdx);

            removedExtendName=Regex.Replace(removedExtendName, @"\s+", "");

            return removedExtendName.ToLower() + extend;
        }

        List<string> IStringOperation.GetStringAgrs()
        {
            throw new NotImplementedException();
        }
    }
}
