using NUnit.Framework;
using SystemCore;

namespace SystemCoreTests
{
    class PasswordHashingTest
    {
        [Test]
        public void IsValidBase64StringTest()
        {
            //TestenVoorToegang
            Assert.IsTrue(PasswordHashing.IsBase64String("VGVzdGVuVm9vclRvZWdhbmc="));
            //LoremIpsumDolorSitAmet
            Assert.IsTrue(PasswordHashing.IsBase64String("TG9yZW1JcHN1bURvbG9yU2l0QW1ldA=="));
            //Op dit moment is het coronatoegangsbewijs op basis van vaccinatie eindeloos geldig binnen Nederland.
            Assert.IsTrue(PasswordHashing.IsBase64String("T3AgZGl0IG1vbWVudCBpcyBoZXQgY29yb25hdG9lZ2FuZ3NiZXdpanMgb3AgYmFzaXMgdmFuIHZhY2NpbmF0aWUgZWluZGVsb29zIGdlbGRpZyBiaW5uZW4gTmVkZXJsYW5kLg=="));
        }

        [Test]
        public void IsInvalidBase64StringTest()
        {
            //LoremIpsumDolorSitAmet + invalid character !
            Assert.IsFalse(PasswordHashing.IsBase64String("!TG9yZW1JcHN1bURvbG9yU2l0QW1ldA=="));
            //LoremIpsumDolorSitAmet + invalid character %
            Assert.IsFalse(PasswordHashing.IsBase64String("%TG9yZW1JcHN1bURvbG9yU2l0QW1ldA=="));
        }

        [Test]
        public void IsValidSignInHashValidation()
        {
            Assert.IsTrue(PasswordHashing.SignInHashValidation(PasswordHashing.HashPassword("123"), "123"));
            Assert.IsTrue(PasswordHashing.SignInHashValidation(PasswordHashing.HashPassword("LoremIpsumDolorSitAmet"), "LoremIpsumDolorSitAmet"));
            Assert.IsTrue(PasswordHashing.SignInHashValidation(PasswordHashing.HashPassword("KoenKampioen123312"), "KoenKampioen123312"));
        }

        [Test]
        public void IsInvalidSignInHashValidation()
        {
            Assert.IsFalse(PasswordHashing.SignInHashValidation(PasswordHashing.HashPassword("123"), ""));
            Assert.IsFalse(PasswordHashing.SignInHashValidation(PasswordHashing.HashPassword("LoremIpsumDolorSitAmet"), "KomtDuidelijkNietOvereen:)"));
            Assert.IsFalse(PasswordHashing.SignInHashValidation(PasswordHashing.HashPassword("KoenKampioen123312"), "KoenGeenKampioen123312"));
        }

        [Test]
        public void IsValidHashedPassword()
        {
            // Hashed passwords are checked on string.length, no other testvariations possible (short string, normal string, long (134char) string)
            Assert.AreEqual(PasswordHashing.HashPassword("123").Length, 48);
            Assert.AreEqual(PasswordHashing.HashPassword("LoremIpsumDolorSitAmet").Length, 48);
            Assert.AreEqual(PasswordHashing.HashPassword("T3AgZGl0IG1vbWVudCBpcyBoZXQgY29yb25hdG9lZ2FuZ3NiZXdpanMgb3AgYmFzaXMgdmFuIHZhY2NpbmF0aWUgZWluZGVsb29zIGdlbGRpZyBiaW5uZW4gTmVkZXJsYW5kLg").Length, 48);
        }

        [Test]
        public void IsInvalidHashedPassword()
        {
            // Hashed passwords are checked on string.length, no other testvariations possible
            Assert.IsFalse(PasswordHashing.HashPassword("123").Length != 48);
        }
    }
}
