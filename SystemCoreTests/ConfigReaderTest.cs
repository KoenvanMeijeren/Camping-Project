using NUnit.Framework;
using SystemCore;

namespace SystemCoreTests
{
    public class Tests
    {

        [Test]
        public void test()
        {
            Assert.AreEqual("test", ConfigReader.GetSetting("test"));
        }
        
    }
}