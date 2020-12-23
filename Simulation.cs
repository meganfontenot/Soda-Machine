using Soda_Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySodaMachine
{
    static class Verification
    {
        public static string VerifyUserInput(string userInput, int start, int end)
        {
            string verifiedUserInput = "";
            int userInputInt;
            bool isNumber = int.TryParse(userInput, out userInputInt);
            while (!isNumber || userInputInt < start || userInputInt > end)
            {
                Console.WriteLine("Invalid selection. please choose again: ");
                userInput = Console.ReadLine();
                isNumber = int.TryParse(userInput, out userInputInt);
            }
            verifiedUserInput = userInput;
            return verifiedUserInput;
        }

        public static bool CheckIfObjectExists(List<Coin> register, string coinName)
        {
            bool coinExists = false;
            foreach (Coin coin in register)
            {
                if (coin.name == coinName)
                {
                    coinExists = true;
                    return coinExists;
                }
            }
            return coinExists;
        }

        public static bool CheckIfObjectExists(List<Can> inventory, string sodaName)
        {
            bool canExists = false;
            foreach (Can can in inventory)
            {
                if (can.name == sodaName)
                {
                    canExists = true;
                    return canExists;
                }
            }
            return canExists;
        }
    }
}
