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
    /// <summary>
    /// Unittests for filtering and displaying reservations
    /// </summary>
    public class ReservationCollectionViewModelTest
    {
        private Mock<ReservationCollectionViewModel> _ReservationCollectionMockObject;
        private List<Accommodation> _campingPlaceTypeList;
        private List<Reservation> _reservations;


        [SetUp]
        public void Setup()
        {
            this._campingPlaceTypeList = new List<Accommodation>
            {
                new Accommodation("CA", "Caravan"),
                new Accommodation("CH", "Chalet"),
                new Accommodation("BU", "Bungalow"),
                new Accommodation("TE", "Tent")
            };

            this._reservations = new List<Reservation>
            {
                new Reservation(),
                new Reservation(),
                new Reservation(),
                new Reservation(),
                new Reservation()
            };

            this._ReservationCollectionMockObject = new Mock<ReservationCollectionViewModel>();
            this._ReservationCollectionMockObject.Setup(x => x.SelectAccomodationTypes()).Returns(this._campingPlaceTypeList);
            this._ReservationCollectionMockObject.Setup(i => i.SelectReservations()).Returns(_reservations);
            this._ReservationCollectionMockObject.Object.CheckInDate = DateTime.Today;
            this._ReservationCollectionMockObject.Object.CheckOutDate = DateTime.Today.AddMonths(1).AddDays(-1);

        }


        [Test]
        public void TestInitializeViewModel()
        {
            string expectedSelectedPlaceType = ReservationCampingPlaceFormViewModel.SelectAll;

            Assert.AreEqual(this._ReservationCollectionMockObject.Object.SelectedCampingPlaceType, expectedSelectedPlaceType);
            Assert.IsNotEmpty(this._ReservationCollectionMockObject.Object.CampingPlaceTypes);
            Assert.AreEqual(this._ReservationCollectionMockObject.Object.CheckInDate, DateTime.Today);
            Assert.AreEqual(this._ReservationCollectionMockObject.Object.CheckOutDate, DateTime.Today.AddMonths(1).AddDays(-1));

        }
    }
}
