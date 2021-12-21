using Contract;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;

namespace Batch_Rename
{

    public partial class MainWindow : Window
    {
        //Biến global
        //Danh sách files
        //BindingList<string> filepaths = new BindingList<string>();
        BindingList<CFile> filepaths = new BindingList<CFile>();
        Dictionary<string, IStringOperation> _prototypes = new Dictionary<string, IStringOperation>();
        BindingList<IStringOperation> _actions = new BindingList<IStringOperation>();
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FilesListView.ItemsSource = filepaths;


            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            var fis = new DirectoryInfo(exePath).GetFiles("*.dll");


            foreach (var f in fis) // Lần lượt duyệt qua các file dll
            {
                var assembly = Assembly.LoadFile(f.FullName);
                var types = assembly.GetTypes();
                foreach (var t in types)
                {

                    if (t.IsClass && typeof(IStringOperation).IsAssignableFrom(t))
                    {
                        IStringOperation c = (IStringOperation)Activator.CreateInstance(t);//do các luật có hàm tạo, new Args rồi nên không cần load class Args như trước nữa

                        _prototypes.Add(c.Name, c);//Replace, AddPrefix,...
                    }

                }
            }
            RulesComboBox.ItemsSource = _prototypes;//bản mẫu cho người dùng xem, nếu người dùng Add thì clone ra
            RulesListView.ItemsSource = _actions;//là Binding list, thêm xóa sửa _action thì giao diện tự cập nhập
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {

            foreach (var filepath in filepaths)
            {
                filepath.Rename(CopyToTextBlock.Text != "" ? CopyToTextBlock.Text : filepath.Path);
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
                            IsFile = false // folder
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

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            var element =
                RulesComboBox.SelectedItem is KeyValuePair<string, IStringOperation> ? (KeyValuePair<string, IStringOperation>)RulesComboBox.SelectedItem : default;//selected item có kiểu keyValue pair<string,IStringOperation>, ép kiểu lại để xài
            var action = element.Value;//value là luật ( IStringOperation)
            _actions.Add(action.Clone());
        }


        private void RulesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var operation =
               RulesListView.SelectedItem as IStringOperation;
            if (operation is not null)//Luc mà delete xong, thằng event này sủa dơ, nên cần check null để app k crash
            {
                RuleConfigContent.Content = operation.ConfigUC;
                var s = new GridLength(290);
                if (RuleConfigColumn.Width.Value < 290)
                {
                    RuleConfigColumn.Width = s;
                }

            }
        }

        private void RemoveRule_Click(object sender, RoutedEventArgs e)
        {
            var index = RulesListView.SelectedIndex;
            if (Equals(RuleConfigContent.Content, _actions[index].ConfigUC))
            {
                RuleConfigContent.Content = "";
            }
            _actions.RemoveAt(index);
        }

        private void MoveUpButtonMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            //temp for dev
            PreviewTrigger();
        }

        private void PreviewTrigger()
        {
            foreach (var filepath in filepaths)
            {
                if (filepath.IsChecked)
                {
                    var previewName = filepath.Name;
                    foreach (var stringOperation in _actions)
                    {
                        if (stringOperation.IsChecked)
                        {
                            previewName = stringOperation.Operate(previewName, filepath.IsFile);
                        }

                    }

                    filepath.PreviewName = previewName;
                }

            }
        }
    }
}
