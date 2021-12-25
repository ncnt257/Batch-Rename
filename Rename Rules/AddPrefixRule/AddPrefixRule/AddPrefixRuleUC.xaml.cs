using System.Windows;
using System.Windows.Controls;

namespace AddPrefixRule
{
    /// <summary>
    /// Interaction logic for AddPrefixRuleUC.xaml
    /// </summary>
    public partial class AddPrefixRuleUC : UserControl
    {
        private readonly AddPrefixOperation _operation;

        public AddPrefixRuleUC(AddPrefixOperation operation)
        {
            InitializeComponent();
            _operation = operation;
            var args = _operation.Args as AddPrefixArgs;
            Param.Text = args.Prefix;
        }
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            var args = _operation.Args as AddPrefixArgs;
            args.Prefix = Param.Text ?? "";
            _operation.DescriptionChangedNotify();//notify thuộc tính description đã thay đổi
        }
    }
}
