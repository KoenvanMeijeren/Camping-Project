using System;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SystemCore;

namespace ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {

        #region Fields
        private string _title, _subtitle, _subSubtitle, _temporaryApplicationColor;
        private const string TemporaryApplicationColor = "#006837";
        #endregion

        #region Properties
        public string Title
        {
            get => this._title;
            private set
            {
                if (Equals(value, this._title))
                {
                    return;
                }

                this._title = value;
                
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        public string Subtitle
        {
            get => this._subtitle;
            private set
            {
                if (Equals(value, this._subtitle))
                {
                    return;
                }

                this._subtitle = value;
                
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string SubSubtitle
        {
            get => this._subSubtitle;
            private set
            {
                if (Equals(value, this._subSubtitle))
                {
                    return;
                }

                this._subSubtitle = value;
                
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string Color
        {
            get => this._temporaryApplicationColor;
            private set
            {
                if (Equals(value, this._temporaryApplicationColor))
                {
                    return;
                }

                this._temporaryApplicationColor = value;

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        #endregion

        #region View construction
        public MainWindowViewModel()
        {
            this._subtitle = ConfigReader.GetSetting("Subtitle");
            this._subSubtitle = ConfigReader.GetSetting("SubSubtitle");
            this._title = this.Subtitle + " " + this.SubSubtitle;
            this._temporaryApplicationColor = TemporaryApplicationColor;

            CurrentCamping.CurrentCampingSetEvent += OnCurrentCampingSetEvent;
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }
        #endregion

        #region Commands
        public void OnCurrentCampingSetEvent(object sender, EventArgs e)
        {
            if (CurrentCamping.Camping == null)
            {
                return;
            }
            
            this._subtitle = ConfigReader.GetSetting("Subtitle");
            this._subSubtitle = "\n" + CurrentCamping.Camping.Name;
            this._title = this.Subtitle + " " + this.SubSubtitle;
            this._temporaryApplicationColor = CurrentCamping.Camping.Color;

            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }
        #endregion
    }
}