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
    }
}