using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
     static class UserInterface
    {

        // member methods
        //prompt customers to begin transaction 
        public static void DisplayWelcome() 
        {
            Console.WriteLine("Press enter to enter payment and make a selection.");
            Console.ReadLine();
            Console.Clear();
        }

        //Card or Coin
        public static string ChoosePaymentMethod() 
        {
            Console.WriteLine("Enter 1 to pay with card, Enter 2 to pay with coins: ");
            string userInput = Console.ReadLine();
            string verifiedUserInput = Verification.VerifyUserInput(userInput, 1, 2);
            return verifiedUserInput;
        }

        //Customer chooses a soda
        public static string MakeSelection(SodaMachine sodaMachine, string paymentMethod) 
        {
            Console.WriteLine("Please make a selection: \n");
            sodaMachine.DisplayCurrentInventory();
            string userInput;
            string verifiedUserInput;
            if (paymentMethod == "1") // Display if card payment
            {
                Console.Write("Select a soda: ");
                userInput = Console.ReadLine();
                verifiedUserInput = Verification.VerifyUserInput(userInput, 1, 3); // Only allow customer to choose a soda
            }
            else // Display if coin payment
            {
                Console.Write("\nSelect a soda, or enter 0 to insert more coins: ");
                userInput = Console.ReadLine();
                verifiedUserInput = Verification.VerifyUserInput(userInput, 0, 3); // Allow customer to choose to input more coins
                if (verifiedUserInput == "0") // Return to prompt interface to enter more coins
                {
                    return verifiedUserInput;
                }
            }
            bool canExists = false;
            while (!canExists)
            {
                switch (verifiedUserInput)
                {
                    case "1":
                        canExists = Verification.CheckIfObjectExists(sodaMachine.inventory, "Cola");
                        break;
                    case "2":
                        canExists = Verification.CheckIfObjectExists(sodaMachine.inventory, "Orange Soda");
                        break;
                    case "3":
                        canExists = Verification.CheckIfObjectExists(sodaMachine.inventory, "Root Beer");
                        break;
                }
                if (!canExists) // Prompt to make another choice if soda their is sold out
                {
                    Console.WriteLine("This soda is sold out. Please make another selection.");
                    userInput = Console.ReadLine();
                    verifiedUserInput = Verification.VerifyUserInput(userInput, 1, 3);
                }
            }
            return verifiedUserInput;
        }
    }
}
