using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    abstract class Can
    {
        // member variables (HAS A)
        public string name;
        protected double cost;
        public double Cost
        {
            get
            {
                return cost;
            }
        }

        // constructor (SPAWN)

        // member methods (CAN DO)
    }
}