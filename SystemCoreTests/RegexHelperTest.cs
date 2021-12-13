using NUnit.Framework;
using SystemCore;

namespace SystemCoreTests
{
    public class RegexHelperTests
    {
        [Test]
        public void TestValidEmail()
        {
            Assert.IsTrue(RegexHelper.IsEmailValid("test@test.nl"));
            Assert.IsTrue(RegexHelper.IsEmailValid("henkdepotvis@info.com"));
            Assert.IsTrue(RegexHelper.IsEmailValid("email@domain.com"));
        }

        [Test]
        public void TestInvalidEmail()
        {
            Assert.IsFalse(RegexHelper.IsEmailValid("test@test.test"));
            Assert.IsFalse(RegexHelper.IsEmailValid("henkdepotvisinfo.com"));
            Assert.IsFalse(RegexHelper.IsEmailValid("email@domaincom"));
            Assert.IsFalse(RegexHelper.IsEmailValid("emaildomaincom"));
            Assert.IsFalse(RegexHelper.IsEmailValid(""));
            Assert.IsFalse(RegexHelper.IsEmailValid(null));
        }

        [Test]
        public void TestValidPostalCode()
        {
            Assert.IsTrue(RegexHelper.IsPostalcodeValid("1000AA"));
            Assert.IsTrue(RegexHelper.IsPostalcodeValid("6969XD"));
            Assert.IsTrue(RegexHelper.IsPostalcodeValid("9999BA"));
        }

        [Test]
        public void TestInvalidPostalCode()
        {
            Assert.IsFalse(RegexHelper.IsPostalcodeValid(""));
            Assert.IsFalse(RegexHelper.IsPostalcodeValid(null));
            Assert.IsFalse(RegexHelper.IsPostalcodeValid("0999AA"));
            Assert.IsFalse(RegexHelper.IsPostalcodeValid("blablabla"));
            Assert.IsFalse(RegexHelper.IsPostalcodeValid("999AA"));
            Assert.IsFalse(RegexHelper.IsPostalcodeValid("99AA"));
            Assert.IsFalse(RegexHelper.IsPostalcodeValid("9AA"));
            Assert.IsFalse(RegexHelper.IsPostalcodeValid("9999A"));

        }
    }
}