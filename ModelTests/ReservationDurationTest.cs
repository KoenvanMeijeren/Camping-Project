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
            ReservationDuration reservationDuration = new ReservationDuration("12", "2021-12-26 12:00:00", "2021-12-28 12:00:00");
            Assert.AreEqual(reservationDuration.Id, 12);
            Assert.AreEqual("", reservationDuration.CheckInDatetime.ToShortDateString());
            Assert.AreEqual("", reservationDuration.CheckOutDatetime.ToShortDateString());
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
