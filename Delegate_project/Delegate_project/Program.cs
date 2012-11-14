using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Number_arranging_and_stuff
{
    class Program
    {
        private delegate double Operation(double op1, double op2);

        static void Main(string[] Args)
        {
            List<Operation> opList = new List<Operation>();
            Operations operations = new Operations();
            opList.Add(operations.Add);
            opList.Add(operations.Subtract);
            opList.Add(operations.Multiply);
            opList.Add(operations.Division);
            opList.Add(operations.Square_sum);
            foreach (Operation x in opList)
            {
                Console.WriteLine(x(69, 96));
            }
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }

    class Operations
    {
        public double Add(double a, double b)
        {
            Console.Write("{0} + {1} = ", a, b);
            return a + b;
        }

        public double Subtract(double a, double b)
        {
            Console.Write("{0} - {1} = ", a, b);
            return a - b;
        }

        public double Multiply(double a, double b)
        {
            Console.Write("{0} x {1} = ", a, b);
            return a * b;
        }

        public double Division(double a, double b)
        {
            Console.Write("{0} / {1} = ", a, b);
            return a / b;
        }

        public double Square_sum(double a, double b)
        {
            Console.Write("{0}^2 + {1}^2 = ", a, b);
            return (a * a) + (b * b);
        }
    }
}
