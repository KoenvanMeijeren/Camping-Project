using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
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
        Dictionary<int, CampingField> CampingFields { get; set; }

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

        public ManageCampingMapViewModel()
        {
            FillCampingFieldColors();
            SetFields();
        }

        public void FillCampingFieldColors()
        {
            CampingFields = new Dictionary<int, CampingField>();

            CampingFields.Add(1, CampingField1 = new CampingField(1, "#FF68C948", null));
            CampingFields.Add(2, CampingField2 = new CampingField(2, "#FF68C948", null));
            CampingFields.Add(3, CampingField3 = new CampingField(3, "#FF68C948", null));
            CampingFields.Add(4, CampingField4 = new CampingField(4, "#FF68C948", null));
            CampingFields.Add(5, CampingField5 = new CampingField(5, "#FF68C948", null));
            CampingFields.Add(6, CampingField6 = new CampingField(6, "#FF68C948", null));
            CampingFields.Add(7, CampingField7 = new CampingField(7, "#FF68C948", null));
            CampingFields.Add(8, CampingField8 = new CampingField(8, "#FF68C948", null));
            CampingFields.Add(9, CampingField9 = new CampingField(9, "#FF68C948", null));
            CampingFields.Add(10, CampingField10 = new CampingField(10, "#FF68C948", null));
            CampingFields.Add(11, CampingField11 = new CampingField(11, "#FF68C948", null));
            CampingFields.Add(12, CampingField12 = new CampingField(12, "#FF68C948", null));
            CampingFields.Add(13, CampingField13 = new CampingField(13, "#FF68C948", null));
            CampingFields.Add(14, CampingField14 = new CampingField(14, "#FF68C948", null));
            CampingFields.Add(15, CampingField15 = new CampingField(15, "#FF68C948", null));
            CampingFields.Add(16, CampingField16 = new CampingField(16, "#FF68C948", null));
            CampingFields.Add(17, CampingField17 = new CampingField(17, "#FF68C948", null));
            CampingFields.Add(18, CampingField18 = new CampingField(18, "#FF68C948", null));
            CampingFields.Add(19, CampingField19 = new CampingField(19, "#FF68C948", null));
            CampingFields.Add(20, CampingField20 = new CampingField(20, "#FF68C948", null));
            CampingFields.Add(21, CampingField21 = new CampingField(21, "#FF68C948", null));
            CampingFields.Add(22, CampingField22 = new CampingField(22, "#FF68C948", null));
            CampingFields.Add(23, CampingField23 = new CampingField(23, "#FF68C948", null));
            CampingFields.Add(24, CampingField24 = new CampingField(24, "#FF68C948", null));
            CampingFields.Add(25, CampingField25 = new CampingField(25, "#FF68C948", null));
            CampingFields.Add(26, CampingField26 = new CampingField(26, "#FF68C948", null));
            CampingFields.Add(27, CampingField27 = new CampingField(27, "#FF68C948", null));
            CampingFields.Add(28, CampingField28 = new CampingField(28, "#FF68C948", null));
            CampingFields.Add(29, CampingField29 = new CampingField(29, "#FF68C948", null));
            CampingFields.Add(30, CampingField30 = new CampingField(30, "#FF68C948", null));
            CampingFields.Add(31, CampingField31 = new CampingField(31, "#FF68C948", null));
            CampingFields.Add(32, CampingField32 = new CampingField(32, "#FF68C948", null));
            CampingFields.Add(33, CampingField33 = new CampingField(33, "#FF68C948", null));
            CampingFields.Add(34, CampingField34 = new CampingField(34, "#FF68C948", null));
        }

        public void SetFields()
        {
            CampingPlace emptyCampingPlace = new CampingPlace();
            CampingPlace campingPlace = new CampingPlace();

            foreach (CampingField campingField in CampingFields.Values)
            {

                campingPlace = emptyCampingPlace.SelectByPlaceNumber(campingField.LocationNumber);
                if (campingPlace != null)
                {
                    campingField.ImageResource = "/MapComponents/CampingFieldImage-" + campingPlace.Type.Accommodation.Name + ".png";
                }
            }
        }
    }
}
