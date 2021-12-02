using NUnit.Framework;
using Model;

namespace ModelTests
{
    [TestFixture]
    public class AddressTest
    {

        [Test]
        public void TestAddressConstructorCorrect()
        {
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            Assert.AreEqual(address.Id, 1);
            Assert.AreEqual(address.address, "testAddress");
            Assert.AreEqual(address.postalCode, "testPostalCode");
            Assert.AreEqual(address.place, "testPlace");
        }


        [Test]
        public void TestAddressConstructorIncorrect()
        {
            Address address = new Address(null, null, null);
            Assert.AreEqual(address.Id, -1);
            Assert.AreEqual(address.address, null);
            Assert.AreEqual(address.postalCode, null);
            Assert.AreEqual(address.place, null);
        }
    }
}
