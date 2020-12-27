using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    class Simulation
    {
        // member variables (HAS A)

        public SodaMachine sodaMachine;
        public Customer customer;

        // constructor (SPAWN)

        public Simulation()
        {
            sodaMachine = new SodaMachine();
            customer = new Customer();
        }

        // member methods (CAN DO)

        public void RunSimulation()
        {
            string paymentMethod = UserInterface.ChoosePaymentMethod(); // Choose card or coin payment
            string userSelection = "0";
            if (paymentMethod == "1") // Card payment
            {
                Console.Clear();
                customer.InsertPayment(customer.wallet.card); // Display available funds
                userSelection = UserInterface.MakeSelection(sodaMachine, paymentMethod); // Choose soda
                bool canCompleteTransaction = sodaMachine.ProcessTransaction(userSelection, customer); // Verify adequate funds are available
                if (canCompleteTransaction) // If funds available
                {
                    // Clean this up - redundant call of GetSodaCost()
                    double sodaCost = Math.Round(SodaMachine.GetSodaCost(userSelection), 2);
                    sodaMachine.CompleteTransaction(customer, sodaCost, userSelection);
                }
                else // Funds not available
                {
                    sodaMachine.CancelTransaction();
                }
            }
            else
            {
                while (userSelection == "0") // User opts to enter more money
                {
                    customer.InsertPayment(sodaMachine); // Insert coin into machine, stored in hopperIn
                    double moneyInHopper = Math.Round(Verification.CountMoney(sodaMachine.hopperIn), 2); // Calculate money currently in hopper
                    Console.Clear();
                    Console.WriteLine($"Money inserted: {moneyInHopper:C2}\n"); // Display money in hopper
                    userSelection = UserInterface.MakeSelection(sodaMachine, paymentMethod); // Choose soda or enter more money
                } // Break once user chooses soda
                bool canCompleteTransaction = sodaMachine.ProcessTransaction(userSelection); // Calculate money inserted v cost of soda
                if (canCompleteTransaction) // If adequate amount of money passed in
                {
                    sodaMachine.CompleteTransaction(customer, userSelection);
                }
                else // If inadequate amount of money passed in
                {
                    sodaMachine.CancelTransaction(customer);
                }
            }

        }

        public void DisplayAllStats() // Display current state of simulation - see how objects have changed hands throughout transaction
        {
            Console.WriteLine("\nPress enter to display all current simulation statistics: ");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Soda machine:" +
                $"\n{sodaMachine.inventory.Count} cans in inventory" +
                $"\n{sodaMachine.register.Count} coins in register totalling {Verification.CountMoney(sodaMachine.register):C2}" +
                $"\n{sodaMachine.CardPaymentBalance:C2} in card credits.\n");
            Console.WriteLine("Customer:" +
                $"\nCard balance of {customer.wallet.card.AvailableFunds:C2}" +
                $"\nWallet contains {Verification.CountMoney(customer.wallet.coins):C2}");
            customer.DisplayContents(customer.backpack);
        }

        public bool RunSimulationAgain()
        {
            bool runAgain;
            Console.Write("\nWould you like to purchase another soda?\nType 1 for yes, Type 2 to end simulation: ");
            string userInput = Console.ReadLine();
            string verifiedUserInput = Verification.VerifyUserInput(userInput, 1, 2);
            if (verifiedUserInput == "1")
            {
                runAgain = true;
                return runAgain;
            }
            else
            {
                runAgain = false;
                return runAgain;
            }
        }
    }
}