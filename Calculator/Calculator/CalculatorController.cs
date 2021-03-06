﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Calculator
{
    public enum Operator
    {
        Multiply = 0,
        Divide,
        Add,
        Subtract,
        Constant
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

    public enum Entry
    {
        Integer = 0,
        Decimal,
    }

    public enum Paren
    {
        Unused = 0,
        Used,
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
        Entry entry = Entry.Integer;

        bool parenClosed = false;
        int decimalCount = 1;
        int parenStackCount = 0;

        Operation current = 0; //initialize current operation
        Operation storedOperation = 0; //initialize storedOperation

        List<Operation> parenStack = new List<Operation>();

        public CalculatorController(ICalculatorView view)
        {
            this.view = view;
            this.view.NumberPressed += HandleNumberPressed;
            this.view.OperatorPressed += HandleOperatorPressed;
            this.view.ModifierPressed += HandleModifierPressed;
        }

        private void HandleOperatorPressed(object sender, OperatorPressedEventArgs e)
        {
            entry = Entry.Integer;
            decimalCount = 1;

            switch (e.Operator)
            {
                case Operator.Add:
                    if(parenClosed)
                    {
                        this.view.Display = parenStack[0].Result.ToString("0.####");
                        AddOperation parenAdd = new AddOperation();
                        parenAdd.LeftOperand = parenStack[0];
                        current = parenAdd;
                        mode = Mode.Replace;
                        parenClosed = false;
                        break;
                    }

                    AddOperation add = new AddOperation();            
                    add.LeftOperand = storedOperation;
                    current = add;
                    mode = Mode.Replace;
                    break;

                case Operator.Subtract:
                    if (parenClosed)
                    {
                        this.view.Display = parenStack[0].Result.ToString("0.####");
                        SubtractOperation parenSubtract = new SubtractOperation();
                        parenSubtract.LeftOperand = parenStack[0];
                        current = parenSubtract;
                        mode = Mode.Replace;
                        parenClosed = false;
                        break;
                    }

                    SubtractOperation subtract = new SubtractOperation();
                    subtract.LeftOperand = storedOperation;
                    current = subtract;
                    mode = Mode.Replace;
                    break;

                case Operator.Multiply:
                    if (parenClosed)
                    {
                        MultiplyOperation parenMultiply = new MultiplyOperation();
                        parenMultiply.LeftOperand = parenStack[0];
                        current = parenMultiply;
                        mode = Mode.Replace;
                        parenClosed = false;
                        break;
                    }
                    MultiplyOperation multiply = new MultiplyOperation();
                    multiply.LeftOperand = storedOperation;
                    current = multiply;
                    mode = Mode.Replace;
                    break;

                case Operator.Divide:
                    if (parenClosed)
                    {
                        DivideOperation parenDivide = new DivideOperation();
                        parenDivide.LeftOperand = parenStack[0];
                        current = parenDivide;
                        mode = Mode.Replace;
                        parenClosed = false;
                        break;
                    }
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
                    if (parenClosed)
                    {
                        current = parenStack[0];
                        this.view.Display = current.Result.ToString("0.####");
                        mode = Mode.Replace;
                        parenClosed = false;
                        break;
                    }

                    current.RightOperand = storedOperation;
                    this.view.Display = current.Result.ToString("0.####");
                    mode = Mode.Replace;
                    break;

                case Modifier.Period:
                    current = storedOperation;
                    this.view.Display = current.Result.ToString("0.####");
                    mode = Mode.Replace;
                    entry = Entry.Decimal;
                    break;

                case Modifier.Invert:
                    storedOperation = storedOperation.Result * -1;
                    current = storedOperation;
                    this.view.Display = current.Result.ToString("0.####");
                    break;

                case Modifier.OpenParen:
                    parenStack.Add(current.Clone());
                    parenStackCount++;
                    mode = Mode.Replace;
                    this.view.Display = "0";
                    break;

                case Modifier.ClosedParen:
                    parenStackCount--;
                    current.RightOperand = storedOperation;
                    parenStack[parenStackCount].RightOperand = current;
                    parenClosed = true;
                    mode = Mode.Replace;
                    this.view.Display = current.Result.ToString("0.####");
                    break;

            }
        }

        private void HandleNumberPressed(object sender, NumberPressedEventArgs e)
        {
            if(entry == Entry.Decimal) 
            {

                storedOperation = storedOperation.Result + e.Number / Math.Pow(10, decimalCount);
                decimalCount++;
                this.view.Display = storedOperation.Result.ToString("0.####");
                return;
            }

            if(mode == Mode.Replace)
            {
                storedOperation = e.Number;
                mode = Mode.Append;
                this.view.Display = storedOperation.Result.ToString("0.####");
            } else
            {
                storedOperation = storedOperation.Result * 10 + e.Number;
                this.view.Display = storedOperation.Result.ToString("0.####");
            }
            
        }

        private bool AreSame(Type a, Type b)
        {
            if (a == b) return true;
            return false;

        }
        
    }
}
