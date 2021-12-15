using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ViewModel;
using Model;
using Moq;

namespace ViewModelTests
{
    /// <summary>
    /// Unittests for filtering and displaying reservations
    /// </summary>
    public class ReservationCollectionViewModel
    {
        private Mock<ReservationCollectionViewModel> _ReservationCollectionMockObject;
        private List<Accommodation> _campingPlaceTypeList;


        [SetUp]
        public void Setup()
        {
            this._campingPlaceTypeList = new List<Accommodation>
            {
                new Accommodation("CA", "Caravan"),
                new Accommodation("CH", "Chalet"),
                new Accommodation("BU", "Bungalow"),
                new Accommodation("TE", "Tent")
            };

            this._ReservationCollectionMockObject = new Mock<ReservationCollectionViewModel>();
            this._ReservationCollectionMockObject.Setup(x => x.SelectAccomodationTypes()).Returns(this._campingPlaceTypeList);




        }


        [Test]
        public void TestInitializeViewModel()
        {

        }
    }
}
