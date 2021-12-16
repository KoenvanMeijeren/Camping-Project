using NUnit.Framework;
using Model;

namespace ModelTests
{
    [TestFixture]
    public class AccommodationTest
    {

        [Test]
        public void TestAccommodationConstructorCorrect()
        {
            Accommodation accommodation = new Accommodation("testPrefix", "testName");
            Assert.AreEqual(accommodation.Id, -1);
            Assert.AreEqual(accommodation.Prefix, "testPrefix");
            Assert.AreEqual(accommodation.Name, "testName");
        }

        [Test]
        public void TestAccommodationLongConstructorCorrect()
        {
            Accommodation accommodation = new Accommodation("12", "testPrefix", "testName");
            Assert.AreEqual(accommodation.Id, 12);
            Assert.AreEqual(accommodation.Prefix, "testPrefix");
            Assert.AreEqual(accommodation.Name, "testName");
        }

        [Test]
        public void TestAccommodationConstructorIncorrect()
        {
            Accommodation accommodation = new Accommodation(null, null, null);
            Assert.AreEqual(accommodation.Id, -1);
            Assert.AreEqual(accommodation.Prefix, null);
            Assert.AreEqual(accommodation.Name, null);
        }
    }
}