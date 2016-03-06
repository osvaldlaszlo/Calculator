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

    public class CalculatorPage : ContentPage
    {

        double leftOperand;
        Operator currentOperator; //create entry states "append" vs. "replace"

        public CalculatorPage() //clean up with break-out methods for later
        {
            Grid grid = new Grid();
            Label label = new Label();
            StackLayout gridWrapper = new StackLayout();

            var buttonClear = new Button();
            var buttonZero = new Button();
            var buttonEquals = new Button();

            var buttonMultiply = new Button();
            var buttonDivide = new Button();
            var buttonAdd = new Button();
            var buttonSubtract = new Button();

            int x = 9;

            for (int i = 0; i < 3; i++)
            {
                for(int j = 4; j > 0; j--)
                {
                    if (j == 4)
                    {
                        continue;
                    }
                    var tempButton = new Button();
                    tempButton.Text = x.ToString();
                    int val = x;
                    tempButton.Clicked += (s, e) =>
                    {
                        label.Text = (label.Text + val.ToString()).TrimStart('0');
                    };
                    x--;
                    grid.Children.Add(tempButton, j - 1, j, i, i + 1);
                }
            }

            grid.Children.Add(buttonMultiply, 3, 4, 0, 1);
            grid.Children.Add(buttonDivide, 3, 4, 1, 2);
            grid.Children.Add(buttonAdd, 3, 4, 2, 3);
            grid.Children.Add(buttonSubtract, 3, 4, 3, 4);

            buttonMultiply.Clicked += (s, e) =>
            {
                leftOperand = double.Parse(label.Text);
                currentOperator = Operator.Multiply;
                label.Text = "0";
            };

            buttonDivide.Clicked += (s, e) =>
            {
                leftOperand = double.Parse(label.Text);
                currentOperator = Operator.Divide;
                label.Text = "0";
            };

            buttonAdd.Clicked += (s, e) =>
            {
                leftOperand = double.Parse(label.Text);
                currentOperator = Operator.Add;
                label.Text = "0";
            };

            buttonSubtract.Clicked += (s, e) =>
            {
                leftOperand = double.Parse(label.Text);
                currentOperator = Operator.Subtract;
                label.Text = "0";
            };

            buttonMultiply.Text = "*";
            buttonDivide.Text = "/";
            buttonAdd.Text = "+";
            buttonSubtract.Text = "-";

            grid.Children.Add(buttonClear, 0, 1, 3, 4);
            grid.Children.Add(buttonZero, 1, 2, 3, 4);
            grid.Children.Add(buttonEquals, 2, 3, 3, 4);

            buttonClear.Text = "C";
            buttonClear.Clicked += (s, e) =>
            {
                label.Text = "0";
            };

            buttonZero.Text = "0";
            buttonZero.Clicked += (s, e) =>
            {
                if(label.Text == "0")
                {
                    return;
                }
                label.Text = label.Text + "0";
            };

            gridWrapper.Children.Add(label);
            gridWrapper.Children.Add(grid);

            label.Text = "0";

            buttonEquals.Text = "=";
            buttonEquals.Clicked += (s, e) =>
            {
                label.Text = Compute(leftOperand, double.Parse(label.Text), currentOperator).ToString();
            };

            label.LineBreakMode = LineBreakMode.NoWrap;
            label.FontSize = 50;
            label.HorizontalTextAlignment = TextAlignment.End;

            grid.Padding= new Thickness(5,5,5,5);
            gridWrapper.Padding = new Thickness(30,30,30,30);
            Content = gridWrapper;
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
