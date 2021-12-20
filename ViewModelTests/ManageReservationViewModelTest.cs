using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Model;
using Model;
using Moq;


namespace ViewModelTests
{
    public class ManageReservationViewModelTest
    {
        private Mock<ManageReservationViewModel> _manageReservationViewModelMock;
        private List<CampingPlace> _campingPlaceList;
        private List<Reservation> _reservations;
        private CampingPlace _selectedCampingPlace;

        private DateTime _checkInDate, _checkOutDate;


        [SetUp]
        public void Setup()
        {
            _manageReservationViewModelMock = new();
            this._checkInDate = DateTime.Today.AddDays(1);
            this._checkOutDate = DateTime.Today.AddDays(3);

            Account account = new Account("1", "admin", "nimda", "0");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            _selectedCampingPlace= new CampingPlace("15", "15", "80", "0", new CampingPlaceType("5", "40", new Accommodation("CA", "Caravan")));
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/21", "testPhoneNumber", "testFirstName", "testLastName");


            _campingPlaceList = new List<CampingPlace>
            {
                new CampingPlace("3", "1", "80", "0", new CampingPlaceType("5", "40", new Accommodation("CA", "Caravan"))),
                new CampingPlace("4", "1", "80", "30", new CampingPlaceType("2", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("5", "1", "80", "30", new CampingPlaceType("10", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("6", "1", "80", "10", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("7", "1", "80", "10", new CampingPlaceType("1", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("8", "1", "80", "0", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper")))
            };

            this._reservations = new();
            int i = 1;
            foreach (var campingPlace in _campingPlaceList)
            {
                this._reservations.Add(new Reservation(i.ToString(), "2", campingCustomer, campingPlace, ReservationColumnStatus.False, ReservationColumnStatus.False, ReservationColumnStatus.False, "", "12/26/2021 12:00:00", "12/28/2021 12:00:00"));
                i++;
            }

            this._manageReservationViewModelMock.Object.SelectedCampingPlace = this._selectedCampingPlace;
            this._manageReservationViewModelMock.Setup(x => x.SelectCampingPlaces()).Returns(this._campingPlaceList);
            this._manageReservationViewModelMock.Object.CheckInDate = DateTime.Today;
            this._manageReservationViewModelMock.Object.CheckOutDate = DateTime.Today.AddDays(1);
            this._manageReservationViewModelMock.Setup(x => x.GetReservationModel()).Returns(this._reservations);
        }


        [Test]
        public void TestInitializeViewModel()
        {
            //+1 for selectedCampingplace
            Assert.IsTrue(_manageReservationViewModelMock.Object.CampingPlaces.Count == this._campingPlaceList.Count+1);
            Assert.IsTrue(this._selectedCampingPlace == this._manageReservationViewModelMock.Object.SelectedCampingPlace);
            Assert.IsTrue(_manageReservationViewModelMock.Object.CampingPlaces.Contains(this._selectedCampingPlace));
        }
    }
}
