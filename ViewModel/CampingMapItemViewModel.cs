using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Model;

namespace ViewModel
{
    public class CampingMapItemViewModel : ObservableObject
    {
        #region Fields

        public const string
            SelectedColor = "#C8FFB3",
            UnselectedColor = "#FF68C948";
        
        private string _backgroundColor;

        private CampingPlace _campingPlace;

        #endregion

        #region Properties

        public int LocationNumber { get; private set; }
        public string ImageResource { get; private set; }

        public string BackgroundColor
        {
            get => this._backgroundColor;
            set
            {
                if (value == this._backgroundColor)
                {
                    return;
                }

                this._backgroundColor = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        public CampingPlace CampingPlace
        {
            get => this._campingPlace;
            set
            {
                this._campingPlace = value;

                this.ImageResource = ComponentsFolder + GetCampingFieldImage(null);
                if (this._campingPlace != null)
                {
                    this.ImageResource = ComponentsFolder + GetCampingFieldImage(this._campingPlace.Type.Accommodation);
                }

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        #endregion
        
        #region Graphics

        public const string
            ComponentsFolder = "/MapComponents/",
            AddButtonImage = "CampingFieldImage-Add.png",
            BungalowImage = "CampingFieldImage-Bungalow.png",
            CamperImage = "CampingFieldImage-Camper.png",
            CaravanImage = "CampingFieldImage-Caravan.png",
            ChaletImage = "CampingFieldImage-Chalet.png",
            TentImage = "CampingFieldImage-Tent.png",
            UnknownImage = "CampingFieldImage-Unknown.png";

        #endregion
        
        public CampingMapItemViewModel(CampingPlace campingPlace)
        {
            this.CampingPlace = campingPlace;
            this.LocationNumber = campingPlace.Number;
            this.BackgroundColor = UnselectedColor;
        }

        public CampingMapItemViewModel(int locationNumber, string backgroundColor, string imageResource)
        {
            this.LocationNumber = locationNumber;
            this.BackgroundColor = backgroundColor;
            this.ImageResource = ComponentsFolder + imageResource;
        }

        public void Update(CampingPlace campingPlace)
        {
            this.CampingPlace = campingPlace;
        }
        
        private static string GetCampingFieldImage(Accommodation accommodation)
        {
            if (accommodation == null)
            {
                return AddButtonImage;
            }
            
            return accommodation.Type switch
            {
                AccommodationTypes.Bungalow => BungalowImage,
                AccommodationTypes.Camper => CamperImage,
                AccommodationTypes.Caravan => CaravanImage,
                AccommodationTypes.Chalet => ChaletImage,
                AccommodationTypes.Tent => TentImage,
                AccommodationTypes.Unknown => UnknownImage,
                _ => UnknownImage
            };
        }
    }
}
