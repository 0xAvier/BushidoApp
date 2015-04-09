using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace statistics
{
    class Profile
    {
        // default number of simulation rolled
        int nbSimu = 1000000;

        // maximum dice allocable
        public int MaxDice { get; set; } 
        // defence dice allocate
        public int DefenceDice { get; set; }
        public int DefenceBonus{ get; set; }
        // attack dice allocate
        public int AttackDice { get; set; }
        public int AttackBonus { get; set; }
        // 
        public TreatList Treats {get; set; }

        public Profile(int pAttackDice, int pDefenceDice)
        {
            MaxDice = int.MaxValue;
            AttackDice = pAttackDice;
            DefenceDice = pDefenceDice;
            AttackBonus = 0;
            DefenceBonus = 0;
            Treats = new TreatList();
        }

        public Profile(int pAttackDice, int pDefenceDice, int pAttackBonus, int pDefenceBonus)
        {
            MaxDice = int.MaxValue;
            AttackDice = pAttackDice;
            DefenceDice = pDefenceDice;
            AttackBonus = pAttackBonus;
            DefenceBonus = pDefenceBonus;
            Treats = new TreatList();
        }

        private int GetSL_Melee(Profile Defender)
        {
            DiceList dicesA, dicesD;
            int result;


            dicesA = new DiceList(AttackDice, "melee");
            dicesD = new DiceList(Defender.DefenceDice, "melee");
            // Careful, if you use reroll, the all process must be remade
            if (this.Treats.UnbreakableStrike.Has) {
                dicesD.RemoveHighestDice();
            }
            if (Defender.Treats.ImpenetrableDefence.Has) {
                dicesA.RemoveHighestDice();
            }

            result = dicesA.Result() - dicesD.Result();

            // brutal / parry
            if (this.Treats.Brutal.Has) {
                result += this.Treats.Brutal.Value;
            }
            if (Defender.Treats.Parry.Has) {
                result -= this.Treats.Parry.Value;
            }

            // reroll 1 
            // reroll 2
            // reroll 3

            if (result == 0 && dicesA.DicesList.Count < dicesD.DicesList.Count) {
                result = -1;
            }

            result = Math.Max(-1, result);
            result = Math.Min(10, result);

            return result;
        }

        private int GetSL_Ranged(int difficulty)
        {
            DiceList dicesA;
            int result;

            dicesA = new DiceList(AttackDice, "ranged");
            result = dicesA.Result() - difficulty;

            // reroll 1 
            // reroll 2
            // reroll 3

            result = Math.Max(-1, result);
            result = Math.Min(10, result);

            return result;
        }

        private int GetSL_Opposed(Profile Defender)
        {
            DiceList dicesA, dicesB;
            int result;

            dicesA = new DiceList(AttackDice, "opposed");
            dicesB = new DiceList(Defender.DefenceDice, "opposed");
            result = dicesA.Result() - dicesB.Result() + AttackBonus - Defender.DefenceBonus;

            if (result == 0 && dicesA.DicesList.Count < dicesB.DicesList.Count) {
                result = -1;
            }

            result = Math.Max(-1, result);
            result = Math.Min(10, result);

            return result;
        }

        private int GetWound(int sl, Profile Defender)
        {
            int tmp;
            if (sl >= 0)
            {
                tmp = Wound.GetWound(sl, this, Defender);
                if (14 < Math.Abs(tmp)) return 14; ;
                return tmp;
            }
            
            return 0;
        }

        public double[] Attack(Profile Defender, int max = 10000, int nbAttack = 1)
        {
            
            double[] result = new double[50];
            int sl, wound;

            if (max == 0) max = nbSimu;

            int woundTmp;
            bool miss;
            for (int i = 0; i < max; i++)
            {
                miss = true;
                woundTmp = 0;
                for (int j = 0; j < nbAttack; j++) {
                    sl = GetSL_Melee(Defender);
                    if (sl > -1) {
                        miss = false;
                        wound = GetWound(sl, Defender);
                        woundTmp += wound;
                    } 
                }
                result[woundTmp+1] += 1.0 / max;
                if (miss) result[0] += 1.0 / max;
            }

            return result;
        }

        public double[] Shoot(Profile Defender, int difficulty, int max = 10000, int nbAttack = 1)
        {

            double[] result = new double[50];
            int sl, wound;

            if (max == 0) max = nbSimu;

            int woundTmp;

            bool miss;
            for (int i = 0; i < max; i++) {
                miss = true;
                woundTmp = 0;
                for (int j = 0; j < nbAttack; j++) {
                    sl = GetSL_Ranged(difficulty);
                    if (sl > -1) {
                        miss = false;
                        wound = GetWound(sl, Defender);
                        woundTmp += wound;
                    }
                }
                result[woundTmp + 1] += 1.0 / max;
                if (miss) result[0] += 1.0 / max;
            }

            return result;
        }

        public double[] SuccessLevel_Opposed(Profile Defender, int max = 10000)
        {
            double[] result = new double[15];
            int sl;

            if (max == 0) max = nbSimu;

            for (int i = 0; i < max; i++) {
                sl = GetSL_Opposed(Defender);
                result[sl + 1] += 1.0 / max;
            }

            return result;
        }

        public double[] SuccessLevel_Melee(Profile Defender, int max = 10000)
        {
            double[] result = new double[15];
            int sl;

            if (max == 0) max = nbSimu;

            for (int i = 0; i < max; i++) {
                sl = GetSL_Melee(Defender);
                result[sl + 1] += 1.0 / max;
            }

            return result;
        }

        public double[] SuccessLevel_Ranged(int difficulty, int max = 10000)
        {
            double[] result = new double[15];
            int sl;

            if (max == 0) max = nbSimu;

            for (int i = 0; i < max; i++) {
                sl = GetSL_Ranged(difficulty);
                result[sl + 1] += 1.0 / max;
            }

            return result;
        }
    }
}
