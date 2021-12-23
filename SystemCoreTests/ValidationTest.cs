using System;
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
        public void TestValidIsInputBelowMaxLength()
        {
            Assert.IsTrue(Validation.IsInputBelowMaxLength("Test", 5));
            Assert.IsTrue(Validation.IsInputBelowMaxLength("Test", 4));
            Assert.IsTrue(Validation.IsInputBelowMaxLength("Hallo1", 7));
            Assert.IsTrue(Validation.IsInputBelowMaxLength("Hallo1", 6));
            Assert.IsTrue(Validation.IsInputBelowMaxLength("123", 4));
            Assert.IsTrue(Validation.IsInputBelowMaxLength("123", 3));
            Assert.IsTrue(Validation.IsInputBelowMaxLength("", 0));
        }
        
        [Test]
        public void TestInvalidIsInputBelowMaxLength()
        {
            Assert.IsFalse(Validation.IsInputBelowMaxLength("test", 2));
            Assert.IsFalse(Validation.IsInputBelowMaxLength(null, 2));
        }
        
        [Test]
        public void TestIsBirthdateValid()
        {
            Assert.IsTrue(Validation.IsBirthdateValid(DateTime.Now.AddYears(-20)));
            Assert.IsTrue(Validation.IsBirthdateValid(DateTime.Now.AddYears(-100)));
            Assert.IsTrue(Validation.IsBirthdateValid(DateTime.Now));
        }

        [Test]
        public void TestIsBirthdateInvalid()
        {
            Assert.IsFalse(Validation.IsBirthdateValid(DateTime.Today.AddYears(-200)));
            Assert.IsFalse(Validation.IsBirthdateValid(DateTime.MinValue));
        }

        [Test]
        public void TestIsBirthdateAdultValid()
        {
            Assert.IsTrue(Validation.IsBirthdateAdult(DateTime.Today.AddYears(-20)));
            Assert.IsTrue(Validation.IsBirthdateAdult(DateTime.Today.AddYears(-19).AddDays(-10)));
            Assert.IsTrue(Validation.IsBirthdateAdult(DateTime.Today.AddYears(-18)));
            Assert.IsTrue(Validation.IsBirthdateAdult(DateTime.Today.AddYears(-18).AddDays(-1)));
        }

        [Test]
        public void TestIsBirthdateAdultInvalid()
        {
            Assert.IsFalse(Validation.IsBirthdateAdult(DateTime.Today.AddYears(-17)));
            Assert.IsFalse(Validation.IsBirthdateAdult(DateTime.Today.AddYears(-10)));
            Assert.IsFalse(Validation.IsBirthdateAdult(DateTime.Today.AddYears(18).AddDays(1)));
            Assert.IsFalse(Validation.IsBirthdateAdult(DateTime.Today.AddYears(10)));
            Assert.IsFalse(Validation.IsBirthdateAdult(DateTime.Today.AddYears(17)));
        }

        [Test]
        public void TestIsNumberValid()
        {
            Assert.IsTrue(Validation.IsNumber("2"));
            Assert.IsTrue(Validation.IsNumber("0"));
            Assert.IsTrue(Validation.IsNumber("-1"));
        }

        [Test]
        public void TestIsNumberInvalid()
        {
            Assert.IsFalse(Validation.IsNumber("2fe"));
            Assert.IsFalse(Validation.IsNumber("faf"));
            Assert.IsFalse(Validation.IsNumber(""));
            Assert.IsFalse(Validation.IsNumber(null));
        }
        
        [Test]
        public void TestIsDecimalNumberValid()
        {
            Assert.IsTrue(Validation.IsDecimalNumber("2.2"));
            Assert.IsTrue(Validation.IsDecimalNumber("2,2"));
            Assert.IsTrue(Validation.IsDecimalNumber("2"));
            Assert.IsTrue(Validation.IsDecimalNumber("0.0"));
            Assert.IsTrue(Validation.IsDecimalNumber("0,0"));
            Assert.IsTrue(Validation.IsDecimalNumber("0"));
            Assert.IsTrue(Validation.IsDecimalNumber("-1.1"));
            Assert.IsTrue(Validation.IsDecimalNumber("-1,1"));
            Assert.IsTrue(Validation.IsDecimalNumber("-1"));
        }

        [Test]
        public void TestIsDecimalNumberInvalid()
        {
            Assert.IsFalse(Validation.IsDecimalNumber("2fe"));
            Assert.IsFalse(Validation.IsDecimalNumber("faf"));
            Assert.IsFalse(Validation.IsDecimalNumber(""));
            Assert.IsFalse(Validation.IsDecimalNumber(null));
        }
    }
}