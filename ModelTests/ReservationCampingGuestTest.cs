using NUnit.Framework;
using Visualization;

namespace ModelTests
{
    [TestFixture]
    public class ReservationCampingGuestTest
    {
        [Test]
        public void TestReservationCampingGuestConstructorCorrect()
        {
            Account account = new Account("1", "admin", "nimda",  "1");
            CampingGuest campingGuest = new CampingGuest("1", "testName", "19/10/21");

            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", "1", campingPlaceType);

            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/12", "testPhoneNumber", "testFirstName", "testLastName");

            Reservation reservation = new Reservation("1", "1", campingCustomer, campingPlace, ReservationColumnStatus.False, ReservationColumnStatus.False, ReservationColumnStatus.False, null, "19/10/21", "20/10/21");

            ReservationCampingGuest reservationCampingGuest = new ReservationCampingGuest("1", reservation, campingGuest);
            Assert.AreEqual(reservationCampingGuest.Id, 1);
            Assert.AreEqual(reservationCampingGuest.CampingGuest, campingGuest);
            Assert.AreEqual(reservationCampingGuest.Reservation, reservation);
        }

        [Test]
        public void TestReservationCampingGuestConstructorIncorrect()
        {
            ReservationCampingGuest reservationCampingGuest = new ReservationCampingGuest(null, null, null);
            Assert.AreEqual(reservationCampingGuest.Id, -1);
            Assert.AreEqual(reservationCampingGuest.CampingGuest, null);
            Assert.AreEqual(reservationCampingGuest.Reservation, null);
        }
    }
}