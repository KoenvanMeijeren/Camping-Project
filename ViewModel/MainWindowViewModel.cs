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
            this.Subtitle = ConfigReader.GetSetting("Subtitle");
            this.SubSubtitle = ConfigReader.GetSetting("SubSubtitle");
            this.Title = this.Subtitle + " " + this.SubSubtitle;
            this.Color = "#000000";

            CurrentCamping.CurrentCampingSetEvent += OnCurrentCampingSetEvent;
        }

        public void OnCurrentCampingSetEvent(object sender, EventArgs e)
        {
            this.Subtitle = ConfigReader.GetSetting("Subtitle");
            this.SubSubtitle = "\n" + CurrentCamping.Camping.Name;
            this.Title = this.Subtitle + " " + this.SubSubtitle;
            this.Color = CurrentCamping.Camping.Color;
        }
    }
}