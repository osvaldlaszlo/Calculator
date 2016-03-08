using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Calculator
{
    public enum Operator
    {
        Multiply = 0,
        Divide = 1,
        Add = 2,
        Subtract = 3,
    }

    public enum Mode
    {
        Replace = 0,
        Append = 1,
    }

    public class CalculatorPage : ContentPage
    {

        double leftOperand;
        Operator currentOperator;
        Mode currentMode;

        double display = 0;
        double Display
        {
            get
            {
                return display;
            }
            set
            {
                display = value;
                label.Text = string.Format("{0:0.###}",value);
            }
        }

        Button buttonClear = new Button();
        Button buttonZero = new Button();
        Button buttonEquals = new Button();

        Button buttonMultiply = new Button();
        Button buttonDivide = new Button();
        Button buttonAdd = new Button();
        Button buttonSubtract = new Button();

        Grid grid = new Grid();
        Label label = new Label();
        StackLayout gridWrapper = new StackLayout();

        public CalculatorPage()
        {

            createButtonText(); 

            buttonMultiply.Clicked += (s, e) => HandleOperatorButtonClicked(buttonMultiply, Operator.Multiply);
            buttonDivide.Clicked += (s, e) => HandleOperatorButtonClicked(buttonMultiply, Operator.Divide);
            buttonAdd.Clicked += (s, e) => HandleOperatorButtonClicked(buttonMultiply, Operator.Add);
            buttonSubtract.Clicked += (s, e) => HandleOperatorButtonClicked(buttonMultiply, Operator.Subtract);

            buttonClear.Clicked += (s, e) => Display = 0;
            buttonZero.Clicked += (s, e) => HandleNumberButtonClicked(buttonZero, 0);

            gridWrapper.Children.Add(label);
            gridWrapper.Children.Add(grid);

            Display = 0; //initialize display

            buttonEquals.Clicked += (s, e) =>
            {
                Display = Compute(leftOperand, Display, currentOperator);
            };

            label.LineBreakMode = LineBreakMode.NoWrap;
            label.FontSize = 50;
            label.HorizontalTextAlignment = TextAlignment.End;

            grid.Padding= new Thickness(5,5,5,5);
            gridWrapper.Padding = new Thickness(30,30,30,30);
            Content = gridWrapper;
        }

        private void HandleNumberButtonClicked(Button button, int number)
        {
            if(currentMode == Mode.Replace)
            {
                Display = number;
                currentMode = Mode.Append;
                return;
            }

            if(label.Text.Length < 9)
                Display = Display*10 + number;
        }

        private void HandleOperatorButtonClicked(Button button, Operator op)
        {
            currentMode = Mode.Replace;
            leftOperand = Display;
            currentOperator = op;
        }

        private void createButtonText()
        {
            int x = 9;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 4; j > 0; j--)
                {
                    if (j == 4)
                    {
                        continue;
                    }
                    var tempButton = new Button();
                    tempButton.Text = x.ToString();
                    int val = x;
                    tempButton.Clicked += (s, e) => HandleNumberButtonClicked(tempButton, val);
                    x--;
                    grid.Children.Add(tempButton, j - 1, j, i, i + 1);
                }
            }

            grid.Children.Add(buttonMultiply, 3, 4, 0, 1);
            grid.Children.Add(buttonDivide, 3, 4, 1, 2);
            grid.Children.Add(buttonAdd, 3, 4, 2, 3);
            grid.Children.Add(buttonSubtract, 3, 4, 3, 4);

            grid.Children.Add(buttonClear, 0, 1, 3, 4);
            grid.Children.Add(buttonZero, 1, 2, 3, 4);
            grid.Children.Add(buttonEquals, 2, 3, 3, 4);

            buttonMultiply.Text = "*";
            buttonDivide.Text = "/";
            buttonAdd.Text = "+";
            buttonSubtract.Text = "-";

            buttonClear.Text = "C";
            buttonZero.Text = "0";
            buttonEquals.Text = "=";

        }

        private double Compute(double left, double right, Operator op)
        {
            switch (op)
            {
                case Operator.Multiply:
                    return left * right;
                case Operator.Divide:
                    return left / right;
                case Operator.Add:
                    return left + right;
                case Operator.Subtract:
                    return left - right;
            }
            throw new InvalidOperationException();
        }
    }
}

