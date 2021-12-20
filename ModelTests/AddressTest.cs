using NUnit.Framework;
using Visualization;

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
            Assert.AreEqual(address.Street, "testAddress");
            Assert.AreEqual(address.PostalCode, "testPostalCode");
            Assert.AreEqual(address.Place, "testPlace");
        }


        [Test]
        public void TestAddressConstructorIncorrect()
        {
            Address address = new Address(null, null, null);
            Assert.AreEqual(address.Id, -1);
            Assert.AreEqual(address.Street, null);
            Assert.AreEqual(address.PostalCode, null);
            Assert.AreEqual(address.Place, null);
        }
    }
}
