using NUnit.Framework;
using Model;

namespace ModelTests
{
    [TestFixture]
    public class AccountTest
    {

        [Test]
        public void TestAccountConstructorCorrect()
        {
            Account account = new Account("test", "nimda", "1");
            Assert.AreEqual(-1, account.Id);
            Assert.AreEqual("test", account.Email);
            Assert.AreEqual("nimda", account.Password);
            Assert.AreEqual(1, account.Rights);
        }


        [Test]
        public void TestAccountConstructorIncorrect()
        {
            Account account = new Account(null, null, null, null);
            Assert.AreEqual(-1, account.Id);
            Assert.AreEqual(null, account.Email);
            Assert.AreEqual(null, account.Password);
            Assert.AreEqual(0, account.Rights);
        }
    }
}
