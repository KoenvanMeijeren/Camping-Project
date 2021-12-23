using NUnit.Framework;
using Model;

namespace ModelTests
{
    [TestFixture]
    public class CampingTest
    {
        [Test]
        public void TestCampingConstructorCorrect()
        {
            Account account = new Account("1", "admin", "nimda",  "1");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingOwner campingOwner = new CampingOwner(account, "test", "testName");
            Camping camping = new Camping("testName", address, campingOwner, "0341234565", "admin@nimda.com", "https://www.facebook.com", "https://www.twitter.com", "https://www.instagram.com", "#000000");
            Assert.AreEqual(camping.Id, -1);
            Assert.AreEqual(camping.Name, "testName");
            Assert.AreEqual(camping.Address, address);
            Assert.AreEqual(camping.CampingOwner, campingOwner);
            Assert.AreEqual(camping.PhoneNumber, "0341234565");
            Assert.AreEqual(camping.Email, "admin@nimda.com");
            Assert.AreEqual(camping.Facebook, "https://www.facebook.com");
            Assert.AreEqual(camping.Twitter, "https://www.twitter.com");
            Assert.AreEqual(camping.Instagram, "https://www.instagram.com");
            Assert.AreEqual(camping.Color, "#000000");
        }
        
        [Test]
        public void TestCampingLongConstructorCorrect()
        {
            Account account = new Account("1", "admin", "nimda",  "1");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingOwner campingOwner = new CampingOwner("1", account, "test", "testName");
            Camping camping = new Camping("123", "testName", address, campingOwner, "0341234565", "admin@nimda.com", "https://www.facebook.com", "https://www.twitter.com", "https://www.instagram.com", "#000000");
            Assert.AreEqual(camping.Id, 123);
            Assert.AreEqual(camping.Name, "testName");
            Assert.AreEqual(camping.Address, address);
            Assert.AreEqual(camping.CampingOwner, campingOwner);
            Assert.AreEqual(camping.PhoneNumber, "0341234565");
            Assert.AreEqual(camping.Email, "admin@nimda.com");
            Assert.AreEqual(camping.Facebook, "https://www.facebook.com");
            Assert.AreEqual(camping.Twitter, "https://www.twitter.com");
            Assert.AreEqual(camping.Instagram, "https://www.instagram.com");
            Assert.AreEqual(camping.Color, "#000000");
        }

        [Test]
        public void TestCampingConstructorIncorrect()
        {
            Camping camping = new Camping(null, null, null, null, null, null, null, null, null, null);
            Assert.AreEqual(camping.Id, -1);
            Assert.AreEqual(camping.Name, null);
            Assert.AreEqual(camping.CampingOwner, null);
            Assert.AreEqual(camping.PhoneNumber, null);
            Assert.AreEqual(camping.Email, null);
            Assert.AreEqual(camping.Facebook, null);
            Assert.AreEqual(camping.Twitter, null);
            Assert.AreEqual(camping.Instagram, null);
            Assert.AreEqual(camping.Color, null);
        }
    }
}
