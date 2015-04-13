using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statistics {

    public class Treat {
        // Name of the treat
        public string Name { get; set; }
        // If "Has" is set to true, the profile has the profile
        public bool Has { get; set; }
        // Is the value of the treat
        // Pertinent only if Has is True 
        public int Value { get; set; }
        // Indicates the use of the Type
        // Physic Attack, Physic Defence, Ki Attack, Ki Defence
        public enum TreatType { PhysicAttack = 1, PhysicDefence = 10, 
                                KiAttack = 100, KiDefence = 1000,
                                RangeAttack = 10000, RangeDefence = 100000};
        public int Type { get; set; }
        // Indicates if a value is need
        // e.g. Sharp does not need a value
        public bool NeedValue { get; set; }

        public Treat(string pName) {
            Name = pName;
            Has = false;
            Type = (int) TreatType.PhysicAttack;
            NeedValue = true;
        }
        
        public Treat(string pName, TreatType pType = TreatType.PhysicAttack,
                     bool pValueNeed = true) : this(pName) {
            Type = (int) pType;
            NeedValue = pValueNeed;
        }
        
        public Treat(string pName, int pType = (int) TreatType.PhysicAttack,
                     bool pValueNeed = true)
            : this(pName) {
            Type = pType;
            NeedValue = pValueNeed;
        }

        public bool IsType(TreatType T) {
            int tmp;
            // Get the value
            tmp = Type;
            // Remove all bigger type
            tmp %= ((int) T * 10);
            // Check if T is set
            return (tmp >= (int) T);
        }
    }
}
