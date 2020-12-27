using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            Simulation simulation = new Simulation();
            UserInterface.DisplayWelcome();
            bool runAgain;
            do
            {
                Console.Clear();
                simulation.RunSimulation();
                simulation.DisplayAllStats();
                runAgain = simulation.RunSimulationAgain();
            }
            while (runAgain);
        }
    }
}
