using NUnit.Framework;
using Model;
using System;

namespace ModelTests
{
    [TestFixture]
    public class CampingCustomerTest
    {

        [Test]
        public void TestCampingCustomerConstructorCorrect()
        {
            Account account = new Account("admin", "nimda", "0");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer(account, address, "19/10/21", "testPhoneNumber", "testFirstName", "testLastName");
            Assert.AreEqual(campingCustomer.Id, -1);
            Assert.AreEqual(campingCustomer.Address, address);
            Assert.AreEqual(campingCustomer.Birthdate, DateTime.Parse("19/10/21"));
            Assert.AreEqual(campingCustomer.PhoneNumber, "testPhoneNumber");
            Assert.AreEqual(campingCustomer.FirstName, "testFirstName");
            Assert.AreEqual(campingCustomer.LastName, "testLastName");
            Assert.AreEqual(campingCustomer.FullName, "testFirstName testLastName");
        }
        
        [Test]
        public void TestCampingCustomerLongConstructorCorrect()
        {
            Account account = new Account("1", "admin", "nimda", "0");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/21", "testPhoneNumber", "testFirstName", "testLastName");
            Assert.AreEqual(campingCustomer.Id, 1);
            Assert.AreEqual(campingCustomer.Address, address);
            Assert.AreEqual(campingCustomer.Birthdate, DateTime.Parse("19/10/21"));
            Assert.AreEqual(campingCustomer.PhoneNumber, "testPhoneNumber");
            Assert.AreEqual(campingCustomer.FirstName, "testFirstName");
            Assert.AreEqual(campingCustomer.LastName, "testLastName");
            Assert.AreEqual(campingCustomer.FullName, "testFirstName testLastName");
        }

        [Test]
        public void TestCampingCustomerConstructorIncorrect()
        {
            CampingCustomer campingCustomer = new CampingCustomer(null, null, null, null, null, null, null);
            Assert.AreEqual(campingCustomer.Id, -1);
            Assert.AreEqual(campingCustomer.Address, null);
            Assert.AreEqual(campingCustomer.Birthdate, DateTime.MinValue);
            Assert.AreEqual(campingCustomer.PhoneNumber, null);
            Assert.AreEqual(campingCustomer.FirstName, null);
            Assert.AreEqual(campingCustomer.LastName, null);
            Assert.AreEqual(campingCustomer.FullName, null);
        }
    }
}
