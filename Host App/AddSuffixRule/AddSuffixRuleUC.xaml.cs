using Contract;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AddSuffixRule
{
    /// <summary>
    /// Interaction logic for AddSuffixRuleUC.xaml
    /// </summary>
    public partial class AddSuffixRuleUC : UserControl
    {
        private readonly AddSuffixOperation _operation;
        public AddSuffixRuleUC(AddSuffixOperation operation)
        {
            InitializeComponent();
            _operation = operation;
            var args = _operation.Args as AddSuffixArgs;
            Param.Text = args.Suffix;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            var args = _operation.Args as AddSuffixArgs;
            args.Suffix = Param.Text ?? "";
            _operation.DescriptionChangedNotify();//notify thuộc tính description đã thay đổi
        }
    }
}
