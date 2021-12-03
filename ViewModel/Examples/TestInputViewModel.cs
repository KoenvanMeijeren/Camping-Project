using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ViewModel.Examples
{
    public class TestInputViewModel : ObservableObject
    {
        private string serialNumber;
        private string modelName;
        private ObservableCollection<Model> models;
        private string soNumber;

        public TestInputViewModel()
        {
            Models = new ObservableCollection<Model>();
            Models.Add(new Model("test"));
            Models.Add(new Model("zoveel"));
            Models.Add(new Model("alweer"));
        }


        public ObservableCollection<Model> Models
        {
            get => models;
            private init
            {
                if (Equals(value, models)) return;
                models = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string ModelName
        {
            get => modelName;
            set
            {
                if (value == modelName) return;
                modelName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        public string SerialNumber // Note you spelled this wrong in your xaml. SONumber
        {
            get => serialNumber;
            set
            {
                if (value == serialNumber) return;
                serialNumber = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public string SONumber
        {
            get => soNumber;
            set
            {
                if (value == soNumber) return;
                soNumber = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        void SaveExecute()
        {
            MessageBox.Show($"Ingevoerde waarden: {this.ModelName}, {this.SerialNumber} en {this.SONumber}");
        }

        bool CanExecuteSave()
        {
            return true;
        }

        public ICommand Save => new RelayCommand(SaveExecute, CanExecuteSave);
    }
    
    public class Model : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private string _model;

        public string ModelName
        {
            get => _model;
            set
            {
                if (value == _model) return;
                _model = value;
                this.OnPropertyChanged();
            }
        }

        public Model(string name)
        {
            this._model = name;
        }

        public override string ToString()
        {
            return this._model;
        }
    }
    
}