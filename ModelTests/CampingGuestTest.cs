using NUnit.Framework;
using Model;
using System;

namespace ModelTests
{
    [TestFixture]
    public class CampingGuestTest
    {
        [Test]
        public void TestCampingGuestConstructorCorrect()
        {
            CampingGuest campingGuest = new CampingGuest("1", "test", "testName", "19/10/21");
            Assert.AreEqual(campingGuest.Id, 1);
            Assert.AreEqual(campingGuest.FirstName, "test");
            Assert.AreEqual(campingGuest.LastName, "testName");
            Assert.AreEqual(campingGuest.Birthdate, DateTime.Parse("19/10/21"));
        }

        [Test]
        public void TestCampingGuestConstructorIncorrect()
        {
            CampingGuest campingGuest = new CampingGuest(null, null, null, "19-10-21");
            Assert.AreEqual(campingGuest.Id, -1);
            Assert.AreEqual(campingGuest.FirstName, null);
            Assert.AreEqual(campingGuest.LastName, null);
            Assert.AreEqual(campingGuest.Birthdate, DateTime.Parse("19/10/21"));
        }
    }
}
