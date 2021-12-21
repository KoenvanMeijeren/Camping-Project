using System.Collections.Generic;
using System.Linq;
using Model;
using Moq;
using NUnit.Framework;
using ViewModel;

namespace ViewModelTests
{
    public class ManageCampingPlaceTypeViewModelTests
    {
        private Mock<ManageCampingPlaceTypeViewModel> _manageCampingPlaceTypeViewModel;
        private List<Accommodation> _accommodations;
        private List<CampingPlaceType> _campingPlaceTypes;

        [SetUp]
        public void Setup()
        {
            this._manageCampingPlaceTypeViewModel = new Mock<ManageCampingPlaceTypeViewModel>();
            this._accommodations = new List<Accommodation>
            {
                new Accommodation("CA", "Caravan"),
                new Accommodation("CH", "Chalet"),
                new Accommodation("CH", "Chalet"),
                new Accommodation("CA", "Camper"),
                new Accommodation("CA", "Camper"),
                new Accommodation("CA", "Camper")
            };

            this._campingPlaceTypes = new List<CampingPlaceType>
            {
                new CampingPlaceType("5", "40", new Accommodation("CA", "Caravan")),
                new CampingPlaceType("2", "80", new Accommodation("CH", "Chalet")),
                new CampingPlaceType("10", "80", new Accommodation("CH", "Chalet")),
                new CampingPlaceType("2", "50", new Accommodation("CA", "Camper")),
                new CampingPlaceType("1", "50", new Accommodation("CA", "Camper")),
                new CampingPlaceType("2", "50", new Accommodation("CA", "Camper"))
            };

            this._manageCampingPlaceTypeViewModel.Setup(x => x.GetAccommodations()).Returns(this._accommodations);
            this._manageCampingPlaceTypeViewModel.Setup(x => x.GetCampingPlaceTypes()).Returns(this._campingPlaceTypes);
        }

        [Test]
        public void TestViewConstruction()
        {
            Assert.AreEqual(this._accommodations.Count, this._manageCampingPlaceTypeViewModel.Object.Accommodations.Count);
            Assert.AreEqual(this._campingPlaceTypes.Count, this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypes.Count);
            
            Assert.AreEqual("Caravan", this._manageCampingPlaceTypeViewModel.Object.Accommodations.First().Name);
            
            Assert.IsNull(this._manageCampingPlaceTypeViewModel.Object.SelectedAccommodation);
            Assert.IsNull(this._manageCampingPlaceTypeViewModel.Object.SelectedCampingPlaceType);
            Assert.IsNull(this._manageCampingPlaceTypeViewModel.Object.GuestLimit);
            Assert.IsNull(this._manageCampingPlaceTypeViewModel.Object.StandardNightPrice);
            Assert.IsNull(this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);
            Assert.AreEqual("Campingplaatstype toevoegen", this._manageCampingPlaceTypeViewModel.Object.EditTitle);
            Assert.IsFalse(this._manageCampingPlaceTypeViewModel.Object.EditSave.CanExecute(null));
            Assert.IsFalse(this._manageCampingPlaceTypeViewModel.Object.Delete.CanExecute(null));
        }

        [Test]
        public void TestSelectedCampingPlaceType()
        {
            this._manageCampingPlaceTypeViewModel.Object.SelectedCampingPlaceType = this._campingPlaceTypes.First();
            
            Assert.AreEqual("Campingplaatstype Caravan (-1) bewerken", this._manageCampingPlaceTypeViewModel.Object.EditTitle);
            Assert.AreEqual("5", this._manageCampingPlaceTypeViewModel.Object.GuestLimit);
            Assert.AreEqual("40", this._manageCampingPlaceTypeViewModel.Object.StandardNightPrice);
            Assert.AreEqual(this._campingPlaceTypes.First(), this._manageCampingPlaceTypeViewModel.Object.SelectedCampingPlaceType);
            Assert.IsEmpty(this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);
            Assert.IsTrue(this._manageCampingPlaceTypeViewModel.Object.EditSave.CanExecute(null));
        }
        
        [Test]
        public void TestDeselectedCampingPlaceType()
        {
            this._manageCampingPlaceTypeViewModel.Object.SelectedCampingPlaceType = this._campingPlaceTypes.First();;
            this._manageCampingPlaceTypeViewModel.Object.SelectedCampingPlaceType = null;
            
            Assert.AreEqual("Campingplaatstype toevoegen", this._manageCampingPlaceTypeViewModel.Object.EditTitle);
            Assert.IsEmpty(this._manageCampingPlaceTypeViewModel.Object.GuestLimit);
            Assert.IsEmpty(this._manageCampingPlaceTypeViewModel.Object.StandardNightPrice);
            Assert.IsNull(this._manageCampingPlaceTypeViewModel.Object.SelectedCampingPlaceType);
            Assert.IsEmpty(this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);
            Assert.IsFalse(this._manageCampingPlaceTypeViewModel.Object.EditSave.CanExecute(null));
        }
        
        [Test]
        public void TestExecuteCancelAction()
        {
            this._manageCampingPlaceTypeViewModel.Object.SelectedCampingPlaceType = this._campingPlaceTypes.First();;
            this._manageCampingPlaceTypeViewModel.Object.CancelEditAction.Execute(null);
            
            Assert.AreEqual("Campingplaatstype toevoegen", this._manageCampingPlaceTypeViewModel.Object.EditTitle);
            Assert.IsEmpty(this._manageCampingPlaceTypeViewModel.Object.GuestLimit);
            Assert.IsEmpty(this._manageCampingPlaceTypeViewModel.Object.StandardNightPrice);
            Assert.IsNull(this._manageCampingPlaceTypeViewModel.Object.SelectedCampingPlaceType);
            Assert.IsEmpty(this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);
            Assert.IsFalse(this._manageCampingPlaceTypeViewModel.Object.EditSave.CanExecute(null));
        }

        [Test]
        public void TestInputFieldErrors()
        {
            Assert.IsFalse(this._manageCampingPlaceTypeViewModel.Object.EditSave.CanExecute(null));
            Assert.IsNull(this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);

            this._manageCampingPlaceTypeViewModel.Object.GuestLimit = "";
            Assert.AreEqual("Gasten limiet is een verplicht veld", this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);
            
            this._manageCampingPlaceTypeViewModel.Object.GuestLimit = "fdafda";
            Assert.AreEqual("Ongeldig gasten limiet opgegeven", this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);

            this._manageCampingPlaceTypeViewModel.Object.GuestLimit = "23";
            Assert.IsEmpty(this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);
            
            this._manageCampingPlaceTypeViewModel.Object.StandardNightPrice = "";
            Assert.AreEqual("Dagtarief is een verplicht veld", this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);
            
            this._manageCampingPlaceTypeViewModel.Object.StandardNightPrice = "fdafda";
            Assert.AreEqual("Ongeldig dagtarief opgegeven", this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);

            this._manageCampingPlaceTypeViewModel.Object.StandardNightPrice = "23";
            Assert.IsEmpty(this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);

            this._manageCampingPlaceTypeViewModel.Object.SelectedAccommodation = this._accommodations.First();
            this._manageCampingPlaceTypeViewModel.Object.SelectedAccommodation = null;
            Assert.AreEqual("Accommodatie is een verplicht veld", this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);
            
            this._manageCampingPlaceTypeViewModel.Object.SelectedAccommodation = this._accommodations.First();
            Assert.IsEmpty(this._manageCampingPlaceTypeViewModel.Object.CampingPlaceTypeError);
            Assert.IsTrue(this._manageCampingPlaceTypeViewModel.Object.EditSave.CanExecute(null));
        }
    }
}