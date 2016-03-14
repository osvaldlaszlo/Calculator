using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public interface ICalculatorView
    {
        event EventHandler<NumberPressedEventArgs> NumberPressed;
        event EventHandler<OperatorPressedEventArgs> OperatorPressed;
        string Display { get; set; }
    }

    public class NumberPressedEventArgs : EventArgs
    {
        public double Number { get; set; }
    }

    public class OperatorPressedEventArgs : EventArgs
    {
        public Operator Operator { get; set; }
    }

    public class CalculatorController
    {
        ICalculatorView view;

        public CalculatorController(ICalculatorView view)
        {
            this.view = view;
            this.view.NumberPressed += HandleNumberPressed;
            this.view.OperatorPressed += HandleOperatorPressed;
        }

        private void HandleOperatorPressed(object sender, OperatorPressedEventArgs e)
        {
        }

        private void HandleNumberPressed(object sender, NumberPressedEventArgs e)
        {
                        
        }

        
    }
}
