using System;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SystemCore;

namespace ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {

        #region Fields
        
        private string _title, _subtitle, _subSubtitle, _color;

        
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
            get => this._color;
            private set
            {
                if (Equals(value, this._color))
                {
                    return;
                }

                this._color = value;

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            this._subtitle = ConfigReader.GetSetting("Subtitle");
            this._subSubtitle = ConfigReader.GetSetting("SubSubtitle");
            this._title = this.Subtitle + " " + this.SubSubtitle;
            this._color = "#000000";

            CurrentCamping.CurrentCampingSetEvent += OnCurrentCampingSetEvent;
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        public void OnCurrentCampingSetEvent(object sender, EventArgs e)
        {
            this._subtitle = ConfigReader.GetSetting("Subtitle");
            this._subSubtitle = "\n" + CurrentCamping.Camping.Name;
            this._title = this.Subtitle + " " + this.SubSubtitle;
            this._color = CurrentCamping.Camping.Color;

            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }
    }
}