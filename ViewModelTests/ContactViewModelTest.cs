using System;
using Model;
using Moq;
using NUnit.Framework;
using ViewModel;

namespace ViewModelTests
{
    public class ContactViewModelTest
    {
        private Mock<ContactViewModel> _campingModelMock;
        private ContactViewModel _contactViewModel = new ContactViewModel();
        private Camping _camping;
        private bool _triggeredToChatEvent;


        [SetUp]
        public void Setup()
        {
            this._campingModelMock = new();
            this._contactViewModel = new();

            ContactViewModel.FromContactToChatEvent += ContactViewModelToChatEvent;

            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            Account account = new Account("1", "admin", "nimda", "0");
            CampingOwner campingOwner = new CampingOwner("1", account, "Jan", "Janssen");
            this._camping = new Camping("2", "Wildhoeve", address, campingOwner, "0612312312", "test@hotmail.com", "https://www.facebook.com", "https://www.twitter.com", "https://www.instagram.com");
            
            this._campingModelMock.Setup(x => x.GetCamping()).Returns(this._camping);
            this._campingModelMock.Object.CurrentCamping = this._camping;
        }

        private void ContactViewModelToChatEvent(object? sender, EventArgs e)
        {
            this._triggeredToChatEvent = true;
        }

        [Test]
        public void TestCampingConstruction()
        {
            Assert.AreEqual("Adres: testAddress, testPlace", this._campingModelMock.Object.ContactPageAddress);
            Assert.AreEqual("Postcode: testPostalCode", this._campingModelMock.Object.ContactPostalCode);
            Assert.AreEqual("Telefoonnummer: 0612312312", this._campingModelMock.Object.ContactPagePhoneNumber);
            Assert.AreEqual("Email: test@hotmail.com", this._campingModelMock.Object.ContactPageEmailAddress);
            Assert.AreEqual("https://www.facebook.com", this._campingModelMock.Object.FacebookLink);
            Assert.AreEqual("https://www.twitter.com", this._campingModelMock.Object.TwitterLink);
            Assert.AreEqual("https://www.instagram.com", this._campingModelMock.Object.InstagramLink);
        }

        [Test]
        public void TestToChatEvent()
        {
            Assert.IsFalse(this._triggeredToChatEvent);
            this._contactViewModel.ChatButton.Execute(null);
            Assert.IsTrue(this._triggeredToChatEvent);

        }
    }
}
