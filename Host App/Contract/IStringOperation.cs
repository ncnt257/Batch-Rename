using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace Contract
{
    public interface IStringOperation :INotifyPropertyChanged
    {
        public StringArgs Args { get; }
        public string Name { get;}
        public string Description { get;}
        public UserControl ConfigUC { get; set; }
        public bool IsChecked { get; set; }

        public IStringOperation Clone();
        public string Operate(string origin, bool isFile);
        public void CreateFromRaw(RawRule input);
        public List<string> GetStringAgrs();
        public void DescriptionChangedNotify();
    }
}
