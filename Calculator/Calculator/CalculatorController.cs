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
        Open = 0,
        Closed = 1,
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
        Paren paren = Paren.Closed;

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
                case Modifier.Equal: //need to modify functionality to back-out parenStack
                    current = parenStack[0];
                    current.RightOperand = storedOperation;
                    this.view.Display = current.Result.ToString("0.######");
                    mode = Mode.Replace;
                    break;

                case Modifier.Period:
                    current = storedOperation;
                    this.view.Display = current.Result.ToString("0.######");
                    mode = Mode.Replace;
                    entry = Entry.Decimal;
                    break;

                case Modifier.Invert:
                    storedOperation = storedOperation.Result * -1;
                    current = storedOperation;
                    this.view.Display = current.Result.ToString("0.######");
                    break;

                case Modifier.OpenParen: //add to ParenOperation by switching over storedOperation.OperatorType and adding the correct kind of operation
                    paren = Paren.Open;
                    
                    switch(storedOperation.OperatorType)
                    {
                        case Operator.Add:
                            parenStack.Add(new AddOperation());
                            parenStack[parenStackCount] = current;
                            break;

                        case Operator.Subtract:
                            parenStack.Add(new SubtractOperation());
                            parenStack[parenStackCount] = current;
                            break;

                        case Operator.Multiply:
                            parenStack.Add(new MultiplyOperation());
                            parenStack[parenStackCount] = current;
                            break;

                        case Operator.Divide:
                            parenStack.Add(new DivideOperation());
                            parenStack[parenStackCount] = current;
                            break;
                    }

                    parenStackCount++;
                    mode = Mode.Replace;
                    this.view.Display = "(";
                    break;

                case Modifier.ClosedParen:
                    paren = Paren.Closed;

                    current = storedOperation;
                    parenStack[parenStackCount].RightOperand = current;
                    parenStackCount--;
                    
                    mode = Mode.Replace;
                    this.view.Display = "(" + current.Result.ToString("0.######") + ")";
                    break;

            }
        }

        private void HandleNumberPressed(object sender, NumberPressedEventArgs e)
        {
            if(entry == Entry.Decimal) 
            {

                storedOperation = storedOperation.Result + e.Number / Math.Pow(10, decimalCount);
                decimalCount++;

                if (paren == Paren.Open)
                {
                    this.view.Display = "(" + storedOperation.Result.ToString("0.######");
                    return;
                }

                this.view.Display = storedOperation.Result.ToString("0.######");
                return;
            }

            if(mode == Mode.Replace)
            {
                storedOperation = e.Number;
                mode = Mode.Append;

                if (paren == Paren.Open)
                    this.view.Display = "(" + storedOperation.Result.ToString("0.######");

                this.view.Display = storedOperation.Result.ToString("0.######");
            } else
            {
                storedOperation = storedOperation.Result * 10 + e.Number;

                if(paren == Paren.Open)
                    this.view.Display = "(" + storedOperation.Result.ToString("0.######");

                this.view.Display = storedOperation.Result.ToString("0.######");
            }


        }

        
    }
}
