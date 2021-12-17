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
            Camping camping = new Camping("testName", address, campingOwner, "0341234565", "admin@nimda.com");
            Assert.AreEqual(camping.Id, -1);
            Assert.AreEqual(camping.Name, "testName");
            Assert.AreEqual(camping.Address, address);
            Assert.AreEqual(camping.CampingOwner, campingOwner);
            Assert.AreEqual(camping.PhoneNumber, "0341234565");
            Assert.AreEqual(camping.Email, "admin@nimda.com");
        }
        
        [Test]
        public void TestCampingLongConstructorCorrect()
        {
            Account account = new Account("1", "admin", "nimda",  "1");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingOwner campingOwner = new CampingOwner("1", account, "test", "testName");
            Camping camping = new Camping("123", "testName", address, campingOwner, "0341234565", "admin@nimda.com");
            Assert.AreEqual(camping.Id, 123);
            Assert.AreEqual(camping.Name, "testName");
            Assert.AreEqual(camping.Address, address);
            Assert.AreEqual(camping.CampingOwner, campingOwner);
            Assert.AreEqual(camping.PhoneNumber, "0341234565");
            Assert.AreEqual(camping.Email, "admin@nimda.com");
        }

        [Test]
        public void TestCampingConstructorIncorrect()
        {
            Camping camping = new Camping(null, null, null, null, null, null);
            Assert.AreEqual(camping.Id, -1);
            Assert.AreEqual(camping.Name, null);
            Assert.AreEqual(camping.CampingOwner, null);
            Assert.AreEqual(camping.PhoneNumber, null);
            Assert.AreEqual(camping.Email, null);
        }
    }
}
