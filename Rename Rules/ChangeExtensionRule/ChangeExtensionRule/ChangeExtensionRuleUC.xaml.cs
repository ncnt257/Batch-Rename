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

namespace ChangeExtensionRule
{
    /// <summary>
    /// Interaction logic for ChangeExtensionRuleUC.xaml
    /// </summary>
    public partial class ChangeExtensionRuleUC : UserControl
    {
        private readonly ChangeExtensionOperation _operation;
        public ChangeExtensionRuleUC(ChangeExtensionOperation operation)
        {
            InitializeComponent();
            _operation = operation;
            var args = _operation.Args as ChangeExtensionArgs;
            Param.Text = args.NewExtension;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            var args = new ChangeExtensionArgs()
            {
                NewExtension = Param.Text ?? "",
            };
            _operation.Args = args;
            _operation.DescriptionChangedNotify();//notify thuộc tính description đã thay đổi
        }
    }
}
