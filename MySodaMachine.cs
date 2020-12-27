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
        // member variables 
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
        // SODA MACHINE INVENTORY //

        //Add to inventory (coins)
        private void AddToInventory(Coin coin, int numberOfCoins)
        {
            for (int i = 0; i < numberOfCoins; i++)
            {
                register.Add(coin);
            }
        }

        //Add to inventory (cans)
        private void AddToInventory(Can can, int numberOfCans)
        {
            for (int i = 0; i < numberOfCans; i++)
            {
                inventory.Add(can);
            }
        }

        //Show the current inventory
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

        //Process a Card Transaction 
        public bool ProcessTransaction(string userInput, Customer customer) 
        {
            bool canCompleteTransaction;
            //Collect cost info
            double sodaCost = Math.Round(GetSodaCost(userInput), 2); 
            // check for adequate funds
            if (sodaCost > Math.Round(customer.wallet.card.AvailableFunds, 2)) 
            {
                canCompleteTransaction = false;
            }
            else
            {
                canCompleteTransaction = true;
            }
            return canCompleteTransaction;
        }

        //Complete Card Transaction
        public void CompleteTransaction(Customer customer, double sodaCost, string userSelection)
        {
            Console.WriteLine("\nCard payment processing!");
            //Money to Soda Machine
            cardPaymentBalance += Math.Round(sodaCost, 2);
            Console.WriteLine($"\nThe soda machine now has a card payment balance of {cardPaymentBalance:C2}");
            //Money from card
            customer.wallet.card.AvailableFunds -= Math.Round(sodaCost, 2); 
            Console.WriteLine($"The customer's card now has {customer.wallet.card.AvailableFunds:C2} in available funds");
            // Sodamachine to backpack
            customer.backpack.cans.Add(DispenseSoda(userSelection)); 
            customer.DisplayContents(customer.backpack);
        }

        //cancel card transaction
        public void CancelTransaction() 
        {
            Console.WriteLine("Cannot complete transaction - inadqeuate funds.");
        }

        // COIN TRANSACTIONS //

        //Process coin transaction
        public bool ProcessTransaction(string userInput)
        {
            bool canCompleteTransaction;
            //tally coins in hopper
            double moneyInHopper = Math.Round(Verification.CountMoney(hopperIn), 2); 
            //get soda cost
            double sodaCost = Math.Round(GetSodaCost(userInput), 2);
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
                //Calculate change for user
                double changeDue = Math.Round(moneyInHopper - sodaCost, 2);
                //Checks if exact change can be given to user
                bool canGiveExactChange = DispenseChange(changeDue);
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

        //Complete coin transaction 
        public void CompleteTransaction(Customer customer, string userSelection)
        {
            //soda machine takes coins from hopper
            foreach (Coin coin in hopperIn.ToList())
            {
                register.Add(coin);
                hopperIn.Remove(coin);
            }
            Console.WriteLine($"The soda machine register now contains {Verification.CountMoney(register),2:C2}");
            //Soda machine puts soda in backpack
            customer.backpack.cans.Add(DispenseSoda(userSelection));
            customer.DisplayContents(customer.backpack);
            //soda machine gives change
            foreach (Coin coin in hopperOut.ToList())
            {
                customer.wallet.coins.Add(coin);
                hopperOut.Remove(coin);
            }
            customer.DisplayContents(customer.wallet);
        }

        //Cancle coin transaction
        public void CancelTransaction(Customer customer)
        {
            //soda machine returns coins from hopper in
            foreach (Coin coin in hopperIn.ToList()) 
            {
                customer.wallet.coins.Add(coin);
                hopperIn.Remove(coin);
            }
            Console.WriteLine($"The soda machine register now contains {Verification.CountMoney(register),2:C2}");
            customer.DisplayContents(customer.backpack);
            customer.DisplayContents(customer.wallet);
        }


        // GET COST OF SODA
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

        // DISPENSE SODA
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

        //GIVE BACK CHANGE
        private bool DispenseChange(double changeDue)
        {
            // Check total change available, make sure change available is greater than change due
            bool canGiveExactChange = false;
            while (Math.Round(changeDue, 2) >= 0.25)
            {
                //Check for quarters
                bool coinExists = Verification.CheckIfObjectExists(register, "Quarter");
                if (coinExists)
                {
                    for (int i = 0; i < register.Count; i++)
                    {
                        //Loop through and remove quarter
                        if (register[i].name == "Quarter")
                        {
                            //Add remove coins to hopper
                            hopperOut.Add(register[i]);
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
                        //Iterate through and remove dime
                        if (register[i].name == "Dime")
                        {
                            // Add removed coins to hopper
                            hopperOut.Add(register[i]);
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
                        //Iterate throu and remove Nickel
                        if (register[i].name == "Nickel")
                        {
                            //Add remove coins to hopper
                            hopperOut.Add(register[i]);
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
                        // Iterate through and remove penny
                        if (register[i].name == "Penny")
                        {
                            //Add removed coins to hopper
                            hopperOut.Add(register[i]);
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