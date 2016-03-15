using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public enum Operator
    {
        Multiply = 0,
        Divide,
        Add,
        Subtract,
    }

    public enum Modifier
    {
        Equal,
        Period,
        OpenParen,
        ClosedParen,
        Invert
    }

    public enum Mode
    {
        Replace = 0,
        Append,
    }

    public interface ICalculatorView
    {
        event EventHandler<NumberPressedEventArgs> NumberPressed;
        event EventHandler<OperatorPressedEventArgs> OperatorPressed;
        event EventHandler<ModifierPressedEventArgs> ModifierPressed;
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

    public class ModifierPressedEventArgs : EventArgs //EventArgs contains data for the event
    {
        public Modifier Modifier { get; set; }
    }

    public class CalculatorController
    {
        ICalculatorView view;

        Mode mode = Mode.Replace;
        Operation current = 0; //initialize current operation
        Operation storedOperation = 0; //initialize storedOperation
        

        public CalculatorController(ICalculatorView view)
        {
            this.view = view;
            this.view.NumberPressed += HandleNumberPressed;
            this.view.OperatorPressed += HandleOperatorPressed;
            this.view.ModifierPressed += HandleModifierPressed;
        }

        private void HandleOperatorPressed(object sender, OperatorPressedEventArgs e)
        {
            switch (e.Operator)
            {
                case Operator.Add:
                    AddOperation add = new AddOperation();
                    add.LeftOperand = storedOperation;
                    current = add;
                    mode = Mode.Replace;
                    break;

                case Operator.Subtract:
                    SubtractOperation subtract = new SubtractOperation();
                    subtract.LeftOperand = storedOperation;
                    current = subtract;
                    mode = Mode.Replace;
                    break;

                case Operator.Multiply:
                    MultiplyOperation multiply = new MultiplyOperation();
                    multiply.LeftOperand = storedOperation;
                    current = multiply;
                    mode = Mode.Replace;
                    break;

                case Operator.Divide:
                    DivideOperation divide = new DivideOperation();
                    divide.LeftOperand = storedOperation;
                    current = divide;
                    mode = Mode.Replace;
                    break;
            }
        }

        private void HandleModifierPressed(object sender, ModifierPressedEventArgs e)
        {
            switch (e.Modifier)
            {
                case Modifier.Equal:
                    current.RightOperand = storedOperation;
                    this.view.Display = current.Result.ToString("0.######");
                    mode = Mode.Replace;
                    break;

                case Modifier.Period:
                    storedOperation = current;
                    
                    break;

                case Modifier.Invert:
                    break;

                case Modifier.OpenParen:
                    break;

                case Modifier.ClosedParen:
                    break;

            }
        }

        private void HandleNumberPressed(object sender, NumberPressedEventArgs e)
        {
            if(mode == Mode.Replace)
            {
                storedOperation = e.Number;
                this.view.Display = storedOperation.Result.ToString("0.######");
                mode = Mode.Append;
            } else
            {
                storedOperation = storedOperation.Result * 10 + e.Number;
                this.view.Display = storedOperation.Result.ToString("0.######");
            }


        }

        
    }
}
