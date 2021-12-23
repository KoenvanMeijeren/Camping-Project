using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using Mollie;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentLink.Request;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using SystemCore;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ReservationPaymentViewModel : ObservableObject
    {
        #region Fields

        private Reservation _reservation;
        private CampingCustomer _campingCustomer;
        private ObservableCollection<CampingGuest> _campingGuests;
        private PaymentRequest _paymentRequest;
        private IPaymentClient _paymentClient;
        private PaymentResponse _paymentResponse;
        private string _status;

        #endregion

        #region Properties

        public Reservation Reservation
        {
            get => this._reservation;
            set
            {
                this._reservation = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public CampingCustomer CampingCustomer
        {
            get => this._campingCustomer;
            set
            {
                this._campingCustomer = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ObservableCollection<CampingGuest> CampingGuests {
            get => this._campingGuests;
            set
            {
                this._campingGuests = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        #endregion

        #region Events

        public static event EventHandler<ReservationGuestEventArgs> ReservationGuestGoBackEvent;
        public static event EventHandler<UpdateModelEventArgs<Reservation>> ReservationConfirmedEvent;
        public static event EventHandler<ReservationEventArgs> ReservationFailedEvent; 

        #endregion

        #region View construction

        public ReservationPaymentViewModel()
        {
            this.CampingGuests = new ObservableCollection<CampingGuest>();
            this._status = "null";
            
            ReservationCampingGuestViewModel.ReservationGuestsConfirmedEvent += this.OnReservationGuestsConfirmedEvent;
            SignInViewModel.SignInEvent += this.SignInViewModelOnSignInEvent;
        }

        private void OnReservationGuestsConfirmedEvent(object sender, ReservationGuestEventArgs args)
        {
            this.Reservation = args.Reservation;
            foreach (var campingGuest in args.CampingGuests)
            {
                this.CampingGuests.Add(campingGuest);
            }
        }

        private void SignInViewModelOnSignInEvent(object sender, AccountEventArgs e)
        {
            this.CampingCustomer = CurrentUser.CampingCustomer;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Creates payment request and opens the link.
        /// </summary>
        private async Task CreateReservationPaymentRequest()
        {
            this._paymentClient = new PaymentClient(ConfigReader.GetSetting("mollie_api_key"));

            this._paymentRequest = new PaymentRequest()
            {
                Amount = new Amount(Currency.EUR, (int)Reservation.CampingPlace.TotalPrice),
                Description = Reservation.CampingPlace.Type.Accommodation.Name,
                RedirectUrl = "https://www.ideal.nl/",
                
                Method = PaymentMethod.Ideal // instead of "Ideal"
            };
            this._paymentResponse = await this._paymentClient.CreatePaymentAsync(this._paymentRequest);

            Process.Start(new ProcessStartInfo(this._paymentResponse.Links.Checkout.Href)
            {
                UseShellExecute = true
            });

        }

        /// <summary>
        /// Fills _status with current payment status.
        /// </summary>
        private async Task GetReservationPaymentRequestId()
        {
            PaymentResponse result = await _paymentClient.GetPaymentAsync(_paymentResponse.Id);
            this._status = result.Status;
        }

        /// <summary>
        /// Opens next page and inserts the reservation when payment is done.
        /// </summary>
        private async void ExecuteCreateReservationPayment()
        {
            // run a method in another thread
            await this.CreateReservationPaymentRequest();

            //checks if payment is completed
            bool canContinue = false;
            while (canContinue == false)
            {
                await GetReservationPaymentRequestId();
                if (this._status.Equals("paid"))
                {
                    this.Reservation.Insert();
                    var lastReservation = this.Reservation.SelectLast();

                    foreach (var guest in this.CampingGuests)
                    {
                        guest.Insert();
                
                        (new ReservationCampingGuest(lastReservation, guest.SelectLast())).Insert();
                    }
            
                    ReservationPaymentViewModel.ReservationConfirmedEvent?.Invoke(this, new UpdateModelEventArgs<Reservation>(lastReservation, true, false));
                    canContinue = true;
                }
                if (this._status.Equals("failed") || this._status.Equals("canceled") || this._status.Equals("expired"))
                {
                    ReservationPaymentViewModel.ReservationFailedEvent?.Invoke(this, new ReservationEventArgs(this.Reservation));
                    canContinue = true;
                }
            }

        }

        private bool CanExecuteCreateReservationPayment()
        {
            return !this._status.Equals("open");
        }

        private void ExecuteCustomerPaymentGoBackReservation()
        {
            ReservationPaymentViewModel.ReservationGuestGoBackEvent?.Invoke(this, new ReservationGuestEventArgs(this.Reservation, this.CampingGuests));
            this.CampingGuests.Clear();
        }

        public ICommand CreateReservationPayment => new RelayCommand(ExecuteCreateReservationPayment, CanExecuteCreateReservationPayment);

        public ICommand CustomerPaymentGoBackReservation => new RelayCommand(ExecuteCustomerPaymentGoBackReservation);

        #endregion

    }
}
