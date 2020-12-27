using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    class SodaMachine
    {
        // member variables (HAS A)
        public List<Coin> register;
        public List<Coin> hopperIn;
        public List<Coin> hopperOut;
        public List<Can> inventory;
        private double cardPaymentBalance;

        public double CardPaymentBalance
        {
            get
            {
                return cardPaymentBalance;
            }
            set
            {
                cardPaymentBalance += value;
            }
        }


        // constructor (SPAWN)
        public SodaMachine()
        {
            hopperIn = new List<Coin>();
            hopperOut = new List<Coin>();
            register = new List<Coin>();
            Quarter quarter = new Quarter();
            Dime dime = new Dime();
            Nickel nickel = new Nickel();
            Penny penny = new Penny();
            AddToInventory(quarter, 20);
            AddToInventory(dime, 10);
            AddToInventory(nickel, 20);
            AddToInventory(penny, 50);

            inventory = new List<Can>();
            Cola cola = new Cola();
            OrangeSoda orangeSoda = new OrangeSoda();
            RootBeer rootBeer = new RootBeer();
            AddToInventory(cola, 5);
            AddToInventory(orangeSoda, 5);
            AddToInventory(rootBeer, 5);

            cardPaymentBalance = 0;
        }

        // member methods (CAN DO)
        private void AddToInventory(Coin coin, int numberOfCoins)
        {
            for (int i = 0; i < numberOfCoins; i++)
            {
                register.Add(coin);
            }
        }

        private void AddToInventory(Can can, int numberOfCans)
        {
            for (int i = 0; i < numberOfCans; i++)
            {
                inventory.Add(can);
            }
        }

        public void DisplayCurrentInventory()
        {
            int colaCansAvailable = 0;
            int orangeSodaCansAvailable = 0;
            int rootBeerCansAvailable = 0;
            foreach (Can can in inventory)
            {
                switch (can.name)
                {
                    case "Cola":
                        colaCansAvailable++;
                        break;
                    case "Orange Soda":
                        orangeSodaCansAvailable++;
                        break;
                    case "Root Beer":
                        rootBeerCansAvailable++;
                        break;
                }
            }
            if (colaCansAvailable > 0)
            {
                Console.WriteLine($"Enter 1 for Cola ($0.35)"); // get values from objects
            }
            else
            {
                Console.WriteLine("Cola (sold out)");
            }

            if (orangeSodaCansAvailable > 0)
            {
                Console.WriteLine("Enter 2 for Orange Soda ($0.06)");
            }
            else
            {
                Console.WriteLine("Orange Soda (sold out)");
            }

            if (rootBeerCansAvailable > 0)
            {
                Console.WriteLine("Enter 3 for Root Beer ($0.60)");
            }
            else
            {
                Console.WriteLine("Root Beer (sold out)");
            }
        }

        // CARD TRANSACTIONS //

        public bool ProcessTransaction(string userInput, Customer customer) // Card transaction
        {
            bool canCompleteTransaction;
            double sodaCost = Math.Round(GetSodaCost(userInput), 2); // Collect cost info
            if (sodaCost > Math.Round(customer.wallet.card.AvailableFunds, 2)) // Determine whether adequate funds are available
            {
                canCompleteTransaction = false;
            }
            else
            {
                canCompleteTransaction = true;
            }
            return canCompleteTransaction;
        }

        public void CompleteTransaction(Customer customer, double sodaCost, string userSelection) // Card transaction
        {
            Console.WriteLine("\nCard payment processing!");

            cardPaymentBalance += Math.Round(sodaCost, 2); // Credit funds to soda machine
            Console.WriteLine($"\nThe soda machine now has a card payment balance of {cardPaymentBalance:C2}");

            customer.wallet.card.AvailableFunds -= Math.Round(sodaCost, 2); // Debit funds from customer card
            Console.WriteLine($"The customer's card now has {customer.wallet.card.AvailableFunds:C2} in available funds");

            customer.backpack.cans.Add(DispenseSoda(userSelection)); // Machine dispenses soda to customer
            customer.DisplayContents(customer.backpack);
        }

        public void CancelTransaction() // Card transaction
        {
            Console.WriteLine("Cannot complete transaction - inadqeuate funds.");
        }

        // COIN TRANSACTIONS //

        public bool ProcessTransaction(string userInput) // Coin transaction
        {
            bool canCompleteTransaction;
            double moneyInHopper = Math.Round(Verification.CountMoney(hopperIn), 2); // Count money in hopper
            double sodaCost = Math.Round(GetSodaCost(userInput), 2); // Collect cost info
            if (sodaCost > moneyInHopper)
            {
                Console.WriteLine("\nTransaction cannot be completed - inadequate funds.");
                canCompleteTransaction = false;
            }
            else if (sodaCost == moneyInHopper)
            {
                Console.WriteLine("\nTransaction complete, no change due!");
                canCompleteTransaction = true;
            }
            else
            {
                double changeDue = Math.Round(moneyInHopper - sodaCost, 2); // Calculate change required
                bool canGiveExactChange = DispenseChange(changeDue); // Checks if exact change can be given
                if (canGiveExactChange == true)
                {
                    Console.WriteLine("\nTransaction complete, pleace collect your change!");
                    canCompleteTransaction = true;
                }
                else
                {
                    Console.WriteLine("\nTransaction cannot be completed - exact change unavailable.");
                    canCompleteTransaction = false;
                }
            }
            return canCompleteTransaction;
        }

        public void CompleteTransaction(Customer customer, string userSelection) // Coin transaction
        {
            foreach (Coin coin in hopperIn.ToList()) // soda machine takes in money from hopper in
            {
                register.Add(coin);
                hopperIn.Remove(coin);
            }
            Console.WriteLine($"The soda machine register now contains {Verification.CountMoney(register),2:C2}");

            customer.backpack.cans.Add(DispenseSoda(userSelection)); // soda machine dispenses soda
            customer.DisplayContents(customer.backpack);
            foreach (Coin coin in hopperOut.ToList()) // soda machine dispenses change
            {
                customer.wallet.coins.Add(coin);
                hopperOut.Remove(coin);
            }
            customer.DisplayContents(customer.wallet);
        }

        public void CancelTransaction(Customer customer) // Coin transaction
        {
            foreach (Coin coin in hopperIn.ToList()) // soda machine returns money from hopper in
            {
                customer.wallet.coins.Add(coin);
                hopperIn.Remove(coin);
            }
            Console.WriteLine($"The soda machine register now contains {Verification.CountMoney(register),2:C2}");
            customer.DisplayContents(customer.backpack);
            customer.DisplayContents(customer.wallet);
        }

        public static double GetSodaCost(string userInput)
        {
            double sodaCost = 0;
            switch (userInput)
            {
                case "1":
                    sodaCost = 0.35;
                    break;
                case "2":
                    sodaCost = 0.06;
                    break;
                case "3":
                    sodaCost = 0.60;
                    break;
            }
            return Math.Round(sodaCost, 2);
        }

        private Can DispenseSoda(string userSelection)
        {
            Can dispensedSoda = null;
            switch (userSelection)
            {
                case "1":
                    for (int i = 0; i < inventory.Count; i++)
                    {
                        if (inventory[i].name == "Cola")
                        {
                            dispensedSoda = inventory[i];
                            inventory.RemoveAt(i);
                            break;
                        }
                    }
                    break;
                case "2":
                    for (int i = 0; i < inventory.Count; i++)
                    {
                        if (inventory[i].name == "Orange Soda")
                        {
                            dispensedSoda = inventory[i];
                            inventory.RemoveAt(i);
                            break;
                        }
                    }
                    break;
                case "3":
                    for (int i = 0; i < inventory.Count; i++)
                    {
                        if (inventory[i].name == "Root Beer")
                        {
                            dispensedSoda = inventory[i];
                            inventory.RemoveAt(i);
                            break;
                        }
                    }
                    break;
            }
            return dispensedSoda;
        }

        private bool DispenseChange(double changeDue)
        {
            // Calculate total change available, ensure than change available is greater than change due
            bool canGiveExactChange = false;
            while (Math.Round(changeDue, 2) >= 0.25)
            {
                bool coinExists = Verification.CheckIfObjectExists(register, "Quarter");// Check if quarters exist
                if (coinExists)
                {
                    for (int i = 0; i < register.Count; i++)
                    {
                        if (register[i].name == "Quarter")// Iterate through and remove quarter
                        {
                            hopperOut.Add(register[i]); // Add removed coins to hopper
                            register.RemoveAt(i); // Remove coin from register
                            changeDue = Math.Round(changeDue - 0.25, 2); // Decrease change due
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            while (Math.Round(changeDue, 2) >= 0.10)
            {
                bool coinExists = Verification.CheckIfObjectExists(register, "Dime");
                if (coinExists)
                {
                    for (int i = 0; i < register.Count; i++)
                    {
                        if (register[i].name == "Dime")// Iterate through and remove quarter
                        {
                            hopperOut.Add(register[i]); // Add removed coins to hopper
                            register.RemoveAt(i); // Remove coin from register
                            changeDue = Math.Round(changeDue - 0.10, 2); // Decrease change due
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            while (Math.Round(changeDue, 2) >= 0.05)
            {
                bool coinExists = Verification.CheckIfObjectExists(register, "Nickel");
                if (coinExists)
                {
                    for (int i = 0; i < register.Count; i++)
                    {
                        if (register[i].name == "Nickel")// Iterate through and remove quarter
                        {
                            hopperOut.Add(register[i]); // Add removed coins to hopper
                            register.RemoveAt(i); // Remove coin from register
                            changeDue = Math.Round(changeDue - 0.05, 2); // Decrease change due
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            while (Math.Round(changeDue, 2) > 0)
            {
                bool coinExists = Verification.CheckIfObjectExists(register, "Penny");
                if (coinExists)
                {
                    for (int i = 0; i < register.Count; i++)
                    {
                        if (register[i].name == "Penny")// Iterate through and remove quarter
                        {
                            hopperOut.Add(register[i]); // Add removed coins to hopper
                            register.RemoveAt(i); // Remove coin from register
                            changeDue = Math.Round(changeDue - 0.01, 2); // Decrease change due
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            // Add removed coins to hopper, dispense only if reach even change
            if (Math.Round(changeDue, 2) == 0.00)
            {
                canGiveExactChange = true;
                // Transfer coins from hopper to customer
                return canGiveExactChange;
            }
            else
            {
                // Return coins to machine register
                return canGiveExactChange;
            }
        }
    }
}