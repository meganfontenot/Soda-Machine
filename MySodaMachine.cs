using Soda_Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace MySodaMachine
{
    class SodaMachine
    {
        // member variables (HAS A)
        public List<Coin> register;
        public List<Coin> hopperIn;
        public List<Coin> hopperOut;
        public List<Can> inventory;

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
            UpdateInventory(quarter, 20);
            UpdateInventory(dime, 10);
            UpdateInventory(nickel, 20);
            UpdateInventory(penny, 50);

            inventory = new List<Can>();
            Cola cola = new Cola();
            OrangeSoda orangeSoda = new OrangeSoda();
            RootBeer rootBeer = new RootBeer();
            UpdateInventory(cola, 20);
            UpdateInventory(orangeSoda, 20);
            UpdateInventory(rootBeer, 20);
        }

        // member methods (CAN DO)

        //Need to update current inventory for both coins and cans
        private void UpdateInventory(Coin coin, int numberOfCoins)
        {
            for (int i = 0; i < numberOfCoins; i++)
            {
                register.Add(coin);
            }
        }

        private void UpdateInventory(Can can, int numberOfCans)
        {
            for (int i = 0; i < numberOfCans; i++)
            {
                inventory.Add(can);
            }
        }

        //Need to display that inventory

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
                Console.WriteLine("Type 1 for Cola");
            }
            else
            {
                Console.WriteLine("Cola (sold out)");
            }

            if (orangeSodaCansAvailable > 0)
            {
                Console.WriteLine("Type 2 for Orange Soda");
            }
            else
            {
                Console.WriteLine("Orange Soda (sold out)");
            }

            if (rootBeerCansAvailable > 0)
            {
                Console.WriteLine("Type 3 for Root Beer");
            }
            else
            {
                Console.WriteLine("Root Beer (sold out)");
            }
        }

        //Need a transaction
        public void CompleteTransaction(string userInput, Customer customer)
        {
            // Select soda based on user selection
            // Collect name and cost info to display during transaction
            string[] userSelection = SelectSoda(userInput);

            string sodaName = userSelection[0];
            double sodaCost = Math.Round(double.Parse(userSelection[1]), 2);
            // Loop - while customer has not yet entered enough change
            while (Math.Round(sodaCost, 2) > 0)
            {
                Console.Clear();
                // Display selection/remaining costs
                Console.WriteLine($"{sodaName}: ${Math.Round(sodaCost, 2)} (amount remaining)");
                
                // Display current total and coin count
                customer.DisplayContents(customer.wallet);
                
                // Customer inputs coin to deposit
                Coin insertedCoin = customer.InsertCoin();
                
                // Coin removed from customer wallet then stored in soda machine hopper
                hopperIn.Add(insertedCoin);
                
                // Subtract coin value from cost of selection made
                sodaCost -= Math.Round(insertedCoin.Value, 2);
            }
            // When enough money has been entered compare the cost to the amount entered
            if (Math.Round(sodaCost, 2) == 0) // If no change due
            {
                // Deposit coins from hopper to machine
                foreach (Coin coin in hopperIn.ToList()) //  throw exception?
                {
                    Console.WriteLine("Transaction complete, no change due!");
                    register.Add(coin);
                    hopperIn.Remove(coin);
                }
                
                // Remove can from machine inventory and add can to customer backpack
                customer.backpack.cans.Add(DispenseSoda(sodaName));
            }
            else if (Math.Round(sodaCost, 2) < 0) // If change is due, run check change method
            {
                double changeDue = Math.Round(Math.Abs(sodaCost), 2); // Pass absolute value of sodaCost (how much change is due)
                // Checks if exact change can be given
                bool canCompletePurchase = DispenseChange(changeDue); 
                if (canCompletePurchase == true)
                {
                    Console.WriteLine("Transaction complete, change due!");
                    //hopperIn coins to the machine
                    foreach (Coin coin in hopperIn.ToList()) 
                    {
                        register.Add(coin);
                        hopperIn.Remove(coin);
                    }
                    // Remove can from machine inventory, add Can to customer backpack
                    customer.backpack.cans.Add(DispenseSoda(sodaName)); 
                    
                    // Dispense hopperOut change to customer
                    foreach (Coin coin in hopperOut.ToList()) 
                    {
                        customer.wallet.coins.Add(coin);
                        hopperOut.Remove(coin);
                    }
                }
                else
                {
                    Console.WriteLine("Transaction cannot be completed - exact change unavailable.");
                    // Return hopperIn coins tothe customer
                    foreach (Coin coin in hopperIn.ToList()) 
                    {
                        customer.wallet.coins.Add(coin);
                        hopperIn.Remove(coin);
                    }
                }

            }
        }

        public string[] SelectSoda(string userInput)
        {
            string[] userSelection = new string[2];
            switch (userInput)
            {
                case "1":
                    userSelection[0] = "Cola";
                    userSelection[1] = "0.35";
                    break;
                case "2":
                    userSelection[0] = "Orange Soda";
                    userSelection[1] = "0.06";
                    break;
                case "3":
                    userSelection[0] = "Root Beer";
                    userSelection[1] = "0.60";
                    break;
            }
            return userSelection;
        }

        public Can DispenseSoda(string sodaName)
        {
            Can dispensedSoda = null;
            switch (sodaName)
            {
                case "Cola":
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
                case "Orange Soda":
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
                case "Root Beer":
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

        public bool DispenseChange(double changeDue)
        {
            // Calculate change available and make sure change available is greater than change due
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
                            // Add removed coins to hopper
                            // Remove coin from register
                            // Decrease change due
                            hopperOut.Add(register[i]); 
                            register.RemoveAt(i); 
                            changeDue = Math.Round(changeDue - 0.25, 2); 
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
                            // Add removed coins to hopper
                            // Remove coin from register
                            // Decrease change due
                            hopperOut.Add(register[i]); 
                            register.RemoveAt(i); 
                            changeDue = Math.Round(changeDue - 0.10, 2); 
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
                            // Add removed coins to hopper
                            // Remove coin from register
                            // Decrease change due
                            hopperOut.Add(register[i]);
                            register.RemoveAt(i); 
                            changeDue = Math.Round(changeDue - 0.05, 2); 
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
                            // Add removed coins to hopper
                            // Remove coin from register
                            // Decrease change due
                            hopperOut.Add(register[i]); 
                            register.RemoveAt(i); 
                            changeDue = Math.Round(changeDue - 0.01, 2); 
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