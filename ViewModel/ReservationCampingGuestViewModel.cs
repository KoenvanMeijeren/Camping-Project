using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public class ReservationCampingGuestViewModel : ObservableObject
    {
        private readonly ReservationCampingGuest _reservationCampingGuest = new ReservationCampingGuest();
        private string _firstNameGuest, _lastNameGuest;
        private List<CampingGuest> _campingGuestsList;
        private DateTime _birthDate;
        public ObservableCollection<CampingGuest> CampingGuests { get; set; }
        private Reservation _reservation;
        private string _checkInDate, _checkOutDate;

        public string FirstNameGuest
        {
            get => this._firstNameGuest;
            set
            {
                this._firstNameGuest = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string LastNameGuest
        {
            get => this._lastNameGuest;
            set
            {
                this._lastNameGuest = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public DateTime BirthDate {
            get => this._birthDate;
            set
            {
                this._birthDate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public Reservation Reservation {
            get => this._reservation;
            set
            {
                this._reservation = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string CheckInDate
        {
            get => this._checkInDate;
            set
            {
                this._checkInDate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string CheckOutDate
        {
            get => this._checkOutDate;
            set
            {
                this._checkOutDate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ObservableCollection<CampingGuest> CampingGuestsTypes
        {
            get => this.CampingGuests;
            private init
            {
                if (Equals(value, this.CampingGuests))
                {
                    return;
                }

                this.CampingGuestsTypes = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public static event EventHandler<ReservationEventArgs> ReservationConfirmEvent;
        public static event EventHandler<ReservationDurationEventArgs> ReservationGoBackEvent;

        public ReservationCampingGuestViewModel()
        {
            _campingGuestsList = new List<CampingGuest>();
            CampingGuests = new ObservableCollection<CampingGuest>();
            ReservationCustomerFormViewModel.ReservationGuestEvent += this.OnReservationConfirmedEvent;
        }
        private void ExecuteAddGuestReservation()
        {
            string birthDate = BirthDate.ToShortDateString();
            CampingGuest campingGuest = new CampingGuest(FirstNameGuest, LastNameGuest, birthDate);
            campingGuest.Insert();
            _campingGuestsList.Add(campingGuest);
            this.CampingGuestsTypes.Add(campingGuest);
            FirstNameGuest = "";
            LastNameGuest = "";
            BirthDate = new DateTime(1 / 1 / 0001);
        }

        private void ExecuteCustomerGuestReservation()
        {

            // Address addressModel = new Address(args.Reservation.CampingCustomer., this.PostalCode, this.PlaceName);
            /*var address = Reservation.CampingCustomer.Address.FirstOrInsert();

            var customer = Reservation.CampingCustomer.Insert();*/

            var reservation = Reservation.Insert();

            foreach (var guest in _campingGuestsList)
            {
                guest.Insert();
            }

            var lastReservation = Reservation.SelectLast();

            ReservationConfirmEvent?.Invoke(this, new ReservationEventArgs(lastReservation));
        }

        private void ExecuteCutomerGuestGoBackReservation()
        {
            ReservationGoBackEvent?.Invoke(this, new ReservationDurationEventArgs(new CampingPlace(), new ReservationDuration()));
        }

        private void OnReservationConfirmedEvent(object sender, ReservationGuestEventArgs args)
        {
            this.Reservation = args.Reservation;
        }

        public ICommand AddCustomerReservation => new RelayCommand(ExecuteCustomerGuestReservation);

        public ICommand CutomerGuestGoBackReservation => new RelayCommand(ExecuteCutomerGuestGoBackReservation);

        public ICommand AddGuestReservation => new RelayCommand(ExecuteAddGuestReservation);
    }
}
