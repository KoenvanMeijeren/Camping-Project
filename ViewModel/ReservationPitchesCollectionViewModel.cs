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
    public class ReservationPitchesCollectionViewModel : ObservableObject
    {
        private string _campingPitch;
        private ObservableCollection<string> _campingPlaceTypes;
        private ObservableCollection<CampingPlace> _campingPlaces;
        public static event EventHandler<ReserveEventArgs> ReserveEvent;

        public ObservableCollection<CampingPlace> CampingPlaces
        {
            get => _campingPlaces;
            private set
            {
                if (Equals(value, _campingPlaces)) return;
                _campingPlaces = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ObservableCollection<string> CampingPlaceTypes
        {
            get => _campingPlaceTypes;
            private init
            {
                if (Equals(value, _campingPlaceTypes)) return;
                _campingPlaceTypes = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string SelectedPlaceType
        {
            get => _campingPitch;
            set
            {
                if (value == _campingPitch) return;
                _campingPitch = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                this.SetOverview();
            }
        }

        public ReservationPitchesCollectionViewModel()
        {
            CampingPlaceTypes = new ObservableCollection<string>();
            CampingPlaceTypes.Add("Alle");
            CampingPlaceTypes.Add("Bungalow");
            CampingPlaceTypes.Add("Camper");
            CampingPlaceTypes.Add("Caravan");
            CampingPlaceTypes.Add("Chalet");
            CampingPlaceTypes.Add("Tent");

            CampingPlaces = new ObservableCollection<CampingPlace>();
            var campingPlaceModel = new CampingPlace();
            foreach (CampingPlace item in campingPlaceModel.Select())
            {
                CampingPlaces.Add(item);
            }
        }

        private void SetOverview()
        {
            var selectedcampingpitch = this._campingPitch;
            var CampingPlacesResults = CampingPlaces.Where(campingPlaceViewData => campingPlaceViewData.Type.Equals(selectedcampingpitch));

            while(CampingPlaces.Count > 0)
            {
                this.CampingPlaces.RemoveAt(0);
            }

            foreach (CampingPlace item in CampingPlacesResults)
            {
                CampingPlaces.Add(item);
            }
        }

    }
}

