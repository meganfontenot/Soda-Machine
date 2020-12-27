using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    class Card
    {
        // member variables
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

        // constructor
        public Card()
        {
            availableFunds = 5.00;
        }
    }
}
