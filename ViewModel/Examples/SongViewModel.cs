using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ViewModel.Examples
{
    /// <summary>
    /// This class is a view model of a song.
    /// </summary>
    public class SongViewModel : ObservableObject
    {
        #region Construction
        /// <summary>
        /// Constructs the default instance of a SongViewModel
        /// </summary>
        public SongViewModel()
        {
            _song = new Song { ArtistName = "Unknown", SongTitle = "Unknown" };
        }
        #endregion

        #region Members
        Song _song;
        int _count = 0;
        #endregion

        #region Properties
        public Song Song
        {
            get
            {
                return _song;
            }
            set
            {
                _song = value;
            }
        }

        public string ArtistName
        {
            get { return Song.ArtistName; }
            set 
            {
                if (Song.ArtistName != value)
                {
                    Song.ArtistName = value;
                    
                    this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                }
            }
        }

        public string SongTitle
        {
            get { return Song.SongTitle; }
            set
            {
                if (Song.SongTitle != value)
                {
                    Song.SongTitle = value;
                    
                    this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                }
            }
        } 
        #endregion

        #region Commands
        void UpdateArtistNameExecute()
        {
            ++_count;
            ArtistName = string.Format("Elvis ({0})", _count);
        }

        bool CanUpdateArtistNameExecute()
        {
            return true;
        }

        public ICommand UpdateArtistName { get { return new RelayCommand(UpdateArtistNameExecute, CanUpdateArtistNameExecute); } }
        #endregion
    }
}
