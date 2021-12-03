﻿using NUnit.Framework;
using Model;

namespace ModelTests
{
    [TestFixture]
    public class CampingOwnerTest
    {
        [Test]
        public void TestCampingOwnerConstructorCorrect()
        {
            Account account = new Account("1", "admin", "nimda", 1);
            CampingOwner campingOwner = new CampingOwner("1", account, "testName", "last");
            Assert.AreEqual(campingOwner.Id, 1);
            Assert.AreEqual(campingOwner.FirstName, "testName");
            Assert.AreEqual(campingOwner.LastName, "last");
        }

        [Test]
        public void TestCampingOwnerConstructorIncorrect()
        {
            CampingOwner campingOwner = new CampingOwner(null, null, null);
            Assert.AreEqual(campingOwner.Id, -1);
            Assert.AreEqual(campingOwner.FirstName, null);
            Assert.AreEqual(campingOwner.LastName, null);
        }
    }
}
