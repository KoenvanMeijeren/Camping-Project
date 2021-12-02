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
        private readonly CampingPlace _campingPlaceModel = new CampingPlace();
        
        private string _selectedCampingPlaceType;
        
        private readonly ObservableCollection<string> _campingPlaceTypes;
        private ObservableCollection<CampingPlace> _campingPlaces;
        public static event EventHandler<ReservationEventArgs> ReserveEvent;

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
            
        }

        private void SetOverview()
        {
            while(this.CampingPlaces.Count > 0)
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
        }

        private IEnumerable<CampingPlace> GetCampingPlaces()
        {
            return this._campingPlaceModel.Select();
        }
    }
}

