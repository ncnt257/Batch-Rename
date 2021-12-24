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

namespace AddCounterRule
{
    /// <summary>
    /// Interaction logic for AddCounterRuleUC.xaml
    /// </summary>
    public partial class AddCounterRuleUC : UserControl
    {
        private readonly AddCounterOperation _operation;
        public AddCounterRuleUC(AddCounterOperation operation)
        {
            InitializeComponent();
            _operation = operation;
            var args = _operation.Args as AddCounterArgs;
            Param1.Text = args.Start.ToString();
            Param2.Text = args.Step.ToString();
            Param3.Text = args.Digit.ToString();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            var args = _operation.Args as AddCounterArgs;
            args.Start = AddCounterOperation.ToNullableInt(Param1.Text);
            args.Counter = AddCounterOperation.ToNullableInt(Param1.Text);
            args.Step = AddCounterOperation.ToNullableInt(Param2.Text);
            args.Digit = AddCounterOperation.ToNullableInt(Param3.Text);
            _operation.DescriptionChangedNotify();//notify thuộc tính description đã thay đổi

        }
    }
}
