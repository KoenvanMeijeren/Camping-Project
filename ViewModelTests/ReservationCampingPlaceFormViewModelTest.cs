using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ViewModel;
using Model;
using Moq;

namespace ViewModelTests
{
    public class ReservationCampingPlaceFormViewModelTest
    {
        private Mock<ReservationCampingPlaceFormViewModel> _campingPlacesMock;
        private List<CampingPlace> _campingPlaceList;
        
        private DateTime _checkInDate, _checkOutDate;
        private CampingPlace _testCampingPlaceOne, _testCampingPlaceTwo;


        [SetUp]
        public void Setup()
        {
            this._campingPlacesMock = new Mock<ReservationCampingPlaceFormViewModel>();
            this._checkInDate = DateTime.Today.AddDays(1);
            this._checkOutDate = DateTime.Today.AddDays(3);

            this._campingPlaceList = new List<CampingPlace>
            {
                new CampingPlace("3", "1", "80", "0", new CampingPlaceType("5", "40", new Accommodation("CA", "Caravan"))),
                new CampingPlace("4", "1", "80", "30", new CampingPlaceType("2", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("5", "1", "80", "30", new CampingPlaceType("10", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("6", "1", "80", "10", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("7", "1", "80", "10", new CampingPlaceType("1", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("8", "1", "80", "0", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper")))
            };

            this._testCampingPlaceOne = new CampingPlace("1", "1", "80", "0", new CampingPlaceType("2", "10", new Accommodation("TE", "Tent")));
            this._campingPlaceList.Add(this._testCampingPlaceOne);
            this._testCampingPlaceTwo = new CampingPlace("2","1", "80", "0", new CampingPlaceType("2", "10", new Accommodation("BG", "Bungalow")));
            this._campingPlaceList.Add(this._testCampingPlaceTwo);

            //IEnumerable<CampingPlace> campingPlaceNumerable = campingPlaceList;
            this._campingPlacesMock.Setup(x => x.GetCampingPlaces()).Returns(this._campingPlaceList);
            this._campingPlacesMock.Object.SelectedCampingPlaceType = ReservationCampingPlaceFormViewModel.SelectAll;
            this._campingPlacesMock.Object.MinNightPrice = "0";
            this._campingPlacesMock.Object.MaxNightPrice = "2000";
            this._campingPlacesMock.Object.CheckInDate = DateTime.Today;
            this._campingPlacesMock.Object.CheckOutDate = DateTime.Today.AddDays(1);
            this._campingPlacesMock.Setup(x => x.ToFilteredOnReservedCampingPlaces(this._campingPlaceList, this._checkInDate, this._checkOutDate)).Returns(this._campingPlaceList);
        }

        [Test]
        public void TestInitializeViewModel()
        {
            var mockListFilterOnDate = this._campingPlacesMock.Object.ToFilteredOnReservedCampingPlaces(this._campingPlaceList, this._checkInDate, this._checkOutDate);
            var mockListGetCampingPlaces = this._campingPlacesMock.Object.GetCampingPlaces();
            string expectedSelectedPlaceType = ReservationCampingPlaceFormViewModel.SelectAll;

            Assert.AreEqual(this._campingPlacesMock.Object.SelectedCampingPlaceType, expectedSelectedPlaceType);
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaceTypes.Any());
            Assert.IsTrue(mockListFilterOnDate.Any());
            Assert.IsTrue(mockListGetCampingPlaces.Any());
            Assert.AreEqual(this._campingPlacesMock.Object.CheckInDate, DateTime.Today);
            Assert.AreEqual(this._campingPlacesMock.Object.CheckOutDate, DateTime.Today.AddDays(1));
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Any());
            
        }

        [Test]
        public void TestFilterCampingPlaceType()
        {
            this._campingPlacesMock.Object.SelectedCampingPlaceType = "Caravan";
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Count() == 1);

            this._campingPlacesMock.Object.SelectedCampingPlaceType = "Chalet";
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Count() == 2);

            this._campingPlacesMock.Object.SelectedCampingPlaceType = "Camper";
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Count() == 3);

            this._campingPlacesMock.Object.SelectedCampingPlaceType = "Tent";
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Count() == 1);

            this._campingPlacesMock.Object.SelectedCampingPlaceType = "Bungalow";
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Count() == 1);
        }

        [Test]
        public void TestFilterMinPrice()
        {
            Assert.AreEqual(8, this._campingPlacesMock.Object.CampingPlaces.Count());
            
            this._campingPlacesMock.Object.MinNightPrice = "50";
            Assert.AreEqual(5, this._campingPlacesMock.Object.CampingPlaces.Count());

            this._campingPlacesMock.Object.MinNightPrice = "90";
            Assert.AreEqual(0, this._campingPlacesMock.Object.CampingPlaces.Count());
        }

        [Test]
        public void TestFilterMaxPrice()
        {
            Assert.AreEqual(8, this._campingPlacesMock.Object.CampingPlaces.Count());

            this._campingPlacesMock.Object.MaxNightPrice = "40";
            Assert.AreEqual(2, this._campingPlacesMock.Object.CampingPlaces.Count());

            this._campingPlacesMock.Object.MaxNightPrice = "90";
            Assert.AreEqual(8, this._campingPlacesMock.Object.CampingPlaces.Count());
        }

        [Test]
        public void TestFilterOnDate()
        {
            List<Reservation> mockListReservations = new List<Reservation>();
            CampingCustomer testCampingCustomer = new CampingCustomer();

            mockListReservations.Add(new Reservation("2", testCampingCustomer, this._testCampingPlaceOne, this._checkInDate.ToString(CultureInfo.InvariantCulture), this._checkOutDate.ToString(CultureInfo.InvariantCulture)));
            mockListReservations.Add(new Reservation("2", testCampingCustomer, this._testCampingPlaceTwo, this._checkInDate.ToString(CultureInfo.InvariantCulture), this._checkOutDate.ToString(CultureInfo.InvariantCulture)));
            this._campingPlacesMock.Setup(x => x.GetReservationModel()).Returns(mockListReservations);

            var testTwee = this._campingPlacesMock.Object.GetReservationModel();
            var test = this._campingPlacesMock.Object.ToFilteredOnReservedCampingPlaces(this._campingPlaceList, this._checkInDate, this._checkOutDate).Count();
            //-------------------------------------------------------------------------
            Assert.AreEqual(8, this._campingPlacesMock.Object.ToFilteredOnReservedCampingPlaces(this._campingPlaceList, this._checkInDate, this._checkOutDate).Count());
        }
    }
}
