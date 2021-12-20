using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using Path = System.IO.Path;

namespace Batch_Rename
{

    public partial class MainWindow : Window
    {
        //Biến global
        //Danh sách files
        //BindingList<string> filepaths = new BindingList<string>();
        BindingList<CFile> filepaths = new BindingList<CFile>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FilesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var filepath in filepaths)
            {
                filepath.PreviewName += "la";
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FilesListView.ItemsSource = filepaths;
        }


        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var filepath in filepaths)
            {
                filepath.PreviewName+="123";
            }
            foreach (var filepath in filepaths)
            {
                filepath.Rename(CopyToTextBlock.Text!=""? CopyToTextBlock.Text:filepath.Path);
            }
        }


        private void RemoveFileButtonMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            filepaths.RemoveAt(FilesListView.SelectedIndex);
        }

        //Browse files
        private void FileExplorerButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new CommonOpenFileDialog();
            fileDialog.Multiselect = true;

            //filter browser
            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (var fileName in fileDialog.FileNames)
                {

                    //kiểm tra xem file đã tồn tại trong list chưa
                    var fn = filepaths.SingleOrDefault(f => Path.GetFullPath(f.FullName) == Path.GetFullPath(fileName));//chuyển hết về 1 dạng ful path name để so sánh chuỗi
                    if (fn == null)
                    {
                        var newFile = new CFile()
                        {
                            IsChecked = true,
                            Path = Path.GetDirectoryName(fileName),
                            Name = Path.GetFileName(fileName),
                            PreviewName = Path.GetFileName(fileName),
                            IsFile = true // folder

                        };
                        filepaths.Add(newFile);
                    }
                }
            }


        }



        private void FolderExplorerButton_Click(object sender, RoutedEventArgs e)
        {

            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.IsFolderPicker = true;
            folderDialog.Multiselect = true;

            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (var folderName in folderDialog.FileNames)
                {

                    //kiểm tra xem file đã tồn tại trong list chưa
                    var fn = filepaths.SingleOrDefault(f => Path.GetFullPath(f.FullName) == Path.GetFullPath(folderName));//chuyển hết về 1 dạng ful path name để so sánh chuỗi
                    if (fn == null)
                    {
                        var newFile = new CFile()
                        {
                            IsChecked = true,
                            Path = Path.GetDirectoryName(folderName),
                            Name = Path.GetFileName(folderName),
                            PreviewName = Path.GetFileName(folderName),
                            IsFile=false // folder
                        };
                        filepaths.Add(newFile);
                    }
                }
            }





            //Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Multiselect = true;

                ////filter browser
                //openFileDialog.Filter = "ALL Files|*.*|TEXT FILE | *.txt|PDF FILE |*.pdf|Folders|\n";

                //openFileDialog.ValidateNames = false;
                //openFileDialog.CheckFileExists = false;
                //openFileDialog.CheckPathExists = true;



                //// Always default to Folder Selection.
                //openFileDialog.FileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                //bool? response = openFileDialog.ShowDialog();

                //if (response == true)
                //{
                //    int fileCount = openFileDialog.FileNames.Count();
                //    for (int i = 0; i < fileCount; i++)
                //    {
                //        //check if duplicate then abort that choosen file
                //        bool flag = true;
                //        for (int j = 0; j < filepaths.Count(); j++)
                //        {
                //            if (filepaths[j].FullName == openFileDialog.FileNames[i])
                //            {
                //                flag = false;
                //                break;
                //            }
                //        }
                //        if (flag)
                //        {
                //            //add file vao array
                //            CFile temp = new CFile
                //            {
                //                //GetFileName trả về tên file hoặc tên folder lun
                //                Name = System.IO.Path.GetFileName(openFileDialog.FileNames[i]),
                //                PreviewName = System.IO.Path.GetFileName(openFileDialog.FileNames[i]),
                //                //GetDirectoryName trả về tên đường dẫn đén file hoặc folder đó
                //                Path = System.IO.Path.GetDirectoryName(openFileDialog.FileNames[i]),
                //                IsChecked = true

                //            };
                //            filepaths.Add(temp);
                //        }

                //    }
                //}
        }
    }
}
