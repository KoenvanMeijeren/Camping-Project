using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Moq;
using NUnit.Framework;
using ViewModel;

namespace ViewModelTests
{
    public class ManageCampingCustomerViewModelTests
    {
        private Mock<ManageCampingCustomerViewModel> _manageCampingCustomerViewModel;
        private List<CampingCustomer> _campingCustomers;

        [SetUp]
        public void Setup()
        {
            this._manageCampingCustomerViewModel = new Mock<ManageCampingCustomerViewModel>();

            Account account = new Account("1", "admin", "nimda", "0");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            this._campingCustomers = new List<CampingCustomer>
            {
                new CampingCustomer("1", account, address, "19/10/21", "0341124354", "John", "Doe"),
                new CampingCustomer("2", account, address, "10/10/15", "0341124354", "Teddy", "Pleiter"),
                new CampingCustomer("3", account, address, "19/03/19", "0341124354", "Jessica", "Dijksma"),
                new CampingCustomer("4", account, address, "23/03/16", "0341124354", "Jessica", "Pleiter"),
            };

            this._manageCampingCustomerViewModel.Setup(x => x.GetCampingCustomers()).Returns(this._campingCustomers);
        }

        [Test]
        public void TestViewConstruction()
        {
            Assert.AreEqual(this._campingCustomers.Count, this._manageCampingCustomerViewModel.Object.CampingCustomers.Count);
            
            Assert.AreEqual("John Doe", this._manageCampingCustomerViewModel.Object.CampingCustomers.First().FullName);
            
            Assert.IsNull(this._manageCampingCustomerViewModel.Object.SelectedCampingCustomer);
            Assert.IsNull(this._manageCampingCustomerViewModel.Object.FirstName);
            Assert.IsNull(this._manageCampingCustomerViewModel.Object.LastName);
            Assert.AreEqual(DateTime.MinValue, this._manageCampingCustomerViewModel.Object.Birthdate);
            Assert.IsNull(this._manageCampingCustomerViewModel.Object.PhoneNumber);
            Assert.IsNull(this._manageCampingCustomerViewModel.Object.Street);
            Assert.IsNull(this._manageCampingCustomerViewModel.Object.PostalCode);
            Assert.IsNull(this._manageCampingCustomerViewModel.Object.Place);
            Assert.IsNull(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            Assert.AreEqual("Campingklant toevoegen", this._manageCampingCustomerViewModel.Object.EditTitle);
            Assert.IsFalse(this._manageCampingCustomerViewModel.Object.EditSave.CanExecute(null));
            Assert.IsFalse(this._manageCampingCustomerViewModel.Object.Delete.CanExecute(null));
        }

        [Test]
        public void TestSelectedCampingCustomer()
        {
            this._manageCampingCustomerViewModel.Object.SelectedCampingCustomer = this._campingCustomers.First();
            
            Assert.AreEqual("Campingklant John Doe bewerken", this._manageCampingCustomerViewModel.Object.EditTitle);
            Assert.AreEqual("John", this._manageCampingCustomerViewModel.Object.FirstName);
            Assert.AreEqual("Doe", this._manageCampingCustomerViewModel.Object.LastName);
            Assert.AreEqual(new DateTime(2021, 10, 19), this._manageCampingCustomerViewModel.Object.Birthdate);
            Assert.AreEqual("0341124354", this._manageCampingCustomerViewModel.Object.PhoneNumber);
            Assert.AreEqual("testAddress", this._manageCampingCustomerViewModel.Object.Street);
            Assert.AreEqual("testPostalCode", this._manageCampingCustomerViewModel.Object.PostalCode);
            Assert.AreEqual("testPlace", this._manageCampingCustomerViewModel.Object.Place);
            Assert.IsNull(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            Assert.IsTrue(this._manageCampingCustomerViewModel.Object.EditSave.CanExecute(null));
        }
        
        [Test]
        public void TestDeSelectedCampingCustomer()
        {
            this._manageCampingCustomerViewModel.Object.SelectedCampingCustomer = this._campingCustomers.First();;
            this._manageCampingCustomerViewModel.Object.SelectedCampingCustomer = null;
            
            Assert.AreEqual("Campingklant toevoegen", this._manageCampingCustomerViewModel.Object.EditTitle);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.FirstName);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.LastName);
            Assert.AreEqual(DateTime.MinValue, this._manageCampingCustomerViewModel.Object.Birthdate);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.PhoneNumber);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.Street);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.PostalCode);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.Place);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            Assert.IsFalse(this._manageCampingCustomerViewModel.Object.EditSave.CanExecute(null));
        }
        
        [Test]
        public void TestExecuteCancelAction()
        {
            this._manageCampingCustomerViewModel.Object.SelectedCampingCustomer = this._campingCustomers.First();;
            this._manageCampingCustomerViewModel.Object.CancelEditAction.Execute(null);
            
            Assert.AreEqual("Campingklant toevoegen", this._manageCampingCustomerViewModel.Object.EditTitle);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.FirstName);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.LastName);
            Assert.AreEqual(DateTime.MinValue, this._manageCampingCustomerViewModel.Object.Birthdate);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.PhoneNumber);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.Street);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.PostalCode);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.Place);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            Assert.IsFalse(this._manageCampingCustomerViewModel.Object.EditSave.CanExecute(null));
        }

        [Test]
        public void TestInputFieldErrors()
        {
            Assert.IsFalse(this._manageCampingCustomerViewModel.Object.EditSave.CanExecute(null));
            Assert.IsNull(this._manageCampingCustomerViewModel.Object.CampingCustomerError);

            this._manageCampingCustomerViewModel.Object.FirstName = "";
            Assert.AreEqual("Voornaam is een verplicht veld", this._manageCampingCustomerViewModel.Object.CampingCustomerError);

            this._manageCampingCustomerViewModel.Object.FirstName = "John";
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            
            this._manageCampingCustomerViewModel.Object.LastName = "";
            Assert.AreEqual("Achternaam is een verplicht veld", this._manageCampingCustomerViewModel.Object.CampingCustomerError);

            this._manageCampingCustomerViewModel.Object.LastName = "Doe";
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            
            this._manageCampingCustomerViewModel.Object.Birthdate = DateTime.Today.AddYears(-16);
            Assert.AreEqual("U bent te jong om te reserveren", this._manageCampingCustomerViewModel.Object.CampingCustomerError);

            this._manageCampingCustomerViewModel.Object.Birthdate = DateTime.Today.AddYears(-18);
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            
            this._manageCampingCustomerViewModel.Object.PhoneNumber = "";
            Assert.AreEqual("Telefoonnummer is een verplicht veld", this._manageCampingCustomerViewModel.Object.CampingCustomerError);

            this._manageCampingCustomerViewModel.Object.PhoneNumber = "fdasfdas";
            Assert.AreEqual("Ongeldig telefoonnummer", this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            
            this._manageCampingCustomerViewModel.Object.PhoneNumber = "0341124354";
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            
            this._manageCampingCustomerViewModel.Object.Street = "";
            Assert.AreEqual("Straatnaam is een verplicht veld", this._manageCampingCustomerViewModel.Object.CampingCustomerError);

            this._manageCampingCustomerViewModel.Object.Street = "Way 1";
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            
            this._manageCampingCustomerViewModel.Object.PostalCode = "";
            Assert.AreEqual("Postcode is een verplicht veld", this._manageCampingCustomerViewModel.Object.CampingCustomerError);

            this._manageCampingCustomerViewModel.Object.PostalCode = "1224BH";
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            
            this._manageCampingCustomerViewModel.Object.Place = "";
            Assert.AreEqual("Plaatsnaam is een verplicht veld", this._manageCampingCustomerViewModel.Object.CampingCustomerError);

            this._manageCampingCustomerViewModel.Object.Place = "City";
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            
            Assert.IsEmpty(this._manageCampingCustomerViewModel.Object.CampingCustomerError);
            Assert.IsTrue(this._manageCampingCustomerViewModel.Object.EditSave.CanExecute(null));
        }
    }
}