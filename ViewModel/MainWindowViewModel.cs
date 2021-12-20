using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SystemCore;

namespace Visualization
{
    public class MainWindowViewModel : ObservableObject
    {

        #region Fields
        
        private string _title;
        private string _subtitle;
        private string _subSubtitle;
        
        #endregion

        #region Properties

        public string Title
        {
            get => this._title;
            private init
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
            private init
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
            private init
            {
                if (Equals(value, this._subSubtitle))
                {
                    return;
                }

                this._subSubtitle = value;
                
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        
        #endregion

        public MainWindowViewModel()
        {
            this.Title = ConfigReader.GetSetting("Title");
            this.Subtitle = ConfigReader.GetSetting("Subtitle");
            this.SubSubtitle = ConfigReader.GetSetting("SubSubtitle");
        }

    }
}