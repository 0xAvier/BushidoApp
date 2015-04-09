using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statistics
{
    // This class provides the test for non-release
    class Test
    {
        static public bool verbose = true;

        // Run every test
        static public bool RunTest()
        {
            bool res = true;
            res &= Dices.LocalTest();
            res &= DiceList.LocalTest();
            res &= Wound.LocalTest();

            if (res && verbose)
            {
                Output.PrintLine("Every test are correct.");
                //System.Console.Read();
            }
            if (! res)
            {
                Output.PrintLine("Recession found.");
                //System.Console.Read();
            }
            
            return res;
        }

        static public bool TestInterval(double min, double max, double value)
        {
            if (value >= min && value <= max)
            {
                if (verbose) Output.PrintLine("ok.");
                return true;
            }
            else
            {
                if (verbose)
                {
                    Output.PrintLine("not ok.");
                    Output.PrintLine("Expected " + min.ToString() + " < " + value.ToString() + " < " + max.ToString() + ".");
                }
                return false;
            }
        }

        static public bool TestValue(double expected, double value)
        {
            if (expected == value)
            {
                if (verbose) Output.PrintLine("ok.");
                return true;
            } else
            {
                if (verbose)
                {
                    Output.PrintLine("not ok.");
                    Output.PrintLine("Expected " + expected.ToString() +
                        ". Got " + value.ToString() + ".");
                }

                return false;
            }
        }

        static public bool TestDouble(double expected, double value, int precision)
        {
            double prec = Math.Pow(10, -1*precision);
            if (prec == 0)
            {
                throw new Exception();
            }

            return Math.Abs(expected - value) < prec;
        }
    }
}
