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
using System.Globalization;
using System.Windows.Input;
using SystemCore;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ContactViewModel : ObservableObject
    {
        #region Fields
        
        private readonly Camping _campingModel = new();

        #endregion

        #region Properties
        
        public string FacebookLink, TwitterLink, InstagramLink;
        
        private Camping _currentCamping;
        
        public Camping CurrentCamping
        {
            get => _currentCamping;
            set
            {
                this._currentCamping = value;
                this.FillContactViewModel(value);
            }
        }

        private string _contactPageAddress = "Adres: ";
        public string ContactPageAddress
        {
            get => _contactPageAddress;
            private set => this._contactPageAddress = "Adres: " + value;
        }

        private string _contactPostalCode = "Postcode: ";
        public string ContactPostalCode
        {
            get => _contactPostalCode;
            private set => this._contactPostalCode = "Postcode: " + value;
        }

        private string _contactPagePhoneNumber = "Telefoonnummer: ";
        public string ContactPagePhoneNumber
        {
            get => _contactPagePhoneNumber;
            private set => this._contactPagePhoneNumber = "Telefoonnummer: " + value;
        }

        private string _contactPageEmailAddress = "Email: ";
        public string ContactPageEmailAddress
        {
            get => _contactPageEmailAddress;
            private set => this._contactPageEmailAddress = "Email: " + value;
        }

        #endregion

        #region Chat button
        public ICommand ChatButton => new RelayCommand(ExecuteGoToChat);
        public static event EventHandler FromContactToChatEvent;
        #endregion

        #region Social media buttons
        // Defined this way so it is possible to add parameters to function in a RelayCommand
        public ICommand FacebookButton => new RelayCommand<object>((x) => ExecuteLink(this.FacebookLink));
        public ICommand InstagramButton => new RelayCommand<object>((x) => ExecuteLink(this.InstagramLink));
        public ICommand TwitterButton => new RelayCommand<object>((x) => ExecuteLink(this.TwitterLink));

        public static event EventHandler<LinkEventArgs> LinkEvent;
        #endregion

        public ContactViewModel()
        {
            this.CurrentCamping = this.GetCamping();
        }

        private void FillContactViewModel(Camping camping)
        {
            this.ContactPageAddress = $"{camping.Address.Street}, {camping.Address.Place}";
            this.ContactPostalCode = camping.Address.PostalCode;
            this.ContactPagePhoneNumber = camping.PhoneNumber;
            this.ContactPageEmailAddress = camping.Email;
            this.FacebookLink = camping.Facebook;
            this.TwitterLink = camping.Twitter;
            this.InstagramLink = camping.Instagram;

            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        /// <summary>
        /// Returns the camping object.
        /// </summary>
        /// <returns>Camping object, latest camping inserted in database (should only be one)</returns>
        public virtual Camping GetCamping()
        {
            return this._campingModel.SelectLast();
        }

        #region Chat
        /// <summary>
        /// Event that fires the go to chat.
        /// </summary>
        private void ExecuteGoToChat()
        {
            ContactViewModel.FromContactToChatEvent?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Social media
        /// <summary>
        /// Event that fires the link to the View.
        /// </summary>
        /// <param name="href">String of the href you want to visit</param>
        private void ExecuteLink(string href)
        {
            ContactViewModel.LinkEvent?.Invoke(this, new LinkEventArgs(href));
        }
        #endregion
    }
}