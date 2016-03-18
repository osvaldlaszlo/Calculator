﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public abstract class Operation //make an implicit operator (google implicit operator override) should take a double and return an operation (a constant)
    {
        Operation leftOperand;
        public Operation LeftOperand
        {
            get{ return leftOperand; }
            set { leftOperand = value; }
        }

        Operation rightOperand;
        public Operation RightOperand
        {
            get{ return rightOperand; }
            set{ rightOperand = value; }
        }
        public abstract double Result { get; }

        public static implicit operator Operation(double number)
        {
            Constant constant = new Constant(number);
            return constant;
        }
    }

    public class AddOperation : Operation
    {
        public Operator operatorType;
        public Operator OperatorType
        {
            get
            {
                return op = Operator.Multiply;
            }
        }
        public override string ToString()
        {
            return $"({LeftOperand} + {RightOperand})";
        }
        public override double Result
        {
            get
            {
                if (LeftOperand == null || RightOperand == null)
                    return double.NaN;
                return LeftOperand.Result + RightOperand.Result;
            }
        }
    }

    public class SubtractOperation : Operation
    {
        public Operator operatorType;
        public Operator OperatorType
        {
            get
            {
                return operatorType = Operator.Multiply;
            }
        }
        public override string ToString()
        {
            return $"({LeftOperand} - {RightOperand})";
        }

        public override double Result
        {
            get
            {
                if (LeftOperand == null || RightOperand == null)
                    return double.NaN;
                return LeftOperand.Result - RightOperand.Result;
            }
        }
    }

    public class MultiplyOperation : Operation
    {
        public Operator operatorType;
        public Operator OperatorType
        {
            get
            {
                return operatorType = Operator.Multiply;
            }
        }

        public override string ToString()
        {
            return $"({LeftOperand} * {RightOperand})";
        }
        public override double Result
        {
            get
            {
                if (LeftOperand == null || RightOperand == null)
                    return double.NaN;
                return LeftOperand.Result * RightOperand.Result;
            }
        }
    }

    public class DivideOperation : Operation
    {
        public Operator operatorType;
        public Operator OperatorType
        {
            get
            {
                return operatorType = Operator.Divide;
            }
        }
        public override string ToString()
        {
            return $"({LeftOperand} / {RightOperand})";
        }
        public override double Result
        {
            get
            {
                if (LeftOperand == null || RightOperand == null)
                    return double.NaN;
                return LeftOperand.Result / RightOperand.Result;
            }
        }
    }

    public class ParenOperation : Operation
    {
        public Operator currentOp;
        public override double Result
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }

    public class Constant : Operation
    {
        double number;
        public Constant(double number)
        {
            this.number = number;    
        }
        public override double Result
        {
            get
            {
                return number;
            }
        }
    }
}
