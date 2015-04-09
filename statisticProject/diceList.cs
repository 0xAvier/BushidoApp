using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statistics
{
    // This class provides functions to manipulate different kind of list of dices
    class DiceList
    {

        // Create a list of dice 
        public DiceList(int nbDices, string typeOfDice)
        {
            Type = typeOfDice;
            DicesList = new List<int>();
            for (int i = 0; i < nbDices; i++)
            {
                DicesList.Add(Dices.BushidoDice());
            }

            TreatList();
        }

        // Checks if the type of dice is correct
        // i.e. "normal", "melee", "ranged" or "opposed"
        private void checkType(string type)
        {
            if (type != "normal" && type != "melee" && type != "ranged" &&
                type != "opposed")
            {
                throw new Exception();
            }
        }
        
        // Contains the dices of the roll
        public List<int> DicesList { get; set; }
        
        // normal ; melee ; ranged ; opposed
        private string _type;
        // Viewable value of the type with a protected getter & setter
        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                // Check the type before setting it
                checkType(value);
                this._type = value;
            }
        }

        // Display the roll
        public void Display()
        {
            Output.Print("\n");
            foreach (int i in DicesList)
            {
                Output.Print(i.ToString() + " ");
            }
            Output.Print("\n");
        }

        // Process the list of dices to extract a result
        public void TreatList()
        {
            // Sort the dice in decreasing order
            DicesList.Sort();
            DicesList.Reverse();
            // Remove all the one
            DicesList.RemoveAll(x => x == 1);
            DicesList.RemoveAll(x => x == 0);
        }

        // Remove the highest roll
        public void RemoveHighestDice()
        {
            DicesList.RemoveAt(0);
        }

        // Get the highest roll
        public int getMax()
        {
            // If the list is empty, return 0
            if (DicesList.Count == 0) return -1;
            // Otherwise, return the first element
            // The list must be sorted at this point
            else return DicesList[0];
        }

        // Return the result of the roll, according to its type
        public int Result()
        {
            switch(Type)
            {
                case "normal":
                    return getNormalResult();
                case "melee":
                    return getMeleeResult();
                case "ranged":
                    return getRangedResult();
                case "opposed":
                    return getOpposedResult();
            }
            return 0;
        }

        public int getNormalResult()
        {
            return getMax();
        }


        private static bool isOne(int x)
        {
            return x == 1;
        }

        // Return the result of a roll, considered as a melee roll
        public int getMeleeResult()
        {
            // Get the maximal dice
            int result = getMax();
            // Find the two others maximal value
            int i;
            // Treat the two next dices
            for (i = 1; i < Math.Min(3, DicesList.Count); i++)
            {
                result += (DicesList[i] == 6) ? 2 : 1;
            }

            // Add the other +1 for the extra 6
            if (DicesList.Count > 3)
            {
                // Get the total number of 6
                int nbSix = DicesList.FindAll(x => x == 6).Count;
                // If there is more than three 6
                if (nbSix > 3)
                {
                    // you have to add those which haven't been treated here
                    result += nbSix - 3;
                }
            }

            // Return the result
            return result;
        }

        // Return the result of a roll, considered as a ranged roll
        public int getRangedResult()
        {
            // Similar to a melee result
            return getMeleeResult();
        }

        // Return the result of a roll, considered as a opposed roll
        public int getOpposedResult()
        {
            // Initialise the result to the maximal value
            int result = getMax();
            // Try to add others 6
            // Get the number of 6 of the roll
            int nbSix = DicesList.FindAll(x => x == 6).Count;
            // Add every 6 that has not been counted yet
            result += Math.Min(0, nbSix - 1);

            return result;
        }

        static public bool LocalTest()
        {
            bool res = true;
            res &= TestMeleeDice();

            return res;
        }

        static public bool TestMeleeDice()
        {
            bool functionnal = true;
            DiceList dices = new DiceList(3, "melee");

            // 2 ; 1 ; 2-3 ; 2-1 ; 6 - 2 ; 6-6 ; 5-3-2 ; 5-3-1 ; 6-6-2 ; 6-6-6-6
            // 2 / 2
            if (Test.verbose) Output.Print("Test dices with 2:    ");
            dices.DicesList = new List<int>();
            dices.DicesList.Add(2);
            dices.TreatList();
            functionnal &= Test.TestValue(2, dices.Result());
            // 3-2 / 4
            if (Test.verbose) Output.Print("Test dices with 3-2:    ");
            dices.DicesList = new List<int>();
            dices.DicesList.Add(3);
            dices.DicesList.Add(2);
            dices.TreatList();
            functionnal &= Test.TestValue(4, dices.Result());
            // 2-1 / 2
            if (Test.verbose) Output.Print("Test dices with 2-1:    ");
            dices.DicesList = new List<int>();
            dices.DicesList.Add(2);
            dices.DicesList.Add(1);
            dices.TreatList();
            functionnal &= Test.TestValue(2, dices.Result());
            // 6-2 / 7
            if (Test.verbose) Output.Print("Test dices with 6-2:    ");
            dices.DicesList = new List<int>();
            dices.DicesList.Add(6);
            dices.DicesList.Add(2);
            dices.TreatList();
            functionnal &= Test.TestValue(7, dices.Result());
            // 6-6 / 8
            if (Test.verbose) Output.Print("Test dices with 6-6:    ");
            dices.DicesList = new List<int>();
            dices.DicesList.Add(6);
            dices.DicesList.Add(6);
            dices.TreatList();
            functionnal &= Test.TestValue(8, dices.Result());
            // 5-3-2 / 7
            if (Test.verbose) Output.Print("Test dices with 5-3-2:    ");
            dices.DicesList = new List<int>();
            dices.DicesList.Add(5);
            dices.DicesList.Add(3);
            dices.DicesList.Add(2);
            dices.TreatList();
            functionnal &= Test.TestValue(7, dices.Result());
            // 5-3-1 / 6
            if (Test.verbose) Output.Print("Test dices with 5-3-1:    ");
            dices.DicesList = new List<int>();
            dices.DicesList.Add(5);
            dices.DicesList.Add(3);
            dices.DicesList.Add(1);
            dices.TreatList();
            functionnal &= Test.TestValue(6, dices.Result());
            // 6-6-2 / 8
            if (Test.verbose) Output.Print("Test dices with 6-6-2:    ");
            dices.DicesList = new List<int>();
            dices.DicesList.Add(6);
            dices.DicesList.Add(6);
            dices.DicesList.Add(2);
            dices.TreatList();
            functionnal &= Test.TestValue(9, dices.Result());
            // 6-6-6-6 / 11
            if (Test.verbose) Output.Print("Test dices with 6-6-6-6:    ");
            dices.DicesList = new List<int>();
            dices.DicesList.Add(6);
            dices.DicesList.Add(6);
            dices.DicesList.Add(6);
            dices.DicesList.Add(6);
            dices.TreatList();
            functionnal &= Test.TestValue(11, dices.Result());

            return functionnal;
        }
    }
}
