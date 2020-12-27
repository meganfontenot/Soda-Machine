using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    abstract class Can
    {
        // member variables 
        public string name;
        protected double cost;
        public double Cost
        {
            get
            {
                return cost;
            }
        }
    }
}