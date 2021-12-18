using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Batch_Rename
{
    public class CFile: INotifyPropertyChanged
    {      
        //File root chưa đường dẫn tuyệt đối
        private string _root;
        public string Root { 
            get { return _root; }
            set
            {
                _root = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Root"));
                }
            } 
        }
        //Chứa tên đã apply rules
        private string _previewName;
        public string PreviewName
        {
            get { return _previewName; }
            set
            {
                _previewName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("PreviewName"));
                }
            }
        }
        //Chứa đường dẫn mới nếu có(để copy sang)
        public string newRoot { get; set; }

        //Lấy tên hiện tại
        public string getCurrentName()
        {
            string[] parts = _root.Split(new string[] { @"\" }, StringSplitOptions.None);
            string ret = parts[parts.Length - 1];
            return ret;
        }
        public string getDir()
        {
            string[] parts = _root.Split(new string[] { @"\" }, StringSplitOptions.None);
            string currentName = parts[parts.Length - 1];
            string ret = _root.Substring(0, _root.Length - currentName.Length);

            return ret;
        }
        //Xem cái này có được check để apply rules không 
        private bool _isChecked;
        public bool isChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("isChecked"));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged = null;

    }
    public class fileToName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string root = (string)(value);
            string[] parts = root.Split(new string[] { @"\" }, StringSplitOptions.None);
            string ret = parts[parts.Length - 1];
            return ret;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class fileToRoot : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string root = (string)(value);
            string[] parts = root.Split(new string[] { @"\" }, StringSplitOptions.None);
            string currentName = parts[parts.Length - 1];
            string ret=root.Substring(0, root.Length - currentName.Length);
            return ret;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
