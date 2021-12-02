using NUnit.Framework;
using Model;

namespace ModelTests
{
    [TestFixture]
    public class CampingOwnerTest
    {
        [Test]
        public void TestCampingOwnerConstructorCorrect()
        {
            CampingOwner campingOwner = new CampingOwner("1", "testName");
            Assert.AreEqual(campingOwner.Id, 1);
            Assert.AreEqual(campingOwner.Name, "testName");
        }

        [Test]
        public void TestCampingOwnerConstructorIncorrect()
        {
            CampingOwner campingOwner = new CampingOwner(null);
            Assert.AreEqual(campingOwner.Id, -1);
            Assert.AreEqual(campingOwner.Name, null);
        }
    }
}
