using NUnit.Framework;
using SystemCore;

namespace SystemCoreTests
{
    public class ValidationTest
    {
        [Test]
        public void TestValidEmail()
        {
            Assert.IsTrue(Validation.IsInputFilled("test"));
        }
        
        [Test]
        public void TestInvalidEmail()
        {
            Assert.IsFalse(Validation.IsInputFilled(""));
            Assert.IsFalse(Validation.IsInputFilled(null));
        }
    }
}