using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Calculator : ICalculator
    {
        Operation current = 0;
        public Operation Current => current;

        private void DoOperation(Operation op)
        {
            
        }
    }
}
