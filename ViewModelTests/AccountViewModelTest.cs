using System;
using Model;
using Moq;
using NUnit.Framework;
using ViewModel;

namespace ViewModelTests
{
    public class AccountViewModelTest
    {
        private AccountViewModel _accountViewModel;

        private bool _triggeredSignOutEvent;
        
        [SetUp]
        public void Setup()
        {
            this._accountViewModel = new AccountViewModel();
            
            AccountViewModel.SignOutEvent += AccountViewModelOnSignOutEvent;
        }

        private void AccountViewModelOnSignOutEvent(object? sender, EventArgs e)
        {
            this._triggeredSignOutEvent = true;
        }

        [Test]
        public void TestTheCampingCustomerOverview()
        {
            Account account = new Account("1", "customer", "nimda", "0");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/21", "testPhoneNumber", "testFirstName", "testLastName");

            CurrentUser.SetCurrentUser(account, campingCustomer);
            
            Assert.AreEqual("testPostalCode testPlace", this._accountViewModel.Address);
            Assert.AreEqual("testFirstName testLastName", this._accountViewModel.Name);
            Assert.AreEqual("testPhoneNumber", this._accountViewModel.PhoneNumber);
            Assert.AreEqual("testAddress", this._accountViewModel.Street);
            Assert.AreEqual("customer", this._accountViewModel.Mail);
            Assert.AreEqual("19-10-2021", this._accountViewModel.Birthdate);
        }
        
        [Test]
        public void TestTheCampingOwnerOverview()
        {
            Account account = new Account("1", "admin", "nimda", "1");
            CampingOwner campingOwner = new CampingOwner("1", account, "Admin", "Nimda");

            CurrentUser.SetCurrentUser(account, campingOwner);
            
            Assert.IsNull(this._accountViewModel.Address);
            Assert.AreEqual("Admin Nimda", this._accountViewModel.Name);
            Assert.IsEmpty(this._accountViewModel.PhoneNumber);
            Assert.IsEmpty(this._accountViewModel.Street);
            Assert.IsEmpty(this._accountViewModel.Birthdate);
            Assert.AreEqual("admin", this._accountViewModel.Mail);
        }

        [Test]
        public void TestNoCurrentUser()
        {
            CurrentUser.EmptyCurrentUser();
            
            Assert.IsNull(this._accountViewModel.Address);
            Assert.IsNull(this._accountViewModel.Name);
            Assert.IsNull(this._accountViewModel.PhoneNumber);
            Assert.IsNull(this._accountViewModel.Street);
            Assert.IsNull(this._accountViewModel.Mail);
            Assert.IsNull(this._accountViewModel.Birthdate);
        }
        
        [Test]
        public void TestSignOutCurrentUser()
        {
            Assert.IsFalse(this._triggeredSignOutEvent);
            this._accountViewModel.SignOut.Execute(null);
            Assert.IsTrue(this._triggeredSignOutEvent);
            
            Assert.IsNull(this._accountViewModel.Address);
            Assert.IsNull(this._accountViewModel.Name);
            Assert.IsNull(this._accountViewModel.PhoneNumber);
            Assert.IsNull(this._accountViewModel.Street);
            Assert.IsNull(this._accountViewModel.Mail);
            Assert.IsNull(this._accountViewModel.Birthdate);
        }
    }
}