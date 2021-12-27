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

        public ReplaceCharacterUC(ReplaceCharacterOperation operation)
        {
            InitializeComponent();
            _operation = operation;
            ArgList.ItemsSource = (_operation.Args as ReplaceCharacterArgs)?.From;
        }


        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            ((ReplaceCharacterArgs)_operation.Args)
                .To = Param.Text;

            _operation.Args = (_operation.Args as ReplaceCharacterArgs)
                ;
            _operation.DescriptionChangedNotify();
        }

        private void RemoveArg_Click(object sender, RoutedEventArgs e)
        {
            int index = ArgList.SelectedIndex;
            (_operation.Args as ReplaceCharacterArgs)?.From.RemoveAt(index);
        }

        private void AddArgBtn_OnClickBtn_Click(object sender, RoutedEventArgs e)
        {
            var insertCharacter = (ArgsAdd.Text ?? "");
            if (insertCharacter == "") return;
            bool flag = true;
            var bindingList = (_operation.Args as ReplaceCharacterArgs)?.From;
            if (bindingList != null)
            {
                foreach (var character in bindingList)
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
                    (_operation.Args as ReplaceCharacterArgs)?.From.Add(insertCharacter);
                    ArgsAdd.Text = "";
                }
            }
        }
    }
}
