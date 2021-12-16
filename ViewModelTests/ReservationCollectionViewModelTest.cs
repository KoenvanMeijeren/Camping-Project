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
    public class ReservationCollectionViewModelTest
    {
        private Mock<ReservationCollectionViewModel> _reservationCollectionMock;
        private List<Reservation> _reservations;

        [SetUp]
        public void Setup()
        {
            this._reservationCollectionMock = new();
            Account account = new Account("1", "admin", "nimda", "0");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/21", "testPhoneNumber", "testFirstName", "testLastName");
            List<CampingPlace> campingPlaces = new List<CampingPlace>
            {
                new CampingPlace("3", "1", "80", "0", new CampingPlaceType("5", "40", new Accommodation("CA", "Caravan"))),
                new CampingPlace("4", "1", "80", "30", new CampingPlaceType("2", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("5", "1", "80", "30", new CampingPlaceType("10", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("6", "1", "50", "10", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("7", "1", "50", "10", new CampingPlaceType("1", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("8", "1", "60", "0", new CampingPlaceType("2", "50", new Accommodation("TE", "Tent")))
            };

            this._reservations = new();
            int i = 1;
            foreach (var campingPlace in campingPlaces)
            {
                this._reservations.Add(new Reservation(i.ToString(), "2", campingCustomer, campingPlace, ReservationColumnStatus.False, ReservationColumnStatus.False, ReservationColumnStatus.False, "", "12/26/2021 12:00:00", "12/28/2021 12:00:00"));
                i++;
            }

            this._reservationCollectionMock.Setup(x => x.GetReservations()).Returns(this._reservations);
        }

        [Test]
        public void TestThatWeHaveReservations()
        {
            Assert.NotNull(this._reservationCollectionMock.Object.Reservations);
            Assert.AreEqual(6, this._reservationCollectionMock.Object.Reservations.Count);
        }

        [Test]
        public void TestFilterCampingPlaceType()
        {
            this._reservationCollectionMock.Object.SelectedCampingPlaceType = "Caravan";
            Assert.AreEqual(this._reservationCollectionMock.Object.Reservations.Count(), 1);

            this._reservationCollectionMock.Object.SelectedCampingPlaceType = "Chalet";
            Assert.AreEqual(this._reservationCollectionMock.Object.Reservations.Count(), 2);

            this._reservationCollectionMock.Object.SelectedCampingPlaceType = "Camper";
            Assert.AreEqual(this._reservationCollectionMock.Object.Reservations.Count(), 2);

            this._reservationCollectionMock.Object.SelectedCampingPlaceType = "Tent";
            Assert.AreEqual(this._reservationCollectionMock.Object.Reservations.Count(), 1);

            this._reservationCollectionMock.Object.SelectedCampingPlaceType = "Bungalow";
            Assert.AreEqual(this._reservationCollectionMock.Object.Reservations.Count(), 0);
        }

        [Test]
        public void TestFilterMinPrice()
        {
            this._reservationCollectionMock.Object.MinTotalPrice = "50";
            Assert.AreEqual(6, this._reservationCollectionMock.Object.Reservations.Count());

            this._reservationCollectionMock.Object.MinTotalPrice = "90";
            Assert.AreEqual(5, this._reservationCollectionMock.Object.Reservations.Count());
        }

        [Test]
        public void TestFilterMaxPrice()
        {
            this._reservationCollectionMock.Object.MaxTotalPrice = "40";
            Assert.AreEqual(0, this._reservationCollectionMock.Object.Reservations.Count());

            this._reservationCollectionMock.Object.MaxTotalPrice = "90";
            Assert.AreEqual(1, this._reservationCollectionMock.Object.Reservations.Count());
        }

        [Test]
        public void TestReservationAmount()
        {
            Assert.AreEqual(this._reservationCollectionMock.Object.Reservations.Count(), 6);
            this._reservationCollectionMock.Object.Reservations.Add(this._reservationCollectionMock.Object.Reservations[this._reservationCollectionMock.Object.Reservations.Count() - 1]);
            Assert.AreEqual(this._reservationCollectionMock.Object.Reservations.Count(), 7);
            this._reservationCollectionMock.Object.Reservations.Remove(this._reservationCollectionMock.Object.Reservations[this._reservationCollectionMock.Object.Reservations.Count() - 1]);
            this._reservationCollectionMock.Object.Reservations.Remove(this._reservationCollectionMock.Object.Reservations[this._reservationCollectionMock.Object.Reservations.Count() - 1]);
            Assert.AreEqual(this._reservationCollectionMock.Object.Reservations.Count, 5);
        }
    }
}
