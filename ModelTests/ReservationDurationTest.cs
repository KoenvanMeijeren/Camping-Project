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
            ReservationDuration reservationDuration = new ReservationDuration("12", "12/26/2021 12:00:00", "12/28/2021 12:00:00");
            Assert.AreEqual(reservationDuration.Id, 12);
            Assert.AreEqual("26-12-2021", reservationDuration.CheckInDatetime.ToShortDateString());
            Assert.AreEqual("28-12-2021", reservationDuration.CheckOutDatetime.ToShortDateString());
            Assert.AreEqual("12/26/2021 12:00:00", reservationDuration.CheckInDateDatabaseFormat);
            Assert.AreEqual("12/28/2021 12:00:00", reservationDuration.CheckOutDateDatabaseFormat);
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
