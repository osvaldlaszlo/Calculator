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

        public event EventHandler<NumberPressedEventArgs> NumberPressed; //magic used to create the "event object"
        public event EventHandler<OperatorPressedEventArgs> OperatorPressed;
        public event EventHandler<ModifierPressedEventArgs> ModifierPressed;

        public void SendNumberPressed(double number)
        {
            NumberPressed?.Invoke(this, new NumberPressedEventArgs { Number = number });
        }

        public void SendOperatorPressed(Operator op)
        {
            OperatorPressed?.Invoke(this, new OperatorPressedEventArgs { Operator = op });
        }
        
        public void SendModifierPressed(Modifier mod)
        {
            ModifierPressed?.Invoke(this, new ModifierPressedEventArgs { Modifier = mod });
        }
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestMultiply()
        {
            var mockView = new MockView();
            var controller = new CalculatorController(mockView);

            mockView.SendNumberPressed(4);
            mockView.SendOperatorPressed(Operator.Multiply);
            mockView.SendNumberPressed(5);
            mockView.SendModifierPressed(Modifier.Equal);

           Assert.AreEqual("20", mockView.Display);

        }

        [Test]
        public void TestDivide()
        {
            var mockView = new MockView();
            var controller = new CalculatorController(mockView);

            mockView.SendNumberPressed(5);
            mockView.SendOperatorPressed(Operator.Divide);
            mockView.SendNumberPressed(3);
            mockView.SendModifierPressed(Modifier.Equal);

            Assert.AreEqual("1.666667", mockView.Display);

        }

        [Test]
        public void TestAdd()
        {
            var mockView = new MockView();
            var controller = new CalculatorController(mockView);

            mockView.SendNumberPressed(0.33333333333);
            mockView.SendOperatorPressed(Operator.Add);
            mockView.SendNumberPressed(5);
            mockView.SendModifierPressed(Modifier.Equal);

            Assert.AreEqual("5.333333", mockView.Display);

        }

        [Test]
        public void TestSubtract()
        {
            var mockView = new MockView();
            var controller = new CalculatorController(mockView);

            mockView.SendNumberPressed(4);
            mockView.SendOperatorPressed(Operator.Subtract);
            mockView.SendNumberPressed(5);
            mockView.SendModifierPressed(Modifier.Equal);

            Assert.AreEqual("-1", mockView.Display);

        }

        [Test]
        public void TestMultipleDigitInput()
        {
            var mockView = new MockView();
            var controller = new CalculatorController(mockView);

            mockView.SendNumberPressed(4);
            mockView.SendNumberPressed(5);

            Assert.AreEqual("45", mockView.Display);
        }

        [Test] //note: need to test for user pressing equal prior to any other operations
        public void TestDecimalInput()
        {
            var mockView = new MockView();
            var controller = new CalculatorController(mockView);

            mockView.SendNumberPressed(6);
            mockView.SendModifierPressed(Modifier.Period);
            mockView.SendNumberPressed(7);
            mockView.SendNumberPressed(8);

            Assert.AreEqual("6.78", mockView.Display);
        }
    }
}
