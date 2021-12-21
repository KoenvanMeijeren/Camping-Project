using System;
using Model;
using Moq;
using NUnit.Framework;
using ViewModel;

namespace ViewModelTests
{
    public class AccountUpdateViewModelTest
    {
        private AccountUpdateViewModel _accountUpdateViewModel;
        private AccountViewModel _accountViewModel;

        private bool _triggeredCancelEvent, _triggeredToUpdatePageEvent;
        
        [SetUp]
        public void Setup()
        {
            this._accountUpdateViewModel = new AccountUpdateViewModel();
            this._accountViewModel = new AccountViewModel();
            
            Account account = new Account("1", "customer", "nimda", "0");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/21", "testPhoneNumber", "testFirstName", "testLastName");

            CurrentUser.SetCurrentUser(account, campingCustomer);
            
            AccountUpdateViewModel.UpdateCancelEvent += AccountUpdateViewModelOnUpdateCancelEvent;
            AccountViewModel.ToAccountUpdatePageEvent += AccountUpdateViewModelOnUpdateConfirmEvent;
        }

        private void AccountUpdateViewModelOnUpdateConfirmEvent(object? sender, EventArgs e)
        {
            this._triggeredToUpdatePageEvent = true;
        }

        private void AccountUpdateViewModelOnUpdateCancelEvent(object? sender, EventArgs e)
        {
            this._triggeredCancelEvent = true;
        }

        [Test]
        public void TestExecuteCancelAction()
        {
            Assert.IsTrue(this._accountUpdateViewModel.UpdateCancel.CanExecute(null));
            
            Assert.IsFalse(this._triggeredCancelEvent);
            this._accountUpdateViewModel.UpdateCancel.Execute(null);
            Assert.IsTrue(this._triggeredCancelEvent);
        }

        [Test]
        public void TestExecuteUpdateConfirmAction()
        {
            Assert.IsFalse(this._accountUpdateViewModel.UpdateConfirm.CanExecute(null));
            
            CurrentUser.EmptyCurrentUser();
            
            Assert.IsFalse(this._accountUpdateViewModel.UpdateConfirm.CanExecute(null));
        }

        [Test]
        public void TestAccountUpdateFormForCurrentCampingCustomerUser()
        {
            Assert.IsFalse(this._triggeredToUpdatePageEvent);
            this._accountViewModel.ToUpdate.Execute(null);
            Assert.IsTrue(this._triggeredToUpdatePageEvent);
            
            Assert.AreEqual("testFirstName", this._accountUpdateViewModel.FirstName);
            Assert.AreEqual("testLastName", this._accountUpdateViewModel.LastName);
            Assert.AreEqual("testPhoneNumber", this._accountUpdateViewModel.PhoneNumber);
            Assert.AreEqual("testAddress", this._accountUpdateViewModel.Street);
            Assert.AreEqual("customer", this._accountUpdateViewModel.Email);
            Assert.AreEqual(new DateTime(2021, 10, 19), this._accountUpdateViewModel.Birthdate);
        }
        
        [Test]
        public void TestAccountUpdateFormForCurrentCampingOwnerUser()
        {
            Account account = new Account("1", "admin", "nimda", "1");
            CampingOwner campingOwner = new CampingOwner("1", account, "Admin", "Nimda");

            CurrentUser.SetCurrentUser(account, campingOwner);
            this._accountViewModel.ToUpdate.Execute(null);
            
            Assert.IsNull(this._accountUpdateViewModel.Street);
            Assert.IsNull(this._accountUpdateViewModel.PostalCode);
            Assert.IsNull(this._accountUpdateViewModel.Place);
            Assert.AreEqual("Admin", this._accountUpdateViewModel.FirstName);
            Assert.AreEqual("Nimda", this._accountUpdateViewModel.LastName);
            Assert.IsNull(this._accountUpdateViewModel.PhoneNumber);
            Assert.AreEqual(DateTime.MinValue, this._accountUpdateViewModel.Birthdate);
            Assert.AreEqual("admin", this._accountUpdateViewModel.Email);
        }
        
        [Test]
        public void TestAccountUpdateFormFieldErrors()
        {
            this._accountUpdateViewModel.FirstName = "";
            Assert.AreEqual("Voornaam is een verplicht veld", this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.FirstName = "test";
            Assert.IsEmpty(this._accountUpdateViewModel.UpdateError);

            this._accountUpdateViewModel.LastName = "";
            Assert.AreEqual("Achternaam is een verplicht veld", this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.LastName = "test";
            Assert.IsEmpty(this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.PhoneNumber = "";
            Assert.AreEqual("Telefoonnummer is een verplicht veld", this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.PhoneNumber = "test";
            Assert.AreEqual("Ongeldig telefoonnummer", this._accountUpdateViewModel.UpdateError);

            this._accountUpdateViewModel.PhoneNumber = "0321345678";
            Assert.IsEmpty(this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.Street = "";
            Assert.AreEqual("Straatnaam is een verplicht veld", this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.Street = "test";
            Assert.IsEmpty(this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.PostalCode = "";
            Assert.AreEqual("Postcode is een verplicht veld", this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.PostalCode = "23";
            Assert.AreEqual("Ongeldig postcode", this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.PostalCode = "1234AB";
            Assert.IsEmpty(this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.Place = "";
            Assert.AreEqual("Plaatsnaam is een verplicht veld", this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.Place = "City";
            Assert.IsEmpty(this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.Birthdate = DateTime.Today;
            Assert.AreEqual("U bent te jong om te reserveren", this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.Birthdate = DateTime.Today.AddYears(-17);
            Assert.AreEqual("U bent te jong om te reserveren", this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.Birthdate = DateTime.Today.AddYears(-18).AddDays(-1);
            Assert.IsEmpty(this._accountUpdateViewModel.UpdateError);
            
            this._accountUpdateViewModel.Email = "test@test.com";
            Assert.IsEmpty(this._accountUpdateViewModel.UpdateError);
            
            Assert.IsTrue(this._accountUpdateViewModel.UpdateConfirm.CanExecute(null));
        }
    }
}