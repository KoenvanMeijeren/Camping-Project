using Visualization;
using NUnit.Framework;

namespace ModelTests
{
    [TestFixture]
    public class CampingPlaceTypeTest
    {
        [Test]
        public void TestCampingPlaceTypeConstructorCorrect()
        {
            Accommodation accommodation = new Accommodation("12", "BH", "test");
            CampingPlaceType campingPlaceType = new CampingPlaceType("122", "10", "100", accommodation);
            Assert.AreEqual(campingPlaceType.Id, 122);
            Assert.AreEqual(campingPlaceType.Accommodation.Id, 12);
            Assert.AreEqual(campingPlaceType.GuestLimit, 10);
            Assert.AreEqual(campingPlaceType.StandardNightPrice, 100);
        }

        [Test]
        public void TestCampingPlaceTypeConstructorIncorrect()
        {
            CampingPlaceType campingPlaceType = new CampingPlaceType(null, null, null, null);
            Assert.AreEqual(campingPlaceType.Id, -1);
            Assert.AreEqual(campingPlaceType.GuestLimit, 0);
            Assert.AreEqual(campingPlaceType.StandardNightPrice, 0);
            Assert.AreEqual(campingPlaceType.Accommodation, null);
        }
    }
}