using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ReplaceCharacter
{
    /// <summary>
    /// Interaction logic for ReplaceCharacterUC.xaml
    /// </summary>
    public partial class ReplaceCharacterUC : UserControl
    {
        private readonly ReplaceCharacterOperation _operation;
        private BindingList<char> listCharBinding = new BindingList<char>();

        public ReplaceCharacterUC(ReplaceCharacterOperation operation)
        {
            InitializeComponent();
            _operation = operation;
            var args = _operation.Args as ReplaceCharacterArgs;
            Param.Text = (args.To).ToString() ?? "";
            foreach (var character in args.From)
            {
                listCharBinding.Add(character);
            }

            ArgList.ItemsSource = listCharBinding;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            var args = _operation.Args as ReplaceCharacterArgs;
            args.To = (Param.Text ?? "")[0];
            _operation.DescriptionChangedNotify();
        }

        private void RemoveArg_Click(object sender, RoutedEventArgs e)
        {
            int index = ArgList.SelectedIndex;
            listCharBinding.RemoveAt(index);
        }

        private void AddArgBtn_OnClickBtn_Click(object sender, RoutedEventArgs e)
        {
            var insertCharacter = (ArgsAdd.Text ?? "")[0];
            bool flag = true;
            foreach (var character in listCharBinding)
            {
                if (character == insertCharacter)
                {
                    ArgsAdd.Text = "";
                    MessageBox.Show("This character has been existed");
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                listCharBinding.Add(insertCharacter);
                ArgsAdd.Text = "";
            }
        }
    }
}
