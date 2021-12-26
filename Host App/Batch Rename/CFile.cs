using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Batch_Rename
{
    public class CFile: INotifyPropertyChanged
    {
        //to rename: System.IO.File.Move(FullName, PreviewName);
        public string Path { get; set; }
        public string Name { get; set; }
        public string FullName => $"{Path}/{Name}";

        public string PreviewName { get; set; }//các luật đổi tên thao tác trên thuộc tính này
        public bool IsChecked { get; set; }
        public bool IsFile { get; set; }//check if file or folder
        public bool Rename(string copyPath, bool overwrite)
        {
            try
            {

                if (IsChecked)
                {
                    if (IsFile)
                    {
                        if (copyPath == Path)//copyPath đc truyền vào là Path tức là người dùng k dùng chức năng copy to
                        {
                            if (Name != PreviewName)
                                File.Move(FullName, $"{copyPath}/{PreviewName}", overwrite);

                        }
                        else
                        {
                            File.Copy(FullName, $"{copyPath}/{PreviewName}", overwrite);
                        }
                    }
                    else
                    {
                        if (copyPath == Path)
                        {
                            if (Name != PreviewName)
                                Directory.Move(FullName, $"{copyPath}/{PreviewName}");
                        }
                        else
                        {
                            Utility.CopyFilesRecursively(FullName, $"{copyPath}/{PreviewName}");
                        }

                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
