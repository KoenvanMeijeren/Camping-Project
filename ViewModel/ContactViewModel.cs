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
        private string _facebookLink;
        private string _twitterLink;
        private string _instagramLink;
        #endregion

        #region Properties

        private string _contactPageAddress = "Adres: ";
        public string ContactPageAddress
        {
            get => _contactPageAddress;
            set => this._contactPageAddress = "Adres: " + value;
        }

        private string _contactPostalCode = "Postcode: ";
        public string ContactPostalCode
        {
            get => _contactPostalCode;
            set => this._contactPostalCode = "Postcode: " + value;
        }

        private string _contactPagePhoneNumber = "Telefoonnummer: ";
        public string ContactPagePhoneNumber
        {
            get => _contactPagePhoneNumber;
            set => this._contactPagePhoneNumber = "Telefoonnummer: " + value;
        }

        private string _contactPageEmailAddress = "Email: ";
        public string ContactPageEmailAddress
        {
            get => _contactPageEmailAddress;
            set => this._contactPageEmailAddress = "Email: " + value;
        }

        #endregion

        #region Chat button
        public ICommand ChatButton => new RelayCommand(ExecuteGoToChat);
        public static event EventHandler FromContactToChatEvent;
        #endregion

        #region Social media buttons
        // Defined this way so it is possible to add parameters to function in a RelayCommand
        public ICommand FacebookButton => new RelayCommand<object>((x) => ExecuteLink(_facebookLink));
        public ICommand InstagramButton => new RelayCommand<object>((x) => ExecuteLink(_instagramLink));
        public ICommand TwitterButton => new RelayCommand<object>((x) => ExecuteLink(_twitterLink));

        public static event EventHandler<LinkEventArgs> LinkEvent;
        #endregion

        public ContactViewModel()
        {
            Camping campingModel = new Camping();
            Camping camping = campingModel.SelectLast();

            this.ContactPageAddress = $"{camping.Address.Street}, {camping.Address.Place}";
            this.ContactPostalCode = camping.Address.PostalCode;
            this.ContactPagePhoneNumber = camping.PhoneNumber;
            this.ContactPageEmailAddress = camping.Email;
            this._facebookLink = camping.Facebook;
            this._twitterLink = camping.Twitter;
            this._instagramLink = camping.Instagram;
        }

        /// <summary>
        /// EVEnt that fires the go to chat
        /// </summary>
        #region Chat
        private void ExecuteGoToChat()
        {
            FromContactToChatEvent?.Invoke(this, null);
        }
        #endregion

        #region Social media
        /// <summary>
        /// Event that fires the link to the View
        /// </summary>
        /// <param name="href">String of the href you want to visit</param>
        private void ExecuteLink(string href)
        {
            LinkEvent?.Invoke(this, new LinkEventArgs(href));
        }
        #endregion
    }
}