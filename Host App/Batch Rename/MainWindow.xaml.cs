using Contract;

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

        Dictionary<string, IStringOperation> _prototypes = new Dictionary<string, IStringOperation>();
        BindingList<IStringOperation> _actions = new BindingList<IStringOperation>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FilesListView.ItemsSource = filepaths;

            /*string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = Path.GetDirectoryName(exePath);
            var fileInfos = new DirectoryInfo(folder).GetFiles("*.dll");
            var plugins = new List<IStringOperation>();
            foreach(var fi in fileInfos)
            {
                Debug.WriteLine(fi.FullName);    
            }*/

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
            RulesListView.Items.Clear();
            RulesComboBox.ItemsSource = _prototypes;//bản mẫu cho người dùng xem, nếu người dùng Add thì clone ra

            RulesListView.ItemsSource = _actions;//là Binding list, thêm xóa sửa _action thì giao diện tự cập nhập


        }


        private void FilesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //Browse files
        private void WindowsExplorerButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            //filter browser
            openFileDialog.Filter = "ALL Files|*.*|TEXT FILE | *.txt|PDF FILE |*.pdf|Folders|\n";

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
                        if (filepaths[j].Root == openFileDialog.FileNames[i])
                        {
                            flag = false;
                            break;
                        }
                    }

                    //add file vao array
                    CFile temp=new CFile();
                    temp.Root = openFileDialog.FileNames[i];
                    if (flag) filepaths.Add(temp);
                    //if (flag) filepaths.Add(openFileDialog.FileNames[i]);
                    //MessageBox.Show(openFileDialog.FileNames[i])
                }
            }
        }

        
        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyRulesButton_Click(object sender, RoutedEventArgs e)
        {
            //Duyệt tất cả các file đã chọn ở listview
            //Cần 1 biến cho biết copy hay chỉ đổi tên nữa
            for (int i = 0; i < filepaths.Count(); i++)
            {
                if (filepaths[i].isChecked) //MessageBox.Show(filepaths[i].Root);
                {
                    //Đổi tên
                    //Nếu chỉ đổi chứ kp copy thì...
                    string newName = $"{filepaths[i].getDir()}{filepaths[i].PreviewName}";
                    System.IO.File.Move(filepaths[i].Root, newName);
                    //Cập nhật lại cái dường dẫn tuyệt đối
                    filepaths[i].Root = newName;
                    //Bỏ check đi, cái này tùy
                    filepaths[i].isChecked = false;


                    //Nếu copy thì...


                    //Cuối cùng set PreviewName về rỗng
                    filepaths[i].PreviewName = "";
                }
            }
            ////Lấy các file được chọn (cố gắng lấy index) bỏ vào list tạm
            //int selectedFilesCount = FilesListView.SelectedItems.Count;
            //List<CFile> selectedFiles = new List<CFile>();
            //for (int i = 0; i < selectedFilesCount; i++)
            //    selectedFiles.Add((CFile)(FilesListView.SelectedItems[i]));

            ////Apply rules vào 
            ////check copy hay rename để lấy root mới

            //for (int i = 0; i < selectedFilesCount; i++)
            //{
            //    string newName = @"D:\updatedddd.txt";
            //    //System.IO.File.Move(selectedFiles[i].Root, newName);

            //    filepaths[0].PreviewName = newName;
            //    MessageBox.Show(filepaths[i].Root);
            //}
        }

        private void PreviewChange_Click(object sender, RoutedEventArgs e)
        {
            //Duyệt tất cả các file đã chọn ở listview
            for (int i = 0; i < filepaths.Count(); i++)
            {
                if (filepaths[i].isChecked) 
                {
                    //Áp luật vào rồi gán vào ReviewName, lấy tên hiện tại bằng .getCurrentName()
                    //Chỗ này gán để test thôi
                    filepaths[i].PreviewName = $"{i}_{filepaths[i].getCurrentName()}";
                }
            }
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
            }
        }

        private void RemoveRule_Click(object sender, RoutedEventArgs e)
        {
            var index = RulesListView.SelectedIndex;
            _actions.RemoveAt(index);
        }
    }
}
