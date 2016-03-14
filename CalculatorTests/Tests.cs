using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Calculator;

namespace CalculatorTests
{
    public class MockView : ICalculatorView
    {
        public string Display { get; set; }

        public event EventHandler<NumberPressedEventArgs> NumberPressed;
        public event EventHandler<OperatorPressedEventArgs> OperatorPressed;

        public void SendNumberPressed(double number)
        {
            NumberPressed?.Invoke(this, new NumberPressedEventArgs { Number = number });
        }

        public void SendOperatorPressed(Operator op)
        {
            OperatorPressed?.Invoke(this, new OperatorPressedEventArgs { Operator = op });
        }
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test()
        {
            var leftOperand = 2;
            var rightOperand = 1;
            var op = Operator.Multiply;

            var result = MathUtils.Compute(leftOperand, rightOperand, op);

            Assert.AreEqual(322, result);
        }

        [Test]
        public void TestMultiply()
        {
            var mockView = new MockView();
            var controller = new CalculatorController(mockView);

            mockView.SendNumberPressed(4);
            Assert.AreEqual("4", mockView.Display);

            mockView.SendOperatorPressed(Operator.Multiply);
            mockView.SendNumberPressed(5);
            mockView.SendOperatorPressed(Operator.Equal);

            Assert.AreEqual("20", mockView.Display);

        }
    }
}
