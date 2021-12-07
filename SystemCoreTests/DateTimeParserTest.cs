using System;
using NUnit.Framework;
using SystemCore;

namespace SystemCoreTests
{
    public class DateTimeParserTests
    {
        [Test]
        public void TestTryParseFromStringDate()
        {
            Assert.AreEqual(new DateTime(2021, 12, 26), DateTimeParser.TryParse("12/26/2021 12:00:00"));
            Assert.AreEqual(new DateTime(2021, 12, 28), DateTimeParser.TryParse("12/28/2021 12:00:00"));
            
            Assert.AreEqual(new DateTime(2021, 12, 1), DateTimeParser.TryParse("12/01/2021 12:00:00"));
            Assert.AreEqual(new DateTime(2021, 12, 4), DateTimeParser.TryParse("12/04/2021 12:00:00"));

            Assert.AreEqual(new DateTime(2022, 3, 1), DateTimeParser.TryParse("03/01/2022 12:00:00"));
            Assert.AreEqual(new DateTime(2022, 3, 4), DateTimeParser.TryParse("03/04/2022 12:00:00"));
            
            Assert.AreEqual(new DateTime(2022, 1, 1), DateTimeParser.TryParse("01/01/2022 12:00:00"));
            Assert.AreEqual(new DateTime(2022, 1, 4), DateTimeParser.TryParse("01/04/2022 12:00:00"));
        }

        [Test]
        public void TestTryParseToDatabaseFormatString()
        {
            Assert.AreEqual("12-26-2021 12:00:00", DateTimeParser.TryParseToDatabaseFormat(new DateTime(2021, 12, 26)));
            Assert.AreEqual("12-28-2021 12:00:00", DateTimeParser.TryParseToDatabaseFormat(new DateTime(2021, 12, 28)));
            
            Assert.AreEqual("12-01-2021 12:00:00", DateTimeParser.TryParseToDatabaseFormat(new DateTime(2021, 12, 1)));
            Assert.AreEqual("12-04-2021 12:00:00", DateTimeParser.TryParseToDatabaseFormat(new DateTime(2021, 12, 4)));
            
            Assert.AreEqual("03-01-2022 12:00:00", DateTimeParser.TryParseToDatabaseFormat(new DateTime(2022, 3, 1)));
            Assert.AreEqual("03-04-2022 12:00:00", DateTimeParser.TryParseToDatabaseFormat(new DateTime(2022, 3, 4)));
            
            Assert.AreEqual("01-01-2022 12:00:00", DateTimeParser.TryParseToDatabaseFormat(new DateTime(2022, 1, 1)));
            Assert.AreEqual("01-04-2022 12:00:00", DateTimeParser.TryParseToDatabaseFormat(new DateTime(2022, 1, 4)));
        }
    }
}