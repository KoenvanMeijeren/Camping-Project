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
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingOwner campingOwner = new CampingOwner("1", "testName");
            Camping camping = new Camping("1", "testName", address, campingOwner);
            Assert.AreEqual(camping.Name, "testName");
            Assert.AreEqual(camping.Address, address);
            Assert.AreEqual(camping.CampingOwner, campingOwner);
        }

        [Test]
        public void TestCampingConstructorIncorrect()
        {
            Camping camping = new Camping(null, null, null);
            Assert.AreEqual(camping.Id, -1);
            Assert.AreEqual(camping.Name, null);
            Assert.AreEqual(camping.CampingOwner, null);
        }
    }
}
