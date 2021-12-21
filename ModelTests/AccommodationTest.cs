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
            Assert.AreEqual(accommodation.Type, AccommodationTypes.Unknown);
        }

        [Test]
        public void TestAccommodationLongConstructorCorrect()
        {
            Accommodation accommodation = new Accommodation("12", "testPrefix", "Bungalow");
            Assert.AreEqual(accommodation.Id, 12);
            Assert.AreEqual(accommodation.Prefix, "testPrefix");
            Assert.AreEqual(accommodation.Name, "Bungalow");
            Assert.AreEqual(accommodation.Type, AccommodationTypes.Bungalow);
        }

        [Test]
        public void TestAccommodationConstructorIncorrect()
        {
            Accommodation accommodation = new Accommodation(null, null, null);
            Assert.AreEqual(accommodation.Id, -1);
            Assert.AreEqual(accommodation.Prefix, null);
            Assert.AreEqual(accommodation.Name, null);
            Assert.AreEqual(accommodation.Type, AccommodationTypes.Unknown);
        }
    }
}