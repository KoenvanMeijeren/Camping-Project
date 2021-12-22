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
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ReservationPaymentViewModel : ObservableObject
    {
        private Reservation _reservation;
        private CampingCustomer _campingCustomer;
        private ObservableCollection<CampingGuest> _campingGuests;
        private Timer paymentTimer;
        private string paymentResponseLink;

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

        public static event EventHandler<ReservationGuestEventArgs> ReservationGuestGoBackEvent;
        public static event EventHandler<ReservationEventArgs> ReservationConfirmedEvent;

        public ReservationPaymentViewModel()
        {
            this.CampingGuests = new ObservableCollection<CampingGuest>();
            ReservationCampingGuestViewModel.ReservationGuestsConfirmedEvent += this.OnReservationGuestsConfirmedEvent;
            SignInViewModel.SignInEvent += SignInViewModelOnSignInEvent;
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

        private async Task CreateReservationPaymentRequest()
        {
            IPaymentClient paymentClient = new PaymentClient("test_sKWktBBCgNax7dGjt8sU6cF92zRuzb");

            PaymentRequest paymentRequest = new PaymentRequest()
            {
                Amount = new Amount(Currency.EUR, 1),
                Description = "Test payment of the example project",
                RedirectUrl = "http://google.com",
                Method = Mollie.Api.Models.Payment.PaymentMethod.Ideal // instead of "Ideal"

            };

            PaymentLinkClient client = new PaymentLinkClient("test_sKWktBBCgNax7dGjt8sU6cF92zRuzb");
            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            Process.Start(new ProcessStartInfo(paymentResponse.Links.Checkout.Href)
            {
                UseShellExecute = true
            });
        }

        public void ExecuteCreateReservationPaymentTest()
        {
            CreateReservationPaymentRequest().GetAwaiter().GetResult();
            //ReservationConfirmedEvent?.Invoke(this, new ReservationEventArgs(Reservation));
        }

        /*private bool CanExecuteCreateReservationPayment()
        {
            //return if payment is done.
            return true;
        }*/

        private void ExecuteCustomerPaymenGoBackReservation()
        {
            ReservationGuestGoBackEvent?.Invoke(this, new ReservationGuestEventArgs(Reservation, CampingGuests));
            CampingGuests.Clear();
        }

        public ICommand CreateReservationPayment => new RelayCommand(ExecuteCreateReservationPaymentTest);

        public ICommand CustomerPaymentGoBackReservation => new RelayCommand(ExecuteCustomerPaymenGoBackReservation);

    }
}
