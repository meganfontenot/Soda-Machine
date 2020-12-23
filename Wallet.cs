using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda_Machine
{
    class Wallet
    {
        // member variables
        public List<Coin> coins;
        public Card card;

        // constructor
        public Wallet()
        {
            coins = new List<Coin>();
            Quarter quarter = new Quarter();
            Dime dime = new Dime();
            Nickel nickel = new Nickel();
            Penny penny = new Penny();
            AddMoney(quarter, 12); // $3 in quarters
            AddMoney(dime, 10); // $1 in dimes
            AddMoney(nickel, 15); // $.75 in nickels;
            AddMoney(penny, 25); // $.25 in pennies
            card = new Card();
        }

        // member methods (CAN DO)
        private void AddMoney(Coin coin, int number)
        {
            for (int i = 0; i < number; i++)
            {
                coins.Add(coin);
            }
        }
    }
}