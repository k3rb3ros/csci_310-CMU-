using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace POS
{
    class Customer
    {
        public Cart cart = new Cart();
        public decimal cash { get; set; }

        public void addCart(Item item)
        {
            cart.Add(item);
        }

        public void checkOut(Cashier cashier)
        {
        }
    }

    class Cashier
    {
        public bool approve(Transaction transaction)
        {
            return true;
        }
    }

    class Item
    {
        string m_name;
        public string name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public Item(string _name)
        {
            name = _name;
        }

        decimal m_price;
        public decimal price
        {
            get
            {
                return m_price;
            }
            set
            {
                m_price = value;
            }
        }
    }

    class Transaction : List<Cart>
    {
    }

    class Cart : List<Item>
    {
        public decimal price
        {
            get
            {
                return this.Sum(item => item.price);
            }
        }
    }

    [TestFixture]
    class TestClass
    {
        Customer customer;
        Cashier cashier;
        Item item;
        Transaction transaction;

        [SetUp]
        public void Init()
        {
            customer = new Customer();
            cashier = new Cashier();
            item = new Item("Jiffy Mix");
            transaction = new Transaction();

            customer.cash = 20m;
        }

        [Test]
        public void BuyItemTest()
        {
            decimal cashTrack = customer.cash;
            customer.addCart(item);
            CollectionAssert.AllItemsAreNotNull(customer.cart);
            customer.checkOut(cashier);
            Assert.AreNotEqual(cashTrack, customer.cash);
        }


        [Test]
        public void ApproveTransactionTest()
        {
            bool approved;
            CollectionAssert.AllItemsAreInstancesOfType(transaction, typeof(Cart));
            approved = cashier.approve(transaction);
            Assert.IsTrue(approved);
        }

        [Test]
        public void CheckTransaction()
        {
            decimal total = 0;
            CollectionAssert.AllItemsAreInstancesOfType(transaction, typeof(Cart));
            foreach (Cart cart in transaction)
            {
                CollectionAssert.AllItemsAreInstancesOfType(cart, typeof(Item));
                foreach (Item item in cart)
                    total += cart.price;
            }

            Assert.Greater(customer.cash, total);
        }

    }
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.Write("Running...");
            Console.ReadLine();
        }
    }
}