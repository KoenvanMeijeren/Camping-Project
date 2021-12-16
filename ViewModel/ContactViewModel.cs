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

        public ContactViewModel()
        {
            Camping campingModel = new Camping();
            Camping camping = campingModel.SelectById(2);

            this.ContactPageAddress = $"{camping.Address.Street}, {camping.Address.Place}";
            this.ContactPostalCode = camping.Address.PostalCode;
            this.ContactPagePhoneNumber = camping.campingPhoneNumber;
            this.ContactPageEmailAddress = camping.campingEmailAddress;
        }
    }
}
