using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    class Card
    {
        // member variables (HAS A)
        private double availableFunds;
        public double AvailableFunds
        {
            get
            {
                return availableFunds;
            }
            set
            {
                availableFunds = value;
            }
        }

        // constructor (SPAWN)
        public Card()
        {
            availableFunds = 5.00;
        }

        // member methods (CAN DO)
    }
}
