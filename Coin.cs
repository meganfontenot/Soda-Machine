using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    abstract class Coin
    {
        // member variables (HAS A)
        public string name;
        protected double value;
        public double Value
        {
            get
            {
                return value;
            }
        }

        // constructor (SPAWN)

        // member methods (CAN DO)
    }
}
