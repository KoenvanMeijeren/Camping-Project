﻿using NUnit.Framework;
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
            Account account = new Account("1", "admin", "nimda", 1);
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/21", "testEmail", "testPhoneNumber", "testFirstName", "testLastName");
            Assert.AreEqual(campingCustomer.Id, 1);
            Assert.AreEqual(campingCustomer.Address, address);
            Assert.AreEqual(campingCustomer.Birthdate, DateTime.Parse("19/10/21"));
            Assert.AreEqual(campingCustomer.Email, "testEmail");
            Assert.AreEqual(campingCustomer.PhoneNumber, "testPhoneNumber");
            Assert.AreEqual(campingCustomer.FirstName, "testFirstName");
            Assert.AreEqual(campingCustomer.LastName, "testLastName");
        }


        [Test]
        public void TestCampingCustomerConstructorIncorrect()
        {
            CampingCustomer campingCustomer = new CampingCustomer(null, null, null, null, null, null, null, null);
            Assert.AreEqual(campingCustomer.Id, -1);
            Assert.AreEqual(campingCustomer.Address, null);
            Assert.AreEqual(campingCustomer.Birthdate, DateTime.MinValue);
            Assert.AreEqual(campingCustomer.Email, null);
            Assert.AreEqual(campingCustomer.PhoneNumber, null);
            Assert.AreEqual(campingCustomer.FirstName, null);
            Assert.AreEqual(campingCustomer.LastName, null);
        }
    }
}
