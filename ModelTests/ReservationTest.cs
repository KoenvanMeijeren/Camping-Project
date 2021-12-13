using NUnit.Framework;
using Model;

namespace ModelTests
{
    [TestFixture]
    public class ReservationTest
    {
        [Test]
        public void TestReservationConstructorCorrect()
        {
            Account account = new Account("1", "admin", "nimda", "1");
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", "1", campingPlaceType);

            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/12", "testPhoneNumber", "testFirstName", "testLastName");

            ReservationDuration reservationDuration = new ReservationDuration("1", "12/26/2021 12:00:00", "12/28/2021 12:00:00");
            Reservation reservation = new Reservation("1", "1", campingCustomer, campingPlace, reservationDuration, ReservationColumnStatus.False, ReservationColumnStatus.False, ReservationColumnStatus.False, null);

            Assert.AreEqual(reservation.Id, 1);
            Assert.AreEqual(reservation.NumberOfPeople, 1);
            Assert.AreEqual(reservation.CampingCustomer, campingCustomer);
            Assert.AreEqual(reservation.CampingPlace, campingPlace);
            Assert.AreEqual(reservation.Duration, reservationDuration);
            Assert.AreEqual(reservation.TotalPrice, reservation.CalculateTotalPrice());
            Assert.AreEqual(reservation.TotalPriceString, $"€{reservation.CalculateTotalPrice()}");
        }

        [Test]
        public void TestReservationConstructorIncorrect()
        {
            Reservation reservation = new Reservation(null, null, null, null);

            Assert.AreEqual(reservation.Id, -1);
            Assert.AreEqual(reservation.NumberOfPeople, 0);
            Assert.AreEqual(reservation.CampingCustomer, null);
            Assert.AreEqual(reservation.CampingPlace, null);
            Assert.AreEqual(reservation.Duration, null);
            Assert.AreEqual(reservation.TotalPrice, 0);
            Assert.AreEqual(reservation.TotalPriceString,  "€0");
        }

        [Test]
        public void TestReservationCalculateTotalPrice()
        {
            Account account = new Account("1", "admin", "nimda", "1");
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", "1", campingPlaceType);

            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/12", "testPhoneNumber", "testFirstName", "testLastName");

            ReservationDuration reservationDuration = new ReservationDuration("1", "12/26/2021 12:00:00", "12/28/2021 12:00:00");
            Reservation reservation = new Reservation("1", campingCustomer, campingPlace, reservationDuration);

            Assert.AreEqual(reservation.CampingPlace.TotalPrice * 2, reservation.CalculateTotalPrice());
        }


    }
}