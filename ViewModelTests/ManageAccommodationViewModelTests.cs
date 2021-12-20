using System.Collections.Generic;
using System.Linq;
using Model;
using Moq;
using NUnit.Framework;
using ViewModel;

namespace ViewModelTests
{
    public class ManageAccommodationViewModelTest
    {
        private Mock<ManageAccommodationViewModel> _manageAccommodationViewModel;
        private List<Accommodation> _accommodations;

        [SetUp]
        public void Setup()
        {
            this._manageAccommodationViewModel = new Mock<ManageAccommodationViewModel>();
            this._accommodations = new List<Accommodation>
            {
                new Accommodation("CA", "Caravan"),
                new Accommodation("CH", "Chalet"),
                new Accommodation("CH", "Chalet"),
                new Accommodation("CA", "Camper"),
                new Accommodation("CA", "Camper"),
                new Accommodation("CA", "Camper")
            };
            
            this._manageAccommodationViewModel.Setup(x => x.GetAccommodations()).Returns(this._accommodations);
        }

        [Test]
        public void TestViewConstruction()
        {
            Assert.AreEqual(this._accommodations.Count, this._manageAccommodationViewModel.Object.Accommodations.Count);
            
            Assert.AreEqual("Caravan", this._manageAccommodationViewModel.Object.Accommodations.First().Name);
            
            Assert.IsNull(this._manageAccommodationViewModel.Object.SelectedAccommodation);
            Assert.IsNull(this._manageAccommodationViewModel.Object.Name);
            Assert.IsNull(this._manageAccommodationViewModel.Object.Prefix);
            Assert.IsNull(this._manageAccommodationViewModel.Object.AccommodationError);
            Assert.AreEqual("Accommodatie toevoegen", this._manageAccommodationViewModel.Object.EditTitle);
            Assert.IsFalse(this._manageAccommodationViewModel.Object.EditSave.CanExecute(null));
            Assert.IsFalse(this._manageAccommodationViewModel.Object.Delete.CanExecute(null));
        }

        [Test]
        public void TestSelectedAccommodation()
        {
            this._manageAccommodationViewModel.Object.SelectedAccommodation = this._accommodations.First();
            
            Assert.AreEqual("Accommodatie Caravan bewerken", this._manageAccommodationViewModel.Object.EditTitle);
            Assert.AreEqual("CA", this._manageAccommodationViewModel.Object.Prefix);
            Assert.AreEqual("Caravan", this._manageAccommodationViewModel.Object.Name);
            Assert.IsEmpty(this._manageAccommodationViewModel.Object.AccommodationError);
            Assert.IsTrue(this._manageAccommodationViewModel.Object.EditSave.CanExecute(null));
        }
        
        [Test]
        public void TestDeSelectedAccommodation()
        {
            this._manageAccommodationViewModel.Object.SelectedAccommodation = this._accommodations.First();;
            this._manageAccommodationViewModel.Object.SelectedAccommodation = null;
            
            Assert.AreEqual("Accommodatie toevoegen", this._manageAccommodationViewModel.Object.EditTitle);
            Assert.IsEmpty(this._manageAccommodationViewModel.Object.Prefix);
            Assert.IsEmpty(this._manageAccommodationViewModel.Object.Name);
            Assert.IsEmpty(this._manageAccommodationViewModel.Object.AccommodationError);
            Assert.IsFalse(this._manageAccommodationViewModel.Object.EditSave.CanExecute(null));
        }
        
        [Test]
        public void TestExecuteCancelAction()
        {
            this._manageAccommodationViewModel.Object.SelectedAccommodation = this._accommodations.First();;
            this._manageAccommodationViewModel.Object.CancelEditAction.Execute(null);
            
            Assert.AreEqual("Accommodatie toevoegen", this._manageAccommodationViewModel.Object.EditTitle);
            Assert.IsEmpty(this._manageAccommodationViewModel.Object.Prefix);
            Assert.IsEmpty(this._manageAccommodationViewModel.Object.Name);
            Assert.IsEmpty(this._manageAccommodationViewModel.Object.AccommodationError);
            Assert.IsFalse(this._manageAccommodationViewModel.Object.EditSave.CanExecute(null));
        }

        [Test]
        public void TestInputFieldErrors()
        {
            Assert.IsFalse(this._manageAccommodationViewModel.Object.EditSave.CanExecute(null));
            Assert.IsNull(this._manageAccommodationViewModel.Object.AccommodationError);

            this._manageAccommodationViewModel.Object.Prefix = "";
            Assert.AreEqual("Prefix is een verplicht veld", this._manageAccommodationViewModel.Object.AccommodationError);
            
            this._manageAccommodationViewModel.Object.Prefix = "HTH";
            Assert.AreEqual("Prefix mag maximaal 2 letters bevatten", this._manageAccommodationViewModel.Object.AccommodationError);
            
            this._manageAccommodationViewModel.Object.Prefix = "BH";
            Assert.IsEmpty(this._manageAccommodationViewModel.Object.AccommodationError);
            
            this._manageAccommodationViewModel.Object.Name = "";
            Assert.AreEqual("Naam is een verplicht veld", this._manageAccommodationViewModel.Object.AccommodationError);
            
            this._manageAccommodationViewModel.Object.Name = "Hotel";
            Assert.IsEmpty(this._manageAccommodationViewModel.Object.AccommodationError);
            
            Assert.IsEmpty(this._manageAccommodationViewModel.Object.AccommodationError);
            Assert.IsTrue(this._manageAccommodationViewModel.Object.EditSave.CanExecute(null));
        }
    }
}