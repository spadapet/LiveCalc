using CalcCore;
using System.Windows.Controls;

namespace CalcWpf
{
    public partial class CalculatorControl : UserControl
    {
        public CalculatorControl()
        {
            InitializeComponent();
            DataContext = new CalculatorViewModel();
        }
    }
}
