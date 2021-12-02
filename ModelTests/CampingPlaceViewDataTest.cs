using NUnit.Framework;
using Model;

namespace ModelTests
{
    [TestFixture]
    public class CampingPlaceViewDataTest
    {
        [Test]
        public void TestCampingCampingPlaceViewDataConstructor()
        {
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", "1", campingPlaceType);
            CampingPlaceViewData campingPlaceViewData = new CampingPlaceViewData(campingPlace);
            Assert.AreEqual(campingPlaceViewData.Type, campingPlace.Type.Accommodation.Name);
            Assert.AreEqual(campingPlaceViewData.Locatie, campingPlace.Type.Accommodation.Prefix + " " + campingPlace.Number);
            Assert.AreEqual(campingPlaceViewData.Personen, campingPlace.Type.GuestLimit);
            Assert.AreEqual(campingPlaceViewData.Oppervlakte, campingPlace.Surface + " m2");
            Assert.AreEqual(campingPlaceViewData.Dagtarief, "€" + (campingPlace.Type.StandardNightPrice + campingPlace.ExtraNightPrice));
        }

        [Test]
        public void TestCampingPlaceViewDataGetId()
        {
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", "1", campingPlaceType);
            CampingPlaceViewData campingPlaceViewData = new CampingPlaceViewData(campingPlace);

            Assert.AreEqual(campingPlaceViewData.GetId(), campingPlace.Id);
        }

        [Test]
        public void TestCampingPlaceViewGetGetNumericNightPrice()
        {
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", "1", campingPlaceType);
            CampingPlaceViewData campingPlaceViewData = new CampingPlaceViewData(campingPlace);

            Assert.AreEqual(campingPlaceViewData.GetNumericNightPrice(), campingPlace.Type.StandardNightPrice + campingPlace.ExtraNightPrice);
        }
    }
}
