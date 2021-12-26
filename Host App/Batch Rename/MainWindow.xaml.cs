using System;
using Contract;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using Path = System.IO.Path;

namespace Batch_Rename
{

    public partial class MainWindow : Window
    {
        //Biến global
        //Danh sách files
        //BindingList<string> filepaths = new BindingList<string>();
        public string sFileName = "";
        BindingList<CFile> filepaths = new BindingList<CFile>();
        public Dictionary<string, IStringOperation> _prototypes = new Dictionary<string, IStringOperation>();
        BindingList<IStringOperation> _actions = new BindingList<IStringOperation>();
        private RenameRuleFactory renameRuleFactory;
        
        public MainWindow()
        {
            InitializeComponent();
            renameRuleFactory = new RenameRuleFactory(this);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FilesListView.ItemsSource = filepaths;
            RulesComboBox.ItemsSource = renameRuleFactory.Prototypes;//bản mẫu cho người dùng xem, nếu người dùng Add thì clone ra
            RulesListView.ItemsSource = _actions;//là Binding list, thêm xóa sửa _action thì giao diện tự cập nhập
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            var count = 0;
            foreach (var filepath in filepaths)
            {
                if (filepath.IsChecked) count++;
                filepath.Rename(CopyToTextBlock.Text != "" ? CopyToTextBlock.Text : filepath.Path);
            }
            MessageBox.Show($"Rename {count} file(s)/folder(s) successfully");
            filepaths.Clear();
        }

        private void RemoveFileButtonMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            filepaths.RemoveAt(FilesListView.SelectedIndex);
            PreviewTrigger();
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
            PreviewTrigger();
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
                PreviewTrigger();
            }

            /*Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            filter browser
            openFileDialog.Filter = "ALL Files|*.*|TEXT FILE | *.txt|PDF FILE |*.pdf|Folders|\n";

            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;



            Always default to Folder Selection.
            openFileDialog.FileName = System.IO.Path.GetFileName(openFileDialog.FileName);
            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                int fileCount = openFileDialog.FileNames.Count();
                for (int i = 0; i < fileCount; i++)
                {
                    //check if duplicate then abort that choosen file
                    bool flag = true;
                    for (int j = 0; j < filepaths.Count(); j++)
                    {
                        if (filepaths[j].FullName == openFileDialog.FileNames[i])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        //add file vao array
                        CFile temp = new CFile
                        {
                            //GetFileName trả về tên file hoặc tên folder lun
                            Name = System.IO.Path.GetFileName(openFileDialog.FileNames[i]),
                            PreviewName = System.IO.Path.GetFileName(openFileDialog.FileNames[i]),
                            //GetDirectoryName trả về tên đường dẫn đén file hoặc folder đó
                            Path = System.IO.Path.GetDirectoryName(openFileDialog.FileNames[i]),
                            IsChecked = true

                        };
                        filepaths.Add(temp);
                    }

                }
            }*/
        }

        private void CopyToButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.IsFolderPicker = true;

            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folderName = folderDialog.FileName;
                CopyToTextBlock.Text = folderName;
            }
        }

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            var element =
                RulesComboBox.SelectedItem is KeyValuePair<string, IStringOperation> ? (KeyValuePair<string, IStringOperation>)RulesComboBox.SelectedItem : default;//selected item có kiểu keyValue pair<string,IStringOperation>, ép kiểu lại để xài
            var action = element.Value;//value là luật ( IStringOperation)
            _actions.Add(action.Clone());
            PreviewTrigger();
        }

        private void RulesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var operation =
               RulesListView.SelectedItem as IStringOperation;
            if (operation is not null)//Luc mà delete xong, thằng event này sủa dơ, nên cần check null để app k crash
            {
                var userControl = operation.ConfigUC;
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
            PreviewTrigger();
        }

        private void MoveUpButtonMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var index = FilesListView.SelectedIndex;
            if (index > 0)
            {
                var temp = filepaths[index];
                filepaths.RemoveAt(index);
                filepaths.Insert(index - 1, temp);
            }
            PreviewTrigger();
        }

        public void PreviewTrigger()
        {
            List<Action> rulesReseter =new List<Action>();
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
                            rulesReseter.Add(stringOperation.ResetRule); //luật counter cần được reset
                        }

                    }
                    filepath.PreviewName = previewName;
                }
            }

            foreach (var action in rulesReseter)
            {
                action.Invoke();
            }
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            _actions.Clear();
            RuleConfigContent.Content = "";
            PreviewTrigger();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (sFileName == "")
            {
                SaveAs_click(sender, e);
                return;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<RawRule>));
            FileStream stream = File.Create(sFileName);
            List<RawRule> outputFileRules = new List<RawRule>();
            foreach (var action in _actions)
            {
                RawRule temp = new RawRule()
                {
                    RuleName = action.Name,
                    AgrList = action.GetStringAgrs(),
                    IsChecked = action.IsChecked
                };
                outputFileRules.Add(temp);
            }
            serializer.Serialize(stream, outputFileRules);
            stream.Dispose();
        }

        private void ChoosePresetButton_OnClick(object sender, RoutedEventArgs e)
        {
            _actions.Clear();
            OpenFileDialog browseDialog = new OpenFileDialog();
            browseDialog.Filter = "XML Files (*.xml*)|*.xml*";
            browseDialog.FilterIndex = 1;
            browseDialog.Multiselect = false;

            if (browseDialog.ShowDialog() != true)
            {
                MessageBox.Show("Can't open file !");
                return;
            }

            sFileName = browseDialog.FileName;
            RuleSetNameLabel.Content = Path.GetFileName(sFileName);
            XmlSerializer serializer = new XmlSerializer(typeof(List<RawRule>));
            FileStream streamRead = File.OpenRead(sFileName);
            var results = (List<RawRule>)(serializer.Deserialize(streamRead));

            foreach (var rawRule in results)
            {
                var temp = renameRuleFactory.Create(rawRule);
                _actions.Add(temp);
            }
            PreviewTrigger();
        }

        private void SaveAs_click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files | *.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                RuleSetNameLabel.Content = Path.GetFileName(sFileName);
                XmlSerializer serializer = new XmlSerializer(typeof(List<RawRule>));
                FileStream stream = File.Create(filePath);
                List<RawRule> outputFileRules = new List<RawRule>();
                foreach (var action in _actions)
                {
                    RawRule temp = new RawRule()
                    {
                        IsChecked = action.IsChecked,
                        RuleName = action.Name,
                        AgrList = action.GetStringAgrs()
                    };
                    outputFileRules.Add(temp);
                }
                serializer.Serialize(stream, outputFileRules);
                stream.Dispose();
            }
        }

        private void MoveRuleUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            var index = RulesListView.SelectedIndex;
            if (index > 0)
            {
                var temp = _actions[index];
                _actions.RemoveAt(index);
                _actions.Insert(index - 1, temp);
            }
            PreviewTrigger();
        }
        private void MoveRuleDownButton_OnClick(object sender, RoutedEventArgs e)
        {
            var index = RulesListView.SelectedIndex;
            if (index < _actions.Count - 1)
            {
                var temp = _actions[index];
                _actions.RemoveAt(index);
                _actions.Insert(index + 1, temp);
            }
            PreviewTrigger();
        }

        private void DisableCopyToBtn_Click(object sender, RoutedEventArgs e)
        {
            CopyToTextBlock.Text = "";
        }

        private void FileCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PreviewTrigger();
        }

        private void CheckToUse_Checked(object sender, RoutedEventArgs e)
        {

            PreviewTrigger();
        }

        private void CheckToUse_Unchecked(object sender, RoutedEventArgs e)
        {
            PreviewTrigger();
        }

        private void FileCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PreviewTrigger();
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            var index = FilesListView.SelectedIndex;
            if (index < filepaths.Count - 1)
            {
                var temp = filepaths[index];
                filepaths.RemoveAt(index);
                filepaths.Insert(index + 1, temp);
            }
            PreviewTrigger();
        }
    }
}
