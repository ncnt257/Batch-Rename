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
        public RemoveSpacesAtBeginEndOperation()//đừng quên cài đặt constructor
        {
            Args = new RemoveSpacesAtBeginEndAgrs();
            ConfigUC = new RemoveSpacesAtBeginEndUC();
            ConfigUC.DataContext = this.Args;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsValidParams => true; //do luật này k có tham số, trigger preview name cũng chẳng sao, những luật khác thì nhớ xem xét

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
                return origin.Trim();
            }

            Regex ext = new Regex("[.]\\w{2,}$");
            Match m3 = ext.Match(origin);
            int extentionIdx = origin.LastIndexOf(m3.ToString());

            string extention = origin.Substring(extentionIdx);

            string removedExtendName = origin.Substring(0, extentionIdx);

            return removedExtendName.Trim() + extention ;
        }
    }
}
