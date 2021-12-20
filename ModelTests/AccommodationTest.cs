using NUnit.Framework;
using Visualization;

namespace ModelTests
{
    [TestFixture]
    public class AccommodationTest
    {

        [Test]
        public void TestAccommodationConstructorCorrect()
        {
            Accommodation accommodation = new Accommodation("1", "testPrefix", "testName");
            Assert.AreEqual(accommodation.Id, 1);
            Assert.AreEqual(accommodation.Prefix, "testPrefix");
            Assert.AreEqual(accommodation.Name, "testName");
        }


        [Test]
        public void TestAccommodationConstructorIncorrect()
        {
            Accommodation accommodation = new Accommodation(null, null, null);
            Assert.AreEqual(accommodation.Id, -1);
            Assert.AreEqual(accommodation.Prefix, null);
            Assert.AreEqual(accommodation.Name, null);
        }
    }
}