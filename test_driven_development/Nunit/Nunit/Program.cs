using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace POS
{
    class Store
    {
        XDocument xdoc = XDocument.Load("Store.xml");
        public Item find(string name)
        {
            var queryResult = (from item in xdoc.Descendants("store").Elements("item")
                               where item.Element("name").Value == name
                               select item).Take(1);
            foreach (var item in queryResult.Take(1))
            {
                return new Item(item);
            }
            return null;
        }
    }
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
        public int quantity;
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

        public Item(XElement xmlItem)
        {
            name = xmlItem.Element("name").Value;
            price = decimal.Parse(xmlItem.Element("price").Value);
            quantity = int.Parse(xmlItem.Element("quantity").Value);
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
        

        [TestFixture]
        class TestStore
        {
            Store store;
            [SetUp]
            public void Init()
            {
                store = new Store();
            }
            [Test]
            public void CheckStoreExists()
            {
                Store store = new Store();
            }

            [Test]
            public void CheckStoreHasTide()
            {
                Assert.NotNull(store.find("Tide"));
            }

            [Test]
            public void CheckStoreHasNoApple()
            {
                Assert.IsNull(store.find("apple"));
            }
        }
    class MainClass
    {
        public static void Main(string[] args)
        {
            
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
