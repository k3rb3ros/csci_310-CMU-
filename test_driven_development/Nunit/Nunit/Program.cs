using System;
using NUnit.Framework;

namespace Bank
{
    public class Account
    {
        private decimal balance;
        private decimal minimumBalance = 10m;

        public void Deposit(decimal amount)
        {
            balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            balance -= amount;
        }

        public void TransferFunds(Account destination, decimal amount)
        {
            if (balance - amount < minimumBalance)
                throw new InsufficientFundsException();

            destination.Deposit(amount);

            Withdraw(amount);
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public decimal MinimumBalance
        {
            get { return minimumBalance; }
        }
    }

    public class InsufficientFundsException : ApplicationException
    {
    }

    [TestFixture]
    public class AccountTest
    {
        Account source;
        Account destination;

        [SetUp]
        public void Init()
        {
            source = new Account();
            source.Deposit(200m);

            destination = new Account();
            destination.Deposit(150m);
        }

        [Test]
        public void TransferFunds()
        {
            source.TransferFunds(destination, 100m);

            Assert.AreEqual(250m, destination.Balance);
            Assert.AreEqual(100m, source.Balance);
        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void TransferWithInsufficientFunds()
        {
            source.TransferFunds(destination, 300m);
        }

        [Test]
        [Ignore("Decide how to implement transaction management")]
        public void TransferWithInsufficientFundsAtomicity()
        {
            try
            {
                source.TransferFunds(destination, 300m); 
            }
            catch (InsufficientFundsException expected)
            {
            }

            Assert.AreEqual(200m, source.Balance);
            Assert.AreEqual(150m, destination.Balance);
        }
    }
}
//##############
//# test cases #
//##############
/*
 * General *
 * Sales clerk can access inventory
 * Sales clerk can scan items
 * Sales clerk can modify price, if incorrect price comes up
 * When items are scanned they come up with an accurate price
 * Inventory manager can access current inventory
 * Inventory manager can order new items
 * Customer can buy item with cash
 * Customer can buy item with Credit
 * Customer can buy item with Debt
 * Customer can buy item with Check
 * Can scan multiple items
 * Specific *
 * Can buy orange juice with $10
 * Correct change is calculated for orange juice when paid with $10
 * Can't buy orange juice with insufficient funds
 * Can buy candy and and a soda for $5
 * Inventory manager can order a pallete of tvs
 * Inventory manager can get number of food items in stock
 * 
 */

class _main
{
    public static void Main(string[] args)
    {}
}