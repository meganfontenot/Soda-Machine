using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
     static class UserInterface
    {
        // member variables

        // constructor

        // member methods
        public static void DisplayWelcome() // Generic display before customer begins transaction
        {
            Console.WriteLine("Press enter to enter payment and make a selection.");
            Console.ReadLine();
            Console.Clear();
        }

        public static string ChoosePaymentMethod() // Choose card or coin payment
        {
            Console.WriteLine("Enter 1 to pay with card, Enter 2 to pay with coins: ");
            string userInput = Console.ReadLine();
            string verifiedUserInput = Verification.VerifyUserInput(userInput, 1, 2);
            return verifiedUserInput;
        }

        public static string MakeSelection(SodaMachine sodaMachine, string paymentMethod) // Customer chooses soda
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
                if (verifiedUserInput == "0") // Return to interface to enter more coins
                {
                    return verifiedUserInput;
                }
            }
            // Move this to Verification?
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
                if (!canExists) // Prompt to make another choice if soda is sold out
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
