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
using ViewModel.EventArguments;


namespace ViewModelTests
{
    public class ManageReservationViewModelTest
    {
        private Mock<ManageReservationViewModel> _manageReservationViewModelMock;
        private ReservationCollectionViewModel _reservationCollectionViewModel = new ReservationCollectionViewModel();
        
        private List<CampingPlace> _campingPlaceList;
        private List<Reservation> _reservations;

        private bool _triggeredBackToDashboardEvent = false, _triggeredOnManageReservationEvent = false;

        [SetUp]
        public void Setup()
        {
            this._manageReservationViewModelMock = new();

            Account account = new Account("1", "admin", "nimda", "0");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/21", "testPhoneNumber", "testFirstName", "testLastName");

            this._campingPlaceList = new List<CampingPlace>
            {
                new CampingPlace("3", "1", "80", "0", new CampingPlaceType("5", "40", new Accommodation("CA", "Caravan"))),
                new CampingPlace("4", "1", "80", "30", new CampingPlaceType("2", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("5", "1", "80", "30", new CampingPlaceType("10", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("6", "1", "80", "10", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("7", "1", "80", "10", new CampingPlaceType("1", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("8", "1", "80", "0", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper")))
            };

            this._reservations = new();
            int counter = 1;
            foreach (var campingPlace in this._campingPlaceList)
            {
                if (counter > 4)
                {
                    break;
                }
                
                this._reservations.Add(new Reservation(counter.ToString(), "2", campingCustomer, campingPlace, ReservationColumnStatus.False, ReservationColumnStatus.False, ReservationColumnStatus.False, "", "12/26/2021 12:00:00", "12/28/2021 12:00:00"));
                counter++;
            }

            this._manageReservationViewModelMock.Setup(x => x.GetCampingPlaces()).Returns(this._campingPlaceList);
            this._manageReservationViewModelMock.Object.CheckInDate = DateTime.Today;
            this._manageReservationViewModelMock.Object.CheckOutDate = DateTime.Today.AddDays(1);
            this._manageReservationViewModelMock.Setup(x => x.GetReservations()).Returns(this._reservations);
            
            ManageReservationViewModel.FromReservationBackToDashboardEvent += this.ManageReservationViewModelOnFromReservationBackToDashboardEvent;
            ReservationCollectionViewModel.ManageReservationEvent += this.ReservationCollectionViewModelOnManageReservationEvent;
        }

        private void ReservationCollectionViewModelOnManageReservationEvent(object sender, ReservationEventArgs e)
        {
            this._triggeredOnManageReservationEvent = true;
        }

        private void ManageReservationViewModelOnFromReservationBackToDashboardEvent(object sender, EventArgs e)
        {
            this._triggeredBackToDashboardEvent = true;
        }

        [Test]
        public void TestInitializeViewModel()
        {
            Assert.AreEqual(this._campingPlaceList.Count + 1, this._manageReservationViewModelMock.Object.CampingPlaces.Count);
            Assert.IsNull(this._manageReservationViewModelMock.Object.PageTitle);
        }

        [Test]
        public void TestManageReservation()
        {
            this._reservationCollectionViewModel.SelectedReservation = this._reservations.First();

            Assert.AreEqual("Reservering 1 bijwerken", this._manageReservationViewModelMock.Object.PageTitle);
            Assert.AreEqual(this._campingPlaceList.First(), this._manageReservationViewModelMock.Object.SelectedCampingPlace);
            Assert.AreEqual("2", this._manageReservationViewModelMock.Object.NumberOfPeople);
            Assert.AreEqual("26-12-2021", this._manageReservationViewModelMock.Object.CheckInDate.ToShortDateString());
            Assert.AreEqual("28-12-2021", this._manageReservationViewModelMock.Object.CheckOutDate.ToShortDateString());
        }
        
        [Test]
        public void TestManageReservationBackToDashboard()
        {
            this._reservationCollectionViewModel.SelectedReservation = this._reservations.First();

            Assert.IsFalse(this._triggeredBackToDashboardEvent);
            this._manageReservationViewModelMock.Object.GoBackToDashboard.Execute(null);
            Assert.IsTrue(this._triggeredBackToDashboardEvent);
        }
        
        [Test]
        public void TestManageReservationChangeFields()
        {
            this._triggeredOnManageReservationEvent = false;
            Assert.IsFalse(this._triggeredOnManageReservationEvent);
            this._reservationCollectionViewModel.SelectedReservation = this._reservations.First();
            Assert.IsTrue(this._triggeredOnManageReservationEvent);

            Assert.AreEqual(2, this._manageReservationViewModelMock.Object.GetFilteredCampingPlaces().Count());
            this._manageReservationViewModelMock.Object.CheckInDate = new DateTime(2021, 12, 29);
            Assert.AreEqual(7, this._manageReservationViewModelMock.Object.CampingPlaces.Count);

            Assert.AreEqual("2", this._manageReservationViewModelMock.Object.NumberOfPeople);
            this._manageReservationViewModelMock.Object.NumberOfPeople = "3";
            Assert.AreEqual("3", this._manageReservationViewModelMock.Object.NumberOfPeople);
        }
    }
}
