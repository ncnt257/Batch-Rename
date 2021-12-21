using System;
using System.Collections.Generic;
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
            var args = new AddSuffixArgs()
            {
                Suffix = Param.Text ?? "",
            };
            _operation.Args = args;
            _operation.DescriptionChangedNotify();//notify thuộc tính description đã thay đổi
        }
    }
}
