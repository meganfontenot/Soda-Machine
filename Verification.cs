using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    static class Verification
    {
        //Make sure user puts in correct responses when prompted
        public static string VerifyUserInput(string userInput, int start, int end)
        {
            string verifiedUserInput = "";
            int userInputInt;
            bool isNumber = int.TryParse(userInput, out userInputInt);
            while (!isNumber || userInputInt < start || userInputInt > end)
            {
                Console.WriteLine("Invalid selection, please choose again: ");
                userInput = Console.ReadLine();
                isNumber = int.TryParse(userInput, out userInputInt);
            }
            verifiedUserInput = userInput;
            return verifiedUserInput;
        }

        //Need to make sure coin is in register
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

        //Need to make sure soda is in inventory 
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
        //Count money
        public static double CountMoney(List<Coin> repository)
        {
            double totalMoney = 0;
            foreach (Coin coin in repository)
            {
                totalMoney += coin.Value;
            }
            return totalMoney;
        }

    }
}
