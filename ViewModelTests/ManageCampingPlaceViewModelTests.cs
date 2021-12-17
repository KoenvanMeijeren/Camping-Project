using System.Collections.Generic;
using System.Linq;
using Model;
using Moq;
using NUnit.Framework;
using ViewModel;

namespace ViewModelTests
{
    public class ManageCampingPlaceViewModelTests
    {
        private Mock<ManageCampingPlaceViewModel> _manageCampingPlaceViewModel;
        private List<CampingPlace> _campingPlaces;
        private List<CampingPlaceType> _campingPlaceTypes;

        [SetUp]
        public void Setup()
        {
            this._manageCampingPlaceViewModel = new Mock<ManageCampingPlaceViewModel>();
            this._campingPlaces = new List<CampingPlace>
            {
                new CampingPlace("3", "1", "80", "0", new CampingPlaceType("5", "40", new Accommodation("CA", "Caravan"))),
                new CampingPlace("4", "1", "80", "30", new CampingPlaceType("2", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("5", "1", "80", "30", new CampingPlaceType("10", "80", new Accommodation("CH", "Chalet"))),
                new CampingPlace("6", "1", "80", "10", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("7", "1", "80", "10", new CampingPlaceType("1", "50", new Accommodation("CA", "Camper"))),
                new CampingPlace("8", "1", "80", "0", new CampingPlaceType("2", "50", new Accommodation("CA", "Camper")))
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

            this._manageCampingPlaceViewModel.Setup(x => x.GetCampingPlaces()).Returns(this._campingPlaces);
            this._manageCampingPlaceViewModel.Setup(x => x.GetCampingPlaceTypes()).Returns(this._campingPlaceTypes);
        }

        [Test]
        public void TestViewConstruction()
        {
            Assert.AreEqual(this._campingPlaces.Count, this._manageCampingPlaceViewModel.Object.CampingPlaces.Count);
            Assert.AreEqual(this._campingPlaceTypes.Count, this._manageCampingPlaceViewModel.Object.CampingPlaceTypes.Count);
            
            Assert.AreEqual("Caravan", this._manageCampingPlaceViewModel.Object.CampingPlaces.First().Type.Accommodation.Name);
            
            Assert.IsNull(this._manageCampingPlaceViewModel.Object.SelectedCampingPlace);
            Assert.IsNull(this._manageCampingPlaceViewModel.Object.SelectedCampingPlaceType);
            Assert.IsNull(this._manageCampingPlaceViewModel.Object.Number);
            Assert.IsNull(this._manageCampingPlaceViewModel.Object.Surface);
            Assert.IsNull(this._manageCampingPlaceViewModel.Object.ExtraNightPrice);
            Assert.IsNull(this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            Assert.AreEqual("Campingplaats toevoegen", this._manageCampingPlaceViewModel.Object.EditTitle);
            Assert.IsFalse(this._manageCampingPlaceViewModel.Object.EditSave.CanExecute(null));
            Assert.IsFalse(this._manageCampingPlaceViewModel.Object.Delete.CanExecute(null));
        }

        [Test]
        public void TestSelectedCampingPlace()
        {
            this._manageCampingPlaceViewModel.Object.SelectedCampingPlace = this._campingPlaces.First();
            
            Assert.AreEqual("Campingplaats CA-1 bewerken", this._manageCampingPlaceViewModel.Object.EditTitle);
            Assert.AreEqual("1", this._manageCampingPlaceViewModel.Object.Number);
            Assert.AreEqual("80", this._manageCampingPlaceViewModel.Object.Surface);
            Assert.AreEqual("0", this._manageCampingPlaceViewModel.Object.ExtraNightPrice);
            Assert.AreEqual(this._campingPlaceTypes.First(), this._manageCampingPlaceViewModel.Object.SelectedCampingPlaceType);
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            Assert.IsTrue(this._manageCampingPlaceViewModel.Object.EditSave.CanExecute(null));
        }
        
        [Test]
        public void TestDeselectedCampingPlace()
        {
            this._manageCampingPlaceViewModel.Object.SelectedCampingPlace = this._campingPlaces.First();;
            this._manageCampingPlaceViewModel.Object.SelectedCampingPlace = null;
            
            Assert.AreEqual("Campingplaats toevoegen", this._manageCampingPlaceViewModel.Object.EditTitle);
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.Number);
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.Surface);
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.ExtraNightPrice);
            Assert.IsNull(this._manageCampingPlaceViewModel.Object.SelectedCampingPlaceType);
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            Assert.IsFalse(this._manageCampingPlaceViewModel.Object.EditSave.CanExecute(null));
        }
        
        [Test]
        public void TestExecuteCancelAction()
        {
            this._manageCampingPlaceViewModel.Object.SelectedCampingPlace = this._campingPlaces.First();;
            this._manageCampingPlaceViewModel.Object.Cancel.Execute(null);
            
            Assert.AreEqual("Campingplaats toevoegen", this._manageCampingPlaceViewModel.Object.EditTitle);
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.Number);
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.Surface);
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.ExtraNightPrice);
            Assert.IsNull(this._manageCampingPlaceViewModel.Object.SelectedCampingPlaceType);
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            Assert.IsFalse(this._manageCampingPlaceViewModel.Object.EditSave.CanExecute(null));
        }

        [Test]
        public void TestInputFieldErrors()
        {
            Assert.IsFalse(this._manageCampingPlaceViewModel.Object.EditSave.CanExecute(null));
            Assert.IsNull(this._manageCampingPlaceViewModel.Object.CampingPlaceError);

            this._manageCampingPlaceViewModel.Object.Number = "";
            Assert.AreEqual("Locatienummer is een verplicht veld", this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            
            this._manageCampingPlaceViewModel.Object.Number = "fdafda";
            Assert.AreEqual("Ongeldig locatienummer opgegeven", this._manageCampingPlaceViewModel.Object.CampingPlaceError);

            this._manageCampingPlaceViewModel.Object.Number = "23";
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            
            this._manageCampingPlaceViewModel.Object.Surface = "";
            Assert.AreEqual("Oppervlakte is een verplicht veld", this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            
            this._manageCampingPlaceViewModel.Object.Surface = "fdafda";
            Assert.AreEqual("Ongeldig oppervlakte opgegeven", this._manageCampingPlaceViewModel.Object.CampingPlaceError);

            this._manageCampingPlaceViewModel.Object.Surface = "23";
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            
            this._manageCampingPlaceViewModel.Object.ExtraNightPrice = "";
            Assert.AreEqual("Extra dagtarief is een verplicht veld", this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            
            this._manageCampingPlaceViewModel.Object.ExtraNightPrice = "fdafda";
            Assert.AreEqual("Ongeldig extra dagtarief opgegeven", this._manageCampingPlaceViewModel.Object.CampingPlaceError);

            this._manageCampingPlaceViewModel.Object.ExtraNightPrice = "23";
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.CampingPlaceError);

            this._manageCampingPlaceViewModel.Object.SelectedCampingPlaceType = this._campingPlaceTypes.First();
            this._manageCampingPlaceViewModel.Object.SelectedCampingPlaceType = null;
            Assert.AreEqual("Verblijf is een verplicht veld", this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            
            this._manageCampingPlaceViewModel.Object.SelectedCampingPlaceType = this._campingPlaceTypes.First();
            Assert.IsEmpty(this._manageCampingPlaceViewModel.Object.CampingPlaceError);
            Assert.IsTrue(this._manageCampingPlaceViewModel.Object.EditSave.CanExecute(null));
        }
    }
}