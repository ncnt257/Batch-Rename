using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PascalCase
{
    public class PascalCaseOperation: IStringOperation
    {
        public event IStringOperation.Trigger PreviewTriggerEvent;
        public StringArgs Args { get; set; }
        public string Name { get => "Pascal case"; }
        public string Description
        {
            get
            {
                return "Convert to pascal case";
            }

        }
        public UserControl ConfigUC { get; set; }
        public bool IsChecked
        {
            get;
            set;
        }
        public PascalCaseOperation()//đừng quên cài đặt constructor
        {
            Args = new PascalCaseArgs();
            ConfigUC = new PascalCaseUC();
            ConfigUC.DataContext = this.Args;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsValidParams => true; //do luật này k có tham số, trigger preview name cũng chẳng sao, những luật khác thì nhớ xem xét

        public IStringOperation Clone()
        {
            var clone = new PascalCaseOperation()
            {
                IsChecked = true,
                PreviewTriggerEvent = this.PreviewTriggerEvent
            };
            return clone;
        }

        public void CreateFromRaw(RawRule input)
        {
            IsChecked = input.IsChecked;
            Args = new PascalCaseArgs();
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
            var args = Args as PascalCaseArgs;
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
                //Tách giữa các kí tự kp chữ hoặc số
                string[] parts1 = Regex.Split(origin, @"[^a-zA-X1-9]+");

                //Viết hoa chữ đầu
                string ret1 = string.Join(" ", parts1);
                ret1 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(ret1.ToLower());

                //Bỏ spaces
                parts1 = ret1.Split(" ");
                ret1 = string.Join("", parts1);

                return ret1;
            }

            Regex ext = new Regex(@"(\.[^.]*)$");
            Match m3 = ext.Match(origin);
            int extendIdx = origin.LastIndexOf(m3.ToString());
            string extend = origin.Substring(extendIdx);

            string removedExtendName = origin.Substring(0, extendIdx);
            
            //Tách giữa các kí tự kp chữ hoặc số
            string[] parts = Regex.Split(removedExtendName, @"[^a-zA-X1-9]+");
            
            //Viết hoa chữ đầu
            string ret = string.Join(" ", parts);
            ret = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(ret.ToLower());

            //Bỏ spaces
            parts = ret.Split(" ");
            ret = string.Join("", parts);

            return ret+extend;
        }

    }
}
