using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using statistics;

namespace statistics
{
    static class Wound
    {
        static private int WoundTable(int slScore, int rollScore)
        {
            int[,] wound_table = new int[13, 11]{
                {0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0},
                {0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0},
			    {0, 0, 0, 0, 1, 2, 3, 4,  5,  6,  7},		
			    {0, 0, 0, 1, 2, 3, 4, 5,  6,  7,  8},
                {0, 0, 1, 2, 3, 4, 5, 6,  7,  8,  9},
                {0, 0, 1, 2, 3, 4, 5, 6,  7,  8,  9},
                {0, 1, 2, 3, 4, 5, 6, 7,  8,  9,  10},
                {0, 1, 2, 3, 4, 5, 6, 7,  8,  9,  10},
                {0, 1, 2, 3, 4, 5, 6, 7,  8,  9,  10},
                {1, 2, 3, 4, 5, 6, 7, 8,  9,  10, 11},
                {1, 2, 3, 4, 5, 6, 7, 8,  9,  10, 11},
                {2, 3, 4, 5, 6, 7, 8, 9,  10, 11, 12},
                {3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13}};
            try
            {
                return wound_table[rollScore, slScore];
            }
            catch (Exception e)
            {
                Output.PrintLine(rollScore.ToString() + " " + slScore.ToString());
                throw e;
            }
        }

        static private int maxOf3(int a, int b, int c)
        {
            return Math.Max(a, Math.Max(b, c));
        }

        static private int secondOf3(int a, int b, int c)
        {
            if (a == Math.Max(a, b)) {
                return Math.Max(b, c);
            } else {
                return Math.Max(a, c);
            }
        }

        static private int minOf3(int a, int b, int c)
        {
            return Math.Min(a, Math.Min(b, c));
        }

        static public int DamageRoll(Profile Attacker, Profile Defender)
        {
            int roll;

            // Get the roll
            if (Attacker.Treats.Strong.Has || Attacker.Treats.Assassin.Has)
            {
                int a = Dices.D6(), b = Dices.D6(), c = Dices.D6();
                roll = a + b + c - minOf3(a, b, c);
            } else if (Attacker.Treats.Weak.Has)
            {
                int a = Dices.D6(), b = Dices.D6(), c = Dices.D6();
                roll = a + b + c - maxOf3(Dices.D6(), Dices.D6(), Dices.D6());
            } else
            {
                roll = Dices.D6() + Dices.D6();
            }

            // Apply strengh
            roll += Attacker.Treats.Strengh.Value;

            // Apply armour & armour piercing
            bool sharpOrPiercing = Attacker.Treats.ArmourPiercing.Has;
            sharpOrPiercing |= Attacker.Treats.Sharp.Has;
            if (Defender.Treats.Armour.Has && !sharpOrPiercing)
            {
                roll -= Defender.Treats.Armour.Value;
            }

            // stay between 0 and 12
            roll = Math.Min(12, roll);
            roll = Math.Max(0, roll);

            return roll;
        }

        static public int GetWound(int SuccessLevel, Profile Attacker, Profile Defender)
        {
            int damageRoll, result;
            
            // Get the damage roll
            damageRoll = DamageRoll(Attacker, Defender);
            // Get the damage
            result = WoundTable(SuccessLevel, damageRoll);
            // Apply toughness / sharp
            if (Defender.Treats.Toughness.Has && 
                ! Attacker.Treats.Sharp.Has)
            {
                result -= Defender.Treats.Toughness.Value;
            }

            if (Defender.Treats.Durable.Has) {
                result = Math.Min(1, result);
            }
            return result;
        }

        static public bool LocalTest()
        {
            bool res = true;
            res &= TestWound();
            res &= TestArmor();

            return res;
        }

        static public bool TestWound()
        {
            bool functionnal = true;
            if (Test.verbose) Output.Print("Test Wound (0, 0):   ");
            functionnal &= Test.TestValue(0, WoundTable(0, 0));
            if (Test.verbose) Output.Print("Test Wound (0, 12):   ");
            functionnal &= Test.TestValue(3, WoundTable(0, 12));
            if (Test.verbose) Output.Print("Test Wound (10, 0):   ");
            functionnal &= Test.TestValue(0, WoundTable(10, 0));
            if (Test.verbose) Output.Print("Test Wound (10, 2):   ");
            functionnal &= Test.TestValue(7, WoundTable(10, 2));
            if (Test.verbose) Output.Print("Test Wound (10, 12):   ");
            functionnal &= Test.TestValue(13, WoundTable(10, 12));

            return functionnal;
        }

        static public bool TestArmor()
        {
            bool res = true, tmp;
            double withPiercing, withoutArmour, withSharp, withSharpPiercing;
            double withArmour;

            Profile A = new Profile(4, 0), D = new Profile(0, 1);
            
            withoutArmour = ResultPresentation.AverageDamage(A.Attack(D));
            D.Treats.Armour.Value = 100;
            withArmour = ResultPresentation.AverageDamage(A.Attack(D));
            A.Treats.ArmourPiercing.Value = 0;
            withPiercing = ResultPresentation.AverageDamage(A.Attack(D));
            A.Treats.Sharp.Value = 0;
            withSharpPiercing = ResultPresentation.AverageDamage(A.Attack(D));
            A.Treats.ArmourPiercing.Value = -1;
            withSharp = ResultPresentation.AverageDamage(A.Attack(D));
            
            tmp = (Test.TestDouble(withPiercing, withoutArmour, 1));
            res &= tmp;
            if (Test.verbose)
            {
                Output.Print("Test armour / armourpiercing: ");
                Output.Print(withPiercing.ToString() + " " + withoutArmour.ToString());
                if (tmp)
                {
                    Output.PrintLine(": ok.");
                } else
                {
                    Output.PrintLine(": not ok.");
                }
            }
            tmp = (Test.TestDouble(withPiercing, withSharp, 1));
            res &= tmp;
            if (Test.verbose)
            {
                Output.Print("Test armour / sharp: ");
                Output.Print(withPiercing.ToString() + " " + withSharp.ToString());
                if (tmp)
                {
                    Output.PrintLine(": ok.");
                } else
                {
                    Output.PrintLine(": not ok.");
                }
            }
            tmp = (Test.TestDouble(withPiercing, withSharpPiercing, 1));
            res &= tmp;
            if (Test.verbose)
            {
                Output.Print("Test armour / sharp-piercing: ");
                Output.Print(withPiercing.ToString() + " " + withSharpPiercing.ToString());
                if (tmp)
                {
                    Output.PrintLine(": ok.");
                } else
                {
                    Output.PrintLine(": not ok.");
                }
            }

            tmp = ! Test.TestDouble(withArmour, withoutArmour, 1);
            res &= tmp;
            if (Test.verbose)
            {
                Output.Print("Test armour / without: ");
                Output.Print(withPiercing.ToString() + " " + withArmour.ToString());
                if (tmp)
                {
                    Output.PrintLine(": ok.");
                }
                else
                {
                    Output.PrintLine(": not ok.");
                }
            }

            return res;
        }
    }
}
