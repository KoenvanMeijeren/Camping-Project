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
        private CampingPlace _testCampingPlaceOne, _testCampingPlaceTwo, _testCampingPlaceThree;

        private List<Reservation> _reservations;


        [SetUp]
        public void Setup()
        {
            this._campingPlacesMock = new Mock<ReservationCampingPlaceFormViewModel>();
            this._checkInDate = DateTime.Today.AddDays(1);
            this._checkOutDate = DateTime.Today.AddDays(3);

            this._campingPlaceList = new List<CampingPlace>
            {
                new CampingPlace("3", "1", "80", "0", new CampingPlaceType("5", "40", new Accommodation("CR", "Caravan"))),
                new CampingPlace("4", "1", "80", "30", new CampingPlaceType("2", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("5", "2", "80", "30", new CampingPlaceType("10", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("6", "1", "80", "10", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("7", "2", "80", "10", new CampingPlaceType("1", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("8", "3", "80", "0", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper")))
            };


            Account account = new Account("1", "admin", "nimda", "0");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingPlace _selectedCampingPlace = new CampingPlace("15", "15", "80", "0", new CampingPlaceType("5", "40", new Accommodation("CA", "Caravan")));
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/21", "testPhoneNumber", "testFirstName", "testLastName");

            this._testCampingPlaceOne = new CampingPlace("3", "1", "80", "0", new CampingPlaceType("5", "40", new Accommodation("CR", "Caravan")));
            this._testCampingPlaceTwo = new CampingPlace("5", "1", "80", "30", new CampingPlaceType("10", "80", new Accommodation("CH", "Chalet")));
            this._testCampingPlaceThree = new CampingPlace("8", "2", "80", "0", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper")));



            _reservations = new List<Reservation>();
            _reservations.Add(new Reservation("2", campingCustomer, this._testCampingPlaceOne, this._checkInDate.ToString(CultureInfo.InvariantCulture), this._checkOutDate.ToString(CultureInfo.InvariantCulture)));
            _reservations.Add(new Reservation("2", campingCustomer, this._testCampingPlaceTwo, this._checkInDate.ToString(CultureInfo.InvariantCulture), this._checkOutDate.ToString(CultureInfo.InvariantCulture)));
            _reservations.Add(new Reservation("2", campingCustomer, this._testCampingPlaceThree, this._checkInDate.ToString(CultureInfo.InvariantCulture), this._checkOutDate.ToString(CultureInfo.InvariantCulture)));
          
      
            this._campingPlacesMock.Setup(x => x.GetCampingPlaces()).Returns(this._campingPlaceList);
            this._campingPlacesMock.Setup(x => x.GetReservations()).Returns(this._reservations);
            this._campingPlacesMock.Object.SelectedAccommodation = ReservationCampingPlaceFormViewModel.SelectAll;
            this._campingPlacesMock.Object.MinNightPrice = "0";
            this._campingPlacesMock.Object.MaxNightPrice = "2000";
            this._campingPlacesMock.Object.CheckInDate = this._checkInDate;
            this._campingPlacesMock.Object.CheckOutDate = this._checkOutDate;

        }

        [Test]
        public void TestInitializeViewModel()
        {
            var mockListFilterOnDate = this._campingPlacesMock.Object.ToFilteredOnReservedCampingPlaces(this._campingPlaceList, this._checkInDate, this._checkOutDate);
            var mockListGetCampingPlaces = this._campingPlacesMock.Object.GetCampingPlaces();
            var mockListGetReservations = this._campingPlacesMock.Object.GetReservations();
            string expectedSelectedPlaceType = ReservationCampingPlaceFormViewModel.SelectAll;

            Assert.AreEqual(this._campingPlacesMock.Object.SelectedAccommodation, expectedSelectedPlaceType);
            Assert.IsTrue(this._campingPlacesMock.Object.Accommodations.Any());
            Assert.IsTrue(mockListFilterOnDate.Any());
            Assert.IsTrue(mockListGetCampingPlaces.Any());
            Assert.AreEqual(this._campingPlacesMock.Object.CheckInDate, DateTime.Today);
            Assert.AreEqual(this._campingPlacesMock.Object.CheckOutDate, DateTime.Today.AddDays(3));
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Any());
            
        }

        [Test]
        public void TestFilterCampingPlaceType()
        {
            this._campingPlacesMock.Object.SelectedAccommodation = "Caravan";
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Count() == 1);

            this._campingPlacesMock.Object.SelectedAccommodation = "Chalet";
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Count() == 2);

            this._campingPlacesMock.Object.SelectedAccommodation = "Camper";
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Count() == 3);

            this._campingPlacesMock.Object.SelectedAccommodation = "Tent";
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Count() == 0);

            this._campingPlacesMock.Object.SelectedAccommodation = "Bungalow";
            Assert.IsTrue(this._campingPlacesMock.Object.CampingPlaces.Count() == 0);
        }

        [Test]
        public void TestFilterMinPrice()
        {
            Assert.AreEqual(6, this._campingPlacesMock.Object.CampingPlaces.Count());
            
            this._campingPlacesMock.Object.MinNightPrice = "50";
            Assert.AreEqual(5, this._campingPlacesMock.Object.CampingPlaces.Count());

            this._campingPlacesMock.Object.MinNightPrice = "90";
            Assert.AreEqual(2, this._campingPlacesMock.Object.CampingPlaces.Count());
        }

        [Test]
        public void TestFilterMaxPrice()
        {
            Assert.AreEqual(6, this._campingPlacesMock.Object.CampingPlaces.Count());

            this._campingPlacesMock.Object.MaxNightPrice = "40";
            Assert.AreEqual(1, this._campingPlacesMock.Object.CampingPlaces.Count());

            this._campingPlacesMock.Object.MaxNightPrice = "90";
            Assert.AreEqual(4, this._campingPlacesMock.Object.CampingPlaces.Count());
        }

        [Test]
        public void TestFilterOnDate()
        {
            List<Reservation> mockListReservations = new List<Reservation>();
            CampingCustomer testCampingCustomer = new CampingCustomer();

            mockListReservations.Add(new Reservation("2", testCampingCustomer, this._testCampingPlaceOne, this._checkInDate.ToString(CultureInfo.InvariantCulture), this._checkOutDate.ToString(CultureInfo.InvariantCulture)));
            mockListReservations.Add(new Reservation("2", testCampingCustomer, this._testCampingPlaceTwo, this._checkInDate.ToString(CultureInfo.InvariantCulture), this._checkOutDate.ToString(CultureInfo.InvariantCulture)));
            this._campingPlacesMock.Setup(x => x.GetReservations()).Returns(mockListReservations);

            //var testTwee = this._campingPlacesMock.Object.GetReservations();
            //var test = this._campingPlacesMock.Object.ToFilteredOnReservedCampingPlaces(this._campingPlaceList, this._checkInDate, this._checkOutDate).Count();
            //-------------------------------------------------------------------------
            Assert.AreEqual(0, this._campingPlacesMock.Object.ToFilteredOnReservedCampingPlaces(this._campingPlaceList, this._checkInDate, this._checkOutDate).Count());
        }
    }
}
