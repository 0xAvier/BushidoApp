using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statistics
{
    static class ResultPresentation
    {
        static public double[] Integral(double[] wound)
        {
            for (int i = wound.Length - 1; i > 1; i--)
            {
                wound[i - 1] += wound[i];
            }
            for (int i = 0; i < wound.Length; i++) {
                wound[i] = Math.Round(wound[i], 2);
                //Console.Write(wound[i]);
                //Console.Write(" ");
            }

                return wound;
        }

        static public double AverageDamage(double[] wound)
        {
            double avg = 0;

            for (int i = wound.Length - 1; i > 0; i--)
            {
                avg += wound[i] * (i - 1);
            }

            return avg;
        }
    }
}
