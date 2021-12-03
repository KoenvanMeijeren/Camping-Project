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
            Account account = new Account("1", "admin", "nimda", 1);
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", "1", campingPlaceType);

            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/12", "testPhoneNumber", "testFirstName", "testLastName");

            ReservationDuration reservationDuration = new ReservationDuration("1", "19/10/21", "20/10/21");
            Reservation reservation = new Reservation("1", "1", campingCustomer, campingPlace, reservationDuration);

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
            Account account = new Account("1", "admin", "nimda", 1);
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", "1", campingPlaceType);

            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/12", "testPhoneNumber", "testFirstName", "testLastName");

            ReservationDuration reservationDuration = new ReservationDuration("1", "19/10/21", "20/10/21");
            Reservation reservation = new Reservation("1", campingCustomer, campingPlace, reservationDuration);

            Assert.AreEqual(reservation.Id, -1);
            Assert.AreEqual(reservation.NumberOfPeople, 1);
            Assert.AreEqual(reservation.CampingCustomer, campingCustomer);
            Assert.AreEqual(reservation.CampingPlace, campingPlace);
            Assert.AreEqual(reservation.Duration, reservationDuration);
            Assert.AreEqual(reservation.TotalPrice, reservation.CalculateTotalPrice());
            Assert.AreEqual(reservation.TotalPriceString, $"€{reservation.CalculateTotalPrice()}");
        }

        [Test]
        public void TestReservationCalculateTotalPrice()
        {
            Account account = new Account("1", "admin", "nimda", 1);
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", accommodation);
            CampingPlace campingPlace = new CampingPlace("1", "1", "1", "1", campingPlaceType);

            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/12", "testPhoneNumber", "testFirstName", "testLastName");

            ReservationDuration reservationDuration = new ReservationDuration("1", "19/10/21", "20/10/21");
            Reservation reservation = new Reservation("1", campingCustomer, campingPlace, reservationDuration);

            Assert.AreEqual(reservation.CalculateTotalPrice(), reservation.CampingPlace.TotalPrice * 1);
        }


    }
}