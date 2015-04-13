using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statistics {

    // This class provides an interface to manage easily all the 
    // treat during a simulation
    public class TreatList {
        // convention : I do not have : -1 / Do have, no number required : 0 / Do have, level of X : X
        // Attack
        public Treat Assassin { get; set; } // OK
        public Treat Brutal { get; set; } // OK
        public Treat Strengh { get; set; } // OK
        public Treat ArmourPiercing { get; set; } // OK
        public Treat Sharp { get; set; } // OK
        public Treat Strong { get; set; } // OK
        public Treat Weak { get; set; } // OK
        public Treat UnbreakableStrike { get; set; } // OK
        // Defence
        public Treat Parry { get; set; } // OK
        public Treat Armour { get; set; } // OK
        public Treat Toughness { get; set; } // OK
        public Treat Durable { get; set; } // OK
        public Treat ImpenetrableDefence { get; set; } // OK
        // Ki
        public Treat ForceOfWill { get; set; } // KOKO
        public Treat IronMind { get; set; } // KOKO
        public Treat StrongMind { get; set; } // KOKO
        public Treat WeakMind { get; set; } // KOKO
        // reroll
        public Treat MartialProwess { get; set; } // KOKO
        public Treat Dodge { get; set; } // KOKO // KOKO
        public Treat Feint { get; set; }
        // shoot
        public Treat RangedDefence { get; set; } // KOKO
        public Treat RapidFire { get; set; } // KOKO
        public Treat Bonus { get; set; } // KOKO

        // Dictionnary of the Treats
        // The ki is the name of the treat
        public Dictionary<string, Treat> TreatDictionnary { get; set; }

        public void InitializeValue()
        {
            // Attack
            Assassin = new Treat("Assassin", (int)Treat.TreatType.PhysicAttack +
                                                (int)Treat.TreatType.RangeAttack, pValueNeed: false);
            Brutal = new Treat("Brutal", (int)Treat.TreatType.PhysicAttack +
                                                (int)Treat.TreatType.RangeAttack);
            Strengh = new Treat("Strengh", Treat.TreatType.PhysicAttack);
            ArmourPiercing = new Treat("Armour Piercing", Treat.TreatType.PhysicAttack, pValueNeed: false);
            Sharp = new Treat("Sharp", Treat.TreatType.PhysicAttack, pValueNeed: false);
            Strong = new Treat("Strong", Treat.TreatType.PhysicAttack, pValueNeed: false);
            UnbreakableStrike = new Treat("Unbreakable Strike", Treat.TreatType.PhysicAttack);
            Weak = new Treat("Weak", Treat.TreatType.PhysicAttack, pValueNeed: false);
            // Defence
            Parry = new Treat("Parry", Treat.TreatType.PhysicDefence);
            Armour = new Treat("Armour", (int)Treat.TreatType.PhysicDefence +
                                            (int)Treat.TreatType.RangeDefence);
            Toughness = new Treat("Toughness", (int)Treat.TreatType.PhysicDefence +
                                            (int)Treat.TreatType.RangeDefence);
            Durable = new Treat("Durable", (int)Treat.TreatType.PhysicDefence +
                                            (int)Treat.TreatType.RangeDefence, pValueNeed: false);
            ImpenetrableDefence = new Treat("Impenetrable Defence",
                                            Treat.TreatType.PhysicDefence, pValueNeed: false);
            // Ki
            ForceOfWill = new Treat("Force of Will", Treat.TreatType.KiAttack);
            IronMind = new Treat("Iron Mind", Treat.TreatType.KiDefence);
            StrongMind = new Treat("Strong Mind", (int)Treat.TreatType.KiAttack
                                    + (int)Treat.TreatType.KiDefence);
            WeakMind = new Treat("Weak Mind", (int)Treat.TreatType.KiAttack
                                    + (int)Treat.TreatType.KiDefence);
            // reroll
            MartialProwess = new Treat("Martial Prowess", (int)Treat.TreatType.PhysicAttack
                                        + (int)Treat.TreatType.PhysicDefence);
            Dodge = new Treat("Dodge", Treat.TreatType.PhysicDefence);
            Feint = new Treat("Feint", (int)Treat.TreatType.PhysicAttack);

            // shoot
            RangedDefence = new Treat("Ranged Defence", (int)Treat.TreatType.RangeDefence);
            RapidFire = new Treat("Rapid Fire", (int)Treat.TreatType.RangeAttack);
            Bonus = new Treat("Bonus (Exhausted, big...)", (int)Treat.TreatType.RangeDefence);
        }

        private static void AddToDic(Dictionary<string, Treat> Dic, Treat Val)
        {
            Dic.Add(Val.Name, Val);
        }

        private void InitializeDictionary()
        {
            TreatDictionnary = new Dictionary<string, Treat>();
            AddToDic(TreatDictionnary, Assassin);
            AddToDic(TreatDictionnary, Brutal);
            AddToDic(TreatDictionnary, Strengh);
            AddToDic(TreatDictionnary, ArmourPiercing);
            AddToDic(TreatDictionnary, Sharp);
            AddToDic(TreatDictionnary, Strong);
            AddToDic(TreatDictionnary, UnbreakableStrike);
            AddToDic(TreatDictionnary, Weak);
            AddToDic(TreatDictionnary, Parry);
            AddToDic(TreatDictionnary, Armour);
            AddToDic(TreatDictionnary, Toughness);
            AddToDic(TreatDictionnary, Durable);
            AddToDic(TreatDictionnary, ImpenetrableDefence);
            AddToDic(TreatDictionnary, ForceOfWill);
            AddToDic(TreatDictionnary, IronMind);
            AddToDic(TreatDictionnary, StrongMind);
            AddToDic(TreatDictionnary, WeakMind);
            AddToDic(TreatDictionnary, MartialProwess);
            AddToDic(TreatDictionnary, Dodge);
            AddToDic(TreatDictionnary, Feint);
            AddToDic(TreatDictionnary, RangedDefence);
            AddToDic(TreatDictionnary, RapidFire);
            AddToDic(TreatDictionnary, Bonus);
        }

        public TreatList()
        {
            InitializeValue();
            InitializeDictionary();
        }

        public List<Treat> getTreatList()
        {
            // The string is the name
            // The boolean indicates if the treat needs a value

            var query =
                from T in TreatDictionnary.Values
                select T;

            return query.ToList();
        }

        public List<Treat> getAttackerMeleeList()
        {
            // The string is the name
            // The boolean indicates if the treat needs a value

            var query =
                from T in TreatDictionnary.Values
                where (T.IsType(Treat.TreatType.PhysicAttack))
                select T;

            return query.ToList();
        }

        public List<Treat> getDefenderMeleeList()
        {
            // The string is the name
            // The boolean indicates if the treat needs a value

            var query =
                from T in TreatDictionnary.Values
                where (T.IsType(Treat.TreatType.PhysicDefence))
                select T;

            return query.ToList();
        }

        public bool Has(string name)
        {
            try {
                return TreatDictionnary[name].Has;
            } catch {
                return false;
            }
        }

        public void ResetTreatList()
        {
            foreach (KeyValuePair<string, statistics.Treat> kvp in TreatDictionnary) {
                TreatDictionnary[kvp.Key].Has = false;
            }
        }

        public void Modify(List<Tuple<string, string>> Treats)
        {
            if (Treats == null) return;

            foreach (Tuple<string, string> T in Treats) {
                Modify(T.Item1, T.Item2);
            }
        }

        private void Modify(string treat, string value)
        {
            TreatDictionnary[treat].Has = true;
            if (TreatDictionnary[treat].NeedValue) {
                TreatDictionnary[treat].Value = int.Parse(value);
            }
        }
    }
}
