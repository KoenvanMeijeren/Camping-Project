using NUnit.Framework;
using Model;
using System;

namespace ModelTests
{
    [TestFixture]
    public class ReservationDurationTest
    {
        [Test]
        public void TestReservationDurationConstructorCorrect()
        {
            ReservationDuration reservationDuration = new ReservationDuration("12", "19/10/21", "20/10/21");
            Assert.AreEqual(reservationDuration.Id, 12);
            Assert.AreEqual(reservationDuration.CheckInDatetime, DateTime.Parse("19/10/21"));
            Assert.AreEqual(reservationDuration.CheckOutDatetime, DateTime.Parse("20/10/21"));
            Assert.AreEqual(reservationDuration.CheckInDateDatabaseFormat, DateTime.Parse("19/10/21").ToShortDateString());
            Assert.AreEqual(reservationDuration.CheckOutDateDatabaseFormat, DateTime.Parse("20/10/21").ToShortDateString());
        }

        [Test]
        public void TestReservationDurationConstructorIncorrect()
        {
            ReservationDuration reservationDuration = new ReservationDuration(null, null, null);
            Assert.AreEqual(reservationDuration.Id, -1);
            Assert.AreEqual(reservationDuration.CheckInDatetime, DateTime.MinValue);
            Assert.AreEqual(reservationDuration.CheckOutDatetime, DateTime.MinValue);
        }
    }
}
