using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ViewModel;
using Model;
using Moq;

namespace ViewModelTests
{
    public class CampingPlacesCollectionViewModelTest
    {
        private Mock<CampingPlacesCollectionViewModel> CampingPlacesMock;
        private List<CampingPlace> campingPlaceList;
        private DateTime CheckinDate;
        private DateTime CheckoutDate;


        [SetUp]
        public void Setup()
        {
            CampingPlacesMock = new Mock<CampingPlacesCollectionViewModel>();
            CheckinDate = DateTime.Today;
            CheckoutDate = DateTime.Today.AddDays(2);

            campingPlaceList = new List<CampingPlace>();
            campingPlaceList.Add(new CampingPlace("1", "80", "0", new CampingPlaceType("5", "40", new Accommodation("CA", "Caravan"))));
            campingPlaceList.Add(new CampingPlace("1", "80", "30", new CampingPlaceType("2", "80", new Accommodation("CH", "Chalet"))));
            campingPlaceList.Add(new CampingPlace("1", "80", "30", new CampingPlaceType("10", "80", new Accommodation("CH", "Chalet"))));
            campingPlaceList.Add(new CampingPlace("1", "80", "10", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper"))));
            campingPlaceList.Add(new CampingPlace("1", "80", "10", new CampingPlaceType("1", "50", new Accommodation("CA", "Camper"))));
            campingPlaceList.Add(new CampingPlace("1", "80", "10", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper"))));
            campingPlaceList.Add(new CampingPlace("1", "80", "0", new CampingPlaceType("2", "10", new Accommodation("TE", "Tent"))));

            //IEnumerable<CampingPlace> campingPlaceNumerable = campingPlaceList;
            CampingPlacesMock.Setup(x => x.GetCampingPlaces()).Returns(campingPlaceList);
            CampingPlacesMock.Object.SelectedPlaceType = "Alle";
            CampingPlacesMock.Object.MinNightPrice = "0";
            CampingPlacesMock.Object.MaxNightPrice = "2000";
            CampingPlacesMock.Object.CheckInDate = DateTime.Today;
            CampingPlacesMock.Object.CheckOutDate = DateTime.Today.AddDays(1);
            CampingPlacesMock.Setup(x => x.ToFilteredOnReservedCampingPlaces(campingPlaceList, CheckinDate, CheckoutDate)).Returns(campingPlaceList);
        }

        [Test]
        public void TestInitializeViewModel()
        {
            var mockListFilterOnDate = CampingPlacesMock.Object.ToFilteredOnReservedCampingPlaces(campingPlaceList, CheckinDate,CheckoutDate);
            var mockListGetCampingPlaces = CampingPlacesMock.Object.GetCampingPlaces();
            string expectedSelectedPlaceType = "Alle";


            Assert.AreEqual(CampingPlacesMock.Object.SelectedPlaceType, expectedSelectedPlaceType);
            Assert.IsTrue(CampingPlacesMock.Object.CampingPlaceTypes.Count() > 0);
            Assert.IsTrue(mockListFilterOnDate.Count() > 0);
            Assert.IsTrue(mockListGetCampingPlaces.Count() > 0);
            Assert.AreEqual(CampingPlacesMock.Object.CheckInDate, DateTime.Today);
            Assert.AreEqual(CampingPlacesMock.Object.CheckOutDate, DateTime.Today.AddDays(1));
            Assert.IsTrue(CampingPlacesMock.Object.CampingPlaces.Count() > 0);
            
        }

        [Test]
        public void TestFilterCampingPlaceType()
        {
            CampingPlacesMock.Object.SelectedPlaceType = "Caravan";
            Assert.IsTrue(CampingPlacesMock.Object.CampingPlaces.Count() == 1);

            CampingPlacesMock.Object.SelectedPlaceType = "Chalet";
            Assert.IsTrue(CampingPlacesMock.Object.CampingPlaces.Count() == 2);

            CampingPlacesMock.Object.SelectedPlaceType = "Camper";
            Assert.IsTrue(CampingPlacesMock.Object.CampingPlaces.Count() == 3);

            CampingPlacesMock.Object.SelectedPlaceType = "Tent";
            Assert.IsTrue(CampingPlacesMock.Object.CampingPlaces.Count() == 1);
        }

        [Test]
        public void TestFilterMinPrice()
        {
            Assert.IsTrue(CampingPlacesMock.Object.CampingPlaces.Count() == 7);

            CampingPlacesMock.Object.MinNightPrice = "50";

            //Assert.IsTrue(CampingPlacesMock.Object.CampingPlaces.Count() ==7);
        }
    }
}
