using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;

namespace ViewModel
{
    public class ReservationCollectionViewModel : ObservableObject
    {
        #region Fields
        private readonly Accommodation _accommodationModel = new Accommodation();
        private readonly Reservation _reservationModel = new Reservation();
        
        private const string SelectAll = "Alle";

        private readonly ObservableCollection<string> _campingPlaceTypes;
        public ObservableCollection<ReservationViewModel> Reservations { get; private set; }
        
        private DateTime _checkOutDate, _checkInDate;
        private string _minTotalPrice, _maxTotalPrice, _selectedCampingPlaceType, _guests;

        #endregion

        #region Properties

        public string MinTotalPrice
        {
            get => this._minTotalPrice;
            set
            {
                if (Equals(value, this._minTotalPrice))
                {
                    return;
                }

                this._minTotalPrice = value;
                this.SetOverview();

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string MaxTotalPrice
        {
            get => this._maxTotalPrice;
            set
            {
                if (Equals(value, this._maxTotalPrice))
                {
                    return;
                }

                this._maxTotalPrice = value;
                this.SetOverview();

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string Guests
        {
            get => this._guests;
            set
            {
                if (Equals(value, this._guests))
                {
                    return;
                }

                this._guests = value;
                this.SetOverview();

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public DateTime CheckInDate
        {
            get => this._checkInDate;
            set
            {
                if (Equals(value, this._checkInDate))
                {
                    return;
                }

                int daysDifference = this._checkOutDate.Subtract(this._checkInDate).Days;
                
                this._checkInDate = value;
                this.SetOverview();
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                this.CheckOutDate = this._checkInDate.AddDays(daysDifference);
            }
        }

        public DateTime CheckOutDate
        {
            get => this._checkOutDate;
            set
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

        public string SelectedCampingPlaceType
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

        public ReservationCollectionViewModel()
        {
            this.Reservations = new ObservableCollection<ReservationViewModel>();
            this.CampingPlaceTypes = new ObservableCollection<string>();
            
            //Loop through rows in Accommodation table
            this.CampingPlaceTypes.Add(SelectAll);
            foreach (var accommodationDatabaseRow in this._accommodationModel.Select())
            {
                this.CampingPlaceTypes.Add(accommodationDatabaseRow.Name);
            }

            this.SelectedCampingPlaceType = SelectAll;
            
            DateTime date = DateTime.Today;
            this.CheckInDate = new DateTime(date.Year, date.Month, 1);
            this.CheckOutDate = this.CheckInDate.AddMonths(1).AddDays(-1);

            ReservationCustomerFormViewModel.ReservationConfirmedEvent += this.OnReservationConfirmedEvent;
        }

        private void OnReservationConfirmedEvent(object sender, ReservationEventArgs args)
        {
            this.SetOverview();
        }

        private void SetOverview()
        {
            while (this.Reservations.Any())
            {
                this.Reservations.RemoveAt(0);
            }

            var reservationItems = this._reservationModel.Select();
            if (!this.SelectedCampingPlaceType.Equals(SelectAll))
            {
                reservationItems = reservationItems.Where(reservation => reservation.CampingPlace.Type.Accommodation.Name.Equals(this.SelectedCampingPlaceType)).ToList();
            }
            
            if (int.TryParse(this.MinTotalPrice, out int min))
            {
                reservationItems = reservationItems.Where(reservation => reservation.TotalPrice >= min).ToList();
            }
            
            if (int.TryParse(this.MaxTotalPrice, out int max))
            {
                reservationItems = reservationItems.Where(reservation => reservation.TotalPrice <= max).ToList();
            }
            
            if (this.CheckInDate != DateTime.MinValue)
            {
                reservationItems = reservationItems.Where(reservation => reservation.Duration.CheckInDatetime >= this.CheckInDate).ToList();
            }
            
            if (this.CheckOutDate != DateTime.MinValue)
            {
                reservationItems = reservationItems.Where(reservation => reservation.Duration.CheckOutDatetime <= this.CheckOutDate).ToList();
            }
            
            if (int.TryParse(this.Guests, out int guests))
            {
                reservationItems = reservationItems.Where(reservation => reservation.NumberOfPeople >= guests).ToList();
            }
            
            foreach (var reservation in reservationItems)
            {
                this.Reservations.Add(new ReservationViewModel(reservation));
            }
        }
    }

    public class ReservationViewModel
    {
        public Reservation Reservation { get; private init; }

        public ReservationViewModel(Reservation reservation)
        {
            this.Reservation = reservation;
        }
        
    }
}
