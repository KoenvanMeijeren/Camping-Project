﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
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
            //creates payment request and opens paymentlink
            IPaymentClient paymentClient = new PaymentClient("test_sKWktBBCgNax7dGjt8sU6cF92zRuzb");

            PaymentRequest paymentRequest = new PaymentRequest()
            {
                Amount = new Amount(Currency.EUR, (int)Reservation.CampingPlace.TotalPrice),
                Description = Reservation.CampingPlace.Type.Accommodation.Name,
                RedirectUrl = "https://www.ideal.nl/",
                
                Method = PaymentMethod.Ideal // instead of "Ideal"
            };
            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            Process.Start(new ProcessStartInfo(paymentResponse.Links.Checkout.Href)
            {
                UseShellExecute = true
            });

        }

        public async void ExecuteCreateReservationPaymentTest()
        {
            // run a method in another thread
            await CreateReservationPaymentRequest();

            //insert Reservation and campingGuests
            this.Reservation.Insert();
            var lastReservation = this.Reservation.SelectLast();
            CampingGuest campingGuest = new CampingGuest();

            foreach (var guest in this.CampingGuests)
            {
                guest.Insert();
                var lastGuest = campingGuest.SelectLast();
                (new ReservationCampingGuest(lastReservation, lastGuest)).Insert();
            }

            ReservationConfirmedEvent?.Invoke(this, new ReservationEventArgs(Reservation));
        }

        private void ExecuteCustomerPaymenGoBackReservation()
        {
            ReservationGuestGoBackEvent?.Invoke(this, new ReservationGuestEventArgs(Reservation, CampingGuests));
            CampingGuests.Clear();
        }

        public ICommand CreateReservationPayment => new RelayCommand(ExecuteCreateReservationPaymentTest);

        public ICommand CustomerPaymentGoBackReservation => new RelayCommand(ExecuteCustomerPaymenGoBackReservation);

    }
}
