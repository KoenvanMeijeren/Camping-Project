using NUnit.Framework;
using Model;

namespace ModelTests
{
    [TestFixture]
    public class CampingPlaceTest
    {
        [Test]
        public void TestCampingConstructorCorrect()
        {
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", "1", campingPlaceType);
            Assert.AreEqual(campingPlace.Id, 1);
            Assert.AreEqual(campingPlace.Number, 1.0);
            Assert.AreEqual(campingPlace.Surface, 1.0);
            Assert.AreEqual(campingPlace.TotalPrice, 2.0);
            Assert.AreEqual(campingPlace.Type, campingPlaceType);
        }

        [Test]
        public void TestCampingConstructorIncorrect()
        {
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", campingPlaceType);
            Assert.AreEqual(campingPlace.Id, -1);
            Assert.AreEqual(campingPlace.Number, 1.0);
            Assert.AreEqual(campingPlace.Surface, 1.0);
            Assert.AreEqual(campingPlace.TotalPrice, 2.0);
            Assert.AreEqual(campingPlace.Type, campingPlaceType);
        }
    }
}
