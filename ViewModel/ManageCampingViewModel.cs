using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;
using SystemCore;
using ViewModel.EventArguments;

namespace ViewModel
{
    public class ManageCampingViewModel : ObservableObject
    {
        #region Fields
        private Camping _camping;
        private string _campingName, _email, _phoneNumber, _facebook, _twitter, _instagram, _color, _error;
        #endregion

        #region Properties
        public Camping Camping
        {
            get => this._camping;
            set
            {
                if (value == this._camping)
                {
                    return;
                }

                this._camping = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.FillFields(this._camping);
            }
        }

        public string Error
        {
            get => this._error;
            set
            {
                if (value == this._error)
                {
                    return;
                }

                this._error = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string CampingName
        {
            get => this._campingName;
            set
            {
                if (value == this._campingName)
                {
                    return;
                }

                this._campingName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.Error = "";
                if (!Validation.IsInputFilled(this._campingName))
                {
                    this.Error = "Campingnaam is een verplicht veld";
                }
            }
        }

        public string Email
        {
            get => this._email;
            set
            {
                if (value == this._email)
                {
                    return;
                }

                this._email = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.Error = "";
                if (!Validation.IsInputFilled(this._email))
                {
                    this.Error = "Email is een verplicht veld";
                }
                else if (!RegexHelper.IsEmailValid(this._email))
                {
                    this.Error = "Ongeldig email";
                }
            }
        }

        public string Phonenumber
        {
            get => this._phoneNumber;
            set
            {
                if (value == this._phoneNumber)
                {
                    return;
                }

                this._phoneNumber = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.Error = "";
                this.ValidatePhoneNumber(value);
            }
        }

        public string Facebook
        {
            get => this._facebook;
            set
            {
                if (value == this._facebook)
                {
                    return;
                }

                this._facebook = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.Error = "";
            }
        }

        public string Twitter
        {
            get => this._twitter;
            set
            {
                if (value == this._twitter)
                {
                    return;
                }

                this._twitter = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.Error = "";
            }
        }

        public string Instagram
        {
            get => this._instagram;
            set
            {
                if (value == this._instagram)
                {
                    return;
                }

                this._instagram = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));

                this.Error = "";
            }
        }

        public string Color
        {
            get => this._color;
            set
            {
                if (value == this._color)
                {
                    return;
                }

                this._color = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        #endregion

        #region ViewConstruction
        public ManageCampingViewModel()
        {
            CurrentCamping.CurrentCampingSetEvent += CurrentCampingOnCurrentCampingSetEvent;
        }

        private void CurrentCampingOnCurrentCampingSetEvent(object sender, UpdateModelEventArgs<Camping> e)
        {
            this.Camping = e.Model;
        }

        private void FillFields(Camping camping)
        {
            if (camping == null)
            {
                return;
            }
            
            this.CampingName = camping.Name;
            this.Email = camping.Email;
            this.Phonenumber = camping.PhoneNumber;
            this.Facebook = camping.Facebook;
            this.Twitter = camping.Twitter;
            this.Instagram = camping.Instagram;
            this.Color = camping.Color;
        }
        #endregion

        #region Input validation

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            if (!Validation.IsInputFilled(phoneNumber))
            {
                this.Error = "Telefoonnummer is een verplicht veld";
                return false;
            }
            
            if (!Validation.IsNumber(phoneNumber))
            {
                this.Error = "Telefoonnummer is ongeldig";
                return false;
            }

            return true;
        }

        #endregion
        
        #region Commands 
        private void ExecuteCancelEditAction()
        {
            this.FillFields(this._camping);
        }
        public ICommand EditCancel => new RelayCommand(ExecuteCancelEditAction);

        private void ExecuteEditSave()
        {
            this.Camping.Update(this.CampingName, this.Camping.Address, this.Camping.CampingOwner, 
            this.Phonenumber, this.Email, this.Facebook, this.Twitter, this.Instagram, this.Color);

            CurrentCamping.SetCurrentCamping(this.Camping);
            MessageBox.Show("De camping is succesvol bijgewerkt.", "Camping bewerken");
        }

        private bool CanExecuteEditSave()
        {
            return Validation.IsInputFilled(this.CampingName)
                   && Validation.IsInputFilled(this.Email)
                   && RegexHelper.IsEmailValid(this.Email)
                   && Validation.IsInputFilled(this.Phonenumber)
                   && Validation.IsInputFilled(this.Color)
                   && this.Camping != null 
                   && this.ValidatePhoneNumber(this.Phonenumber);
        }

        public ICommand EditSave => new RelayCommand(ExecuteEditSave, CanExecuteEditSave);
        #endregion
    }
}
