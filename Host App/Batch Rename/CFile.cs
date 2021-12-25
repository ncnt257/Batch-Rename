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
        public void Rename(string copyPath)
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
                                File.Move(FullName, $"{copyPath}/{PreviewName}",
                                    true); //ở đây không xài copy overwrite vì khi đổi tên nó thành file khác và đc copy chồng vô
                        }
                        else
                        {
                            File.Copy(FullName, $"{copyPath}/{PreviewName}", true);
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

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
