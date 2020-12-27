using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    class Simulation
    {
        // member variables

        public SodaMachine sodaMachine;
        public Customer customer;

        // constructor

        public Simulation()
        {
            sodaMachine = new SodaMachine();
            customer = new Customer();
        }

        // member methods

        public void RunSimulation()
        {
            // Choose card or coin
            string paymentMethod = UserInterface.ChoosePaymentMethod(); 
            string userSelection = "0";
            if (paymentMethod == "1") 
            {
                Console.Clear();
                //display available funds
                customer.InsertPayment(customer.wallet.card);
                //pick soda
                userSelection = UserInterface.MakeSelection(sodaMachine, paymentMethod);
                //check  funds are available
                bool canCompleteTransaction = sodaMachine.ProcessTransaction(userSelection, customer); 
                if (canCompleteTransaction) 
                {
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
                //User can enter more money
                while (userSelection == "0")
                {
                    //Insert coin into hopper
                    customer.InsertPayment(sodaMachine); 
                    double moneyInHopper = Math.Round(Verification.CountMoney(sodaMachine.hopperIn), 2); // Count money in hopper
                    Console.Clear();
                    Console.WriteLine($"Money inserted: {moneyInHopper:C2}\n"); // Display money in hopper
                    //Choose soda OR enter more money
                    userSelection = UserInterface.MakeSelection(sodaMachine, paymentMethod); 
                } // Break once user chooses soda
                bool canCompleteTransaction = sodaMachine.ProcessTransaction(userSelection); 
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

        //Display "state" of simulation. (how objects change throughout transaction process
        public void DisplayAllStats() 
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

        //Give option to purchase again
        //run simulation again
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