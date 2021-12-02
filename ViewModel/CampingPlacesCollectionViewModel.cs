using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModel
{
    public class CampingPlacesCollectionViewModel : ObservableObject
    {
        //CampingPlaceViewData/CampingPlace?
        private readonly CampingPlace _campingPlaceModel = new CampingPlace();
        private const string SelectAll = "Alle";

        private string _selectedCampingPlaceType;
        
        private readonly ObservableCollection<string> _campingPlaceTypes;
        private ObservableCollection<CampingPlace> _campingPlaces;
        public static event EventHandler<ReserveEventArgs> ReserveEvent;
        private DateTime _checkOutDate;
        private DateTime _checkInDate;
        private string _minNightPrice;
        private string _maxNightPrice;

        public string Title { get; private set; }

        #region getters/setters
        public string MinNightPrice
        {
            get => this._minNightPrice;
            set
            {
                if (Equals(value, this._minNightPrice))
                {
                    return;
                }

                this._minNightPrice = value;
                this.SetOverview();

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string MaxNightPrice
        {
            get => this._maxNightPrice;
            set
            {
                if (Equals(value, this._maxNightPrice))
                {
                    return;
                }

                this._maxNightPrice = value;
                this.SetOverview();

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }


        public DateTime CheckInDate
        {
            get => this._checkInDate;
            private set
            {
                if (Equals(value, this._checkInDate))
                {
                    return;
                }

                this._checkInDate = value;
                this.SetOverview();
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public DateTime CheckOutDate
        {
            get => this._checkOutDate;
            private set
            {
                if (Equals(value, this._checkOutDate))
                {
                    return;
                }

                this._checkOutDate = value;
                this.SetOverview();
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ObservableCollection<CampingPlace> CampingPlaces
        {
            get => this._campingPlaces;
            private set
            {
                if (Equals(value, this._campingPlaces))
                {
                    return;
                }
                
                this._campingPlaces = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ObservableCollection<string> CampingPlaceTypes
        {
            get => this._campingPlaceTypes;
            private init
            {
                if (Equals(value, this._campingPlaceTypes))
                {
                    return;
                }
                
                this._campingPlaceTypes = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string SelectedPlaceType
        {
            get => this._selectedCampingPlaceType;
            set
            {
                if (Equals(value, this._selectedCampingPlaceType))
                {
                    return;
                }

                this._selectedCampingPlaceType = value;
                this.SetOverview();
                
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        #endregion

        public CampingPlacesCollectionViewModel()
        {
            
            this.CampingPlaceTypes = new ObservableCollection<string>();
            this.CampingPlaceTypes.Add("Alle");
            this.CampingPlaceTypes.Add("Bungalow");
            this.CampingPlaceTypes.Add("Camper");
            this.CampingPlaceTypes.Add("Caravan");
            this.CampingPlaceTypes.Add("Chalet");
            this.CampingPlaceTypes.Add("Tent");

            
            this.CampingPlaces = new ObservableCollection<CampingPlace>(this.GetCampingPlaces());
            this.SelectedPlaceType = SelectAll;
            var today = DateTime.Today;
            this.CheckInDate = today;
            this.CheckOutDate = today.AddDays(1);

            this.MaxNightPrice = null;
            this.MinNightPrice = null;


        }

        private void SetOverview()
        {
            if (this.CheckInDate == null || this.CheckOutDate == null)
            {
                Title = "Selecteer uw verblijfstermijn";
                //this.ReserveButton.IsEnabled = false;
                return;
            }

            while (this.CampingPlaces.Count > 0)
            {
                this.CampingPlaces.RemoveAt(0);
            }


            var selectedCampingPlaceType = this._selectedCampingPlaceType;

            var campingPlaceItems = this.GetCampingPlaces();
            if (!selectedCampingPlaceType.Equals("Alle"))
            {
                campingPlaceItems = campingPlaceItems.Where(campingPlace => campingPlace.Type.Accommodation.Name.Equals(selectedCampingPlaceType)).ToList();
            }

            foreach (CampingPlace item in campingPlaceItems)
            {
                this.CampingPlaces.Add(item);
            }
            var campingCopy = campingPlaceItems;//check collection changed
            campingPlaceItems = CampingPlaceViewDataCollection.ToFilteredOnReservedCampingPitches(campingPlaceItems, CheckInDate, CheckOutDate);


            if (int.TryParse(this.MinNightPrice, out int min))
            {
                CampingPlaces = new ObservableCollection<CampingPlace>( CampingPlaces.Where(campingPlace => campingPlace.TotalPrice >= min).ToList());
            }

            if (int.TryParse(this.MaxNightPrice, out int max))
            {
                CampingPlaces = new ObservableCollection<CampingPlace>(CampingPlaces.Where(campingPlace => campingPlace.TotalPrice <= max).ToList());
            }
           
        }

        private IEnumerable<CampingPlace> GetCampingPlaces()
        {
            return this._campingPlaceModel.Select();
        }



    }
}

