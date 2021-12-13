using NUnit.Framework;
using SystemCore;

namespace SystemCoreTests
{
    public class ValidationTest
    {
        [Test]
        public void TestValidIsInputFilled()
        {
            Assert.IsTrue(Validation.IsInputFilled("Test"));
            Assert.IsTrue(Validation.IsInputFilled("Hallo1"));
            Assert.IsTrue(Validation.IsInputFilled("123"));
        }
        
        [Test]
        public void TestInvalidIsInputFilled()
        {
            Assert.IsFalse(Validation.IsInputFilled(""));
            Assert.IsFalse(Validation.IsInputFilled(null));
        }

        [Test]
        public void TestIsBirthdateValid()
        {
            Assert.IsTrue(Validation.IsBirthdateValid(new System.DateTime(1900, 1, 1)));
            Assert.IsTrue(Validation.IsBirthdateValid(new System.DateTime(1999, 4, 12)));
            Assert.IsTrue(Validation.IsBirthdateValid(new System.DateTime(2021, 4, 12)));
        }

        [Test]
        public void TestIsBirthdateInvalid()
        {
            Assert.IsFalse(Validation.IsBirthdateValid(new System.DateTime(0001, 1, 1)));
            Assert.IsFalse(Validation.IsBirthdateValid(new System.DateTime(1888, 1, 1)));
            Assert.IsFalse(Validation.IsBirthdateValid(new System.DateTime(2030, 4, 12)));
        }

        [Test]
        public void TestIsBirthdateAdultValid()
        {
            Assert.IsTrue(Validation.IsBirthdateAdult(new System.DateTime(1900, 1, 1)));
            Assert.IsTrue(Validation.IsBirthdateAdult(new System.DateTime(1999, 4, 12)));
        }

        [Test]
        public void TestIsBirthdateAdultInvalid()
        {
            Assert.IsFalse(Validation.IsBirthdateAdult(new System.DateTime(2010, 1, 1)));
            Assert.IsFalse(Validation.IsBirthdateAdult(new System.DateTime(2030, 4, 12)));
        }
    }
}