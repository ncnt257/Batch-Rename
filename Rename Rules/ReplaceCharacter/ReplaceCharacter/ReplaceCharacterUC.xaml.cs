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
        public ReplaceCharacterArgs args = new ReplaceCharacterArgs();
        public ReplaceCharacterUC(ReplaceCharacterOperation operation)
        {
            InitializeComponent();
            _operation = operation;
            args = _operation.Args as ReplaceCharacterArgs;
            ArgList.ItemsSource = args.From;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            args.To = Param.Text;
            _operation.Args = args;
            _operation.DescriptionChangedNotify();
        }

        private void RemoveArg_Click(object sender, RoutedEventArgs e)
        {
            int index = ArgList.SelectedIndex;
            args.From.RemoveAt(index);
        }

        private void AddArgBtn_OnClickBtn_Click(object sender, RoutedEventArgs e)
        {
            var insertCharacter = (ArgsAdd.Text ?? "");
            bool flag = true;
            foreach (var character in args.From)
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
                args.From.Add(insertCharacter);
                ArgsAdd.Text = "";
            }
        }
    }
}
