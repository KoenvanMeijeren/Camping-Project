using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemCore;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ManageCampingMapViewModel : ObservableObject
    {
        private Dictionary<int, CampingField> CampingFields { get; set; }
        public CampingField SelectedCampingField { get; set; }
        
        #region CampingField Fields

        private CampingField
        _campingField1,
        _campingField2,
        _campingField3,
        _campingField4,
        _campingField5,
        _campingField6,
        _campingField7,
        _campingField8,
        _campingField9,
        _campingField10,
        _campingField11,
        _campingField12,
        _campingField13,
        _campingField14,
        _campingField15,
        _campingField16,
        _campingField17,
        _campingField18,
        _campingField19,
        _campingField20,
        _campingField21,
        _campingField22,
        _campingField23,
        _campingField24,
        _campingField25,
        _campingField26,
        _campingField27,
        _campingField28,
        _campingField29,
        _campingField30,
        _campingField31,
        _campingField32,
        _campingField33,
        _campingField34;
        #endregion

        #region CampingField Properties
        public CampingField CampingField1
        {
            get => this._campingField1;
            set
            {
                this._campingField1 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField2
        {
            get => this._campingField2;
            set
            {
                this._campingField2 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField3
        {
            get => this._campingField3;
            set
            {
                this._campingField3 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField4
        {
            get => this._campingField4;
            set
            {
                this._campingField4 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField5
        {
            get => this._campingField5;
            set
            {
                this._campingField5 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField6
        {
            get => this._campingField6;
            set
            {
                this._campingField6 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField7
        {
            get => this._campingField7;
            set
            {
                this._campingField7 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField8
        {
            get => this._campingField8;
            set
            {
                this._campingField8 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField9
        {
            get => this._campingField9;
            set
            {
                this._campingField9 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField10
        {
            get => this._campingField10;
            set
            {
                this._campingField10 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField11
        {
            get => this._campingField11;
            set
            {
                this._campingField11 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField12
        {
            get => this._campingField12;
            set
            {
                this._campingField12 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField13
        {
            get => this._campingField13;
            set
            {
                this._campingField13 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField14
        {
            get => this._campingField14;
            set
            {
                this._campingField14 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField15
        {
            get => this._campingField15;
            set
            {
                this._campingField15 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField16
        {
            get => this._campingField16;
            set
            {
                this._campingField16 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField17
        {
            get => this._campingField17;
            set
            {
                this._campingField17 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField18
        {
            get => this._campingField18;
            set
            {
                this._campingField18 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField19
        {
            get => this._campingField19;
            set
            {
                this._campingField19 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField20
        {
            get => this._campingField20;
            set
            {
                this._campingField20 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField21
        {
            get => this._campingField21;
            set
            {
                this._campingField21 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField22
        {
            get => this._campingField22;
            set
            {
                this._campingField22 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField23
        {
            get => this._campingField23;
            set
            {
                this._campingField23 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField24
        {
            get => this._campingField24;
            set
            {
                this._campingField24 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField25
        {
            get => this._campingField25;
            set
            {
                this._campingField25 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField26
        {
            get => this._campingField26;
            set
            {
                this._campingField26 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField27
        {
            get => this._campingField27;
            set
            {
                this._campingField27 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField28
        {
            get => this._campingField28;
            set
            {
                this._campingField28 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField29
        {
            get => this._campingField29;
            set
            {
                this._campingField29 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField30
        {
            get => this._campingField30;
            set
            {
                this._campingField30 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField31
        {
            get => this._campingField31;
            set
            {
                this._campingField31 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField32
        {
            get => this._campingField32;
            set
            {
                this._campingField32 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField33
        {
            get => this._campingField33;
            set
            {
                this._campingField33 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingField CampingField34
        {
            get => this._campingField34;
            set
            {
                this._campingField34 = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        #endregion

        #region Graphics

        private const string
            ComponentsFolder = "/MapComponents/",
            AddButtonImage = "CampingFieldImage-Add.png",
            BungalowImage = "CampingFieldImage-Bungalow.png",
            CamperImage = "CampingFieldImage-Camper.png",
            CaravanImage = "CampingFieldImage-Caravan.png",
            ChaletImage = "CampingFieldImage-Chalet.png",
            TentImage = "CampingFieldImage-Tent.png",
            UnknownImage = "CampingFieldImage-Unknown.png";

        #endregion

        #region View construction

        public ManageCampingMapViewModel()
        {
            this.FillCampingFieldColors();
            this.SetFields();
        }

        private void FillCampingFieldColors()
        {
            this.CampingFields = new Dictionary<int, CampingField>
            {
                {1, this.CampingField1 = new CampingField(1, "#FF68C948", null)},
                {2, this.CampingField2 = new CampingField(2, "#FF68C948", null)},
                {3, this.CampingField3 = new CampingField(3, "#FF68C948", null)},
                {4, this.CampingField4 = new CampingField(4, "#FF68C948", null)},
                {5, this.CampingField5 = new CampingField(5, "#FF68C948", null)},
                {6, this.CampingField6 = new CampingField(6, "#FF68C948", null)},
                {7, this.CampingField7 = new CampingField(7, "#FF68C948", null)},
                {8, this.CampingField8 = new CampingField(8, "#FF68C948", null)},
                {9, this.CampingField9 = new CampingField(9, "#FF68C948", null)},
                {10, this.CampingField10 = new CampingField(10, "#FF68C948", null)},
                {11, this.CampingField11 = new CampingField(11, "#FF68C948", null)},
                {12, this.CampingField12 = new CampingField(12, "#FF68C948", null)},
                {13, this.CampingField13 = new CampingField(13, "#FF68C948", null)},
                {14, this.CampingField14 = new CampingField(14, "#FF68C948", null)},
                {15, this.CampingField15 = new CampingField(15, "#FF68C948", null)},
                {16, this.CampingField16 = new CampingField(16, "#FF68C948", null)},
                {17, this.CampingField17 = new CampingField(17, "#FF68C948", null)},
                {18, this.CampingField18 = new CampingField(18, "#FF68C948", null)},
                {19, this.CampingField19 = new CampingField(19, "#FF68C948", null)},
                {20, this.CampingField20 = new CampingField(20, "#FF68C948", null)},
                {21, this.CampingField21 = new CampingField(21, "#FF68C948", null)},
                {22, this.CampingField22 = new CampingField(22, "#FF68C948", null)},
                {23, this.CampingField23 = new CampingField(23, "#FF68C948", null)},
                {24, this.CampingField24 = new CampingField(24, "#FF68C948", null)},
                {25, this.CampingField25 = new CampingField(25, "#FF68C948", null)},
                {26, this.CampingField26 = new CampingField(26, "#FF68C948", null)},
                {27, this.CampingField27 = new CampingField(27, "#FF68C948", null)},
                {28, this.CampingField28 = new CampingField(28, "#FF68C948", null)},
                {29, this.CampingField29 = new CampingField(29, "#FF68C948", null)},
                {30, this.CampingField30 = new CampingField(30, "#FF68C948", null)},
                {31, this.CampingField31 = new CampingField(31, "#FF68C948", null)},
                {32, this.CampingField32 = new CampingField(32, "#FF68C948", null)},
                {33, this.CampingField33 = new CampingField(33, "#FF68C948", null)},
                {34, this.CampingField34 = new CampingField(34, "#FF68C948", null)}
            };
        }

        private void SetFields()
        {
            CampingPlace emptyCampingPlace = new CampingPlace();

            foreach (CampingField campingField in this.CampingFields.Values)
            {
                var campingPlace = emptyCampingPlace.SelectByPlaceNumber(campingField.LocationNumber);
                if (campingPlace != null)
                {
                    campingField.ImageResource = ComponentsFolder + GetCampingFieldImage(campingPlace.Type.Accommodation);
                    campingField.CampingPlace = campingPlace;
                } 
                else
                {
                    campingField.ImageResource = ComponentsFolder + AddButtonImage;
                }
            }
        }

        public void SetSelectedCampingField(string selectedImage)
        {
            if (SelectedCampingField != null)
            {
                this.SelectedCampingField.BackgroundColor = "#FF68C948";
            }

            this.SelectedCampingField = selectedImage switch
            {
                "CampingFieldImage1" => this.CampingField1,
                "CampingFieldImage2" => this.CampingField2,
                "CampingFieldImage3" => this.CampingField3,
                "CampingFieldImage4" => this.CampingField4,
                "CampingFieldImage5" => this.CampingField5,
                "CampingFieldImage6" => this.CampingField6,
                "CampingFieldImage7" => this.CampingField7,
                "CampingFieldImage8" => this.CampingField8,
                "CampingFieldImage9" => this.CampingField9,
                "CampingFieldImage10" => this.CampingField10,
                "CampingFieldImage11" => this.CampingField11,
                "CampingFieldImage12" => this.CampingField12,
                "CampingFieldImage13" => this.CampingField13,
                "CampingFieldImage14" => this.CampingField14,
                "CampingFieldImage15" => this.CampingField15,
                "CampingFieldImage16" => this.CampingField16,
                "CampingFieldImage17" => this.CampingField17,
                "CampingFieldImage18" => this.CampingField18,
                "CampingFieldImage19" => this.CampingField19,
                "CampingFieldImage20" => this.CampingField20,
                "CampingFieldImage21" => this.CampingField21,
                "CampingFieldImage22" => this.CampingField22,
                "CampingFieldImage23" => this.CampingField23,
                "CampingFieldImage24" => this.CampingField24,
                "CampingFieldImage25" => this.CampingField25,
                "CampingFieldImage26" => this.CampingField26,
                "CampingFieldImage27" => this.CampingField27,
                "CampingFieldImage28" => this.CampingField28,
                "CampingFieldImage29" => this.CampingField29,
                "CampingFieldImage30" => this.CampingField30,
                "CampingFieldImage31" => this.CampingField31,
                "CampingFieldImage32" => this.CampingField32,
                "CampingFieldImage33" => this.CampingField33,
                "CampingFieldImage34" => this.CampingField34,
                _ => this.SelectedCampingField
            };

            this.SelectedCampingField.BackgroundColor = "#C8FFB3";
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        private static string GetCampingFieldImage(Accommodation accommodation)
        {
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

        #endregion
    }
}
