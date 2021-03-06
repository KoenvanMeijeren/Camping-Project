using NUnit.Framework;
using Model;

namespace ModelTests
{
    [TestFixture]
    public class CampingPlaceTest
    {
        [Test]
        public void TestCampingPlaceConstructorCorrect()
        {
            Accommodation accommodation = new Accommodation( "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", campingPlaceType);
            Assert.AreEqual(campingPlace.Id, -1);
            Assert.AreEqual(campingPlace.Number, 1.0);
            Assert.AreEqual(campingPlace.Surface, 1.0);
            Assert.AreEqual(campingPlace.TotalPrice, 2.0);
            Assert.AreEqual(campingPlace.Type, campingPlaceType);
            Assert.AreEqual(campingPlace.Location, "testPrefix-1");
        }
        
        [Test]
        public void TestCampingPlaceLongConstructorCorrect()
        {
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("11", "1", "1", "1", campingPlaceType);
            Assert.AreEqual(campingPlace.Id, 11);
            Assert.AreEqual(campingPlace.Number, 1.0);
            Assert.AreEqual(campingPlace.Surface, 1.0);
            Assert.AreEqual(campingPlace.SurfaceReadable, "1 m3");
            Assert.AreEqual(campingPlace.TotalPrice, 2.0);
            Assert.AreEqual(campingPlace.TotalPriceReadable, "€ 2");
            Assert.AreEqual(campingPlace.Type, campingPlaceType);
            Assert.AreEqual(campingPlace.Location, "testPrefix-1");
            Assert.AreEqual(campingPlace.ToString(), "testPrefix-1");
        }

        [Test]
        public void TestCampingPlaceConstructorIncorrect()
        {
            CampingPlace campingPlace = new CampingPlace(null, null, null, null, null);
            Assert.AreEqual(campingPlace.Id, -1);
            Assert.AreEqual(campingPlace.Number, 0);
            Assert.AreEqual(campingPlace.Surface, 0);
            Assert.AreEqual(campingPlace.TotalPrice, 0);
            Assert.AreEqual(campingPlace.Type, null);
            Assert.AreEqual(campingPlace.GetLocation(), "0");
        }
    }
}
