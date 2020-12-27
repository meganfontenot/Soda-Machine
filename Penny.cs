using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    class Penny : Coin // (IS A)
    {
        // member variables (HAS A)

        // constructor (SPAWN)
        public Penny()
        {
            name = "Penny";
            value = 0.01;
        }

        // member methods (CAN DO)
    }
}
