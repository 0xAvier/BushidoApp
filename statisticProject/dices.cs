using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statistics
{
    // This class provide every dices statically
    static class Dices
    {
        // Generator used for every roll
        private static Random Generator = new Random();

        // Return the roll of a generic dice of "faces" faces
        private static int DX(int faces)
        {
            // Get a new random value
            double rand = Generator.NextDouble();
            // Treat every faces
            for (int i = 0; i < faces; i ++)
            {
                // If the random value is in the interval
                if (rand >= (double)i / faces && rand < (double) (i+1) / faces)
                {
                    // return the current face
                    return i + 1;
                }
            }
            if (rand == 1.0)
            {
                // return the last face 
                return faces;
            }

            // Shouldn't be reach
            // Every value between 0 and 1 must be covered
            throw new Exception();
        }

        // Return the result of a D6-roll
        public static int D6()
        {
            // Return the result of a generic dice set to 6 faces
            return DX(6);
        }

        // Return the result of a D2-roll
        public static int D2()
        {
            // Return the result of a generic dice set to 2 faces
            return DX(2);
        }

        // Return the result of the roll of a bushido dice
        public static int BushidoDice()
        {
            // Get the value of a roll
            int res = Dices.D6();
            // Return 0 if the D6 is a 1
            return (res != 1) ? res : 0;
        }

        static public bool LocalTest()
        {
            bool res = true;
            res &= TestDices();

            return res;
        }

        static public bool TestDices()
        {
            bool functionnal = true;

            int d2 = 0, d6 = 0, bushidoDice = 0;
            long max = 1000000;
            for (int i = 0; i < max; i++)
            {
                d2 += Dices.D2();
                d6 += Dices.D6();
                bushidoDice += Dices.BushidoDice();
            }

            double average;
            if (Test.verbose) Output.Print("Test Dices.D2:   ");
            average = (double)d2 / max;
            functionnal &= Test.TestInterval(1.49, 1.51, average);

            if (Test.verbose) Output.Print("Test Dices.D6:   ");
            average = (double)d6 / max;
            functionnal &= Test.TestInterval(3.49, 3.51, average);

            if (Test.verbose) Output.Print("Test Dices.bushidoDice:   ");
            average = (double)bushidoDice / max;
            functionnal &= Test.TestInterval(3.33, 3.34, average);

            if (Test.verbose && functionnal) Output.PrintLine("Test Dices:  ok.");
            else if (Test.verbose) Output.PrintLine("Test Dices:  not ok.");

            return functionnal;
        }
    }
}
