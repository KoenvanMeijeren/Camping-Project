using Model;
using NUnit.Framework;
using ViewModel;

namespace ViewModelTests
{
    public class CampingMapItemViewModelTests
    {

        [Test]
        public void TestCampingMapItemConstruction()
        {
            var campingMapItemViewModel = new CampingMapItemViewModel(1, CampingMapItemViewModel.SelectedColor,
                CampingMapItemViewModel.BungalowImage);
            
            Assert.AreEqual(1, campingMapItemViewModel.LocationNumber);
            Assert.AreEqual(CampingMapItemViewModel.SelectedColor, campingMapItemViewModel.BackgroundColor);
            Assert.AreEqual(CampingMapItemViewModel.ComponentsFolder + CampingMapItemViewModel.BungalowImage, campingMapItemViewModel.ImageResource);
        }

        [Test]
        public void TestCampingMapItemConstructionFromCampingPlace()
        {
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", new Accommodation( "testPrefix", "Chalet"));
            CampingPlace campingPlace = new CampingPlace("12", "1", "1", campingPlaceType);
            var campingMapItemViewModel = new CampingMapItemViewModel(campingPlace);
            
            Assert.AreEqual(12, campingMapItemViewModel.LocationNumber);
            Assert.AreEqual(CampingMapItemViewModel.UnselectedColor, campingMapItemViewModel.BackgroundColor);
            Assert.AreEqual(CampingMapItemViewModel.ComponentsFolder + CampingMapItemViewModel.ChaletImage, campingMapItemViewModel.ImageResource);
        }
        
        [Test]
        public void TestUpdateCampingPlace()
        {
            CampingPlaceType campingPlaceType = new CampingPlaceType("1", "1", "1", new Accommodation( "testPrefix", "Chalet"));
            CampingPlaceType campingPlaceType2 = new CampingPlaceType("1", "1", "1", new Accommodation( "testPrefix", "Bungalow"));
            CampingPlace campingPlace = new CampingPlace("12", "1", "1", campingPlaceType);
            var campingMapItemViewModel = new CampingMapItemViewModel(campingPlace);
            campingMapItemViewModel.Update(new CampingPlace("123", "1", "1", campingPlaceType2));
            
            Assert.AreEqual(12, campingMapItemViewModel.LocationNumber);
            Assert.AreEqual(CampingMapItemViewModel.UnselectedColor, campingMapItemViewModel.BackgroundColor);
            Assert.AreEqual(CampingMapItemViewModel.ComponentsFolder + CampingMapItemViewModel.BungalowImage, campingMapItemViewModel.ImageResource);
        }
    }
}