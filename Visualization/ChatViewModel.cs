using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ViewModel;

namespace Visualization
{
    public class ChatViewModel : ObservableObject
    {
        private ObservableCollection<Client> _users;
        public ObservableCollection<Client> Users
        {
            get => this._users;
            private set
            {
                if (Equals(value, this._users))
                {
                    return;
                }

                this._users = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        private ObservableCollection<string> _messages;
        public ObservableCollection<string> Messages
        {
            get => this._messages;
            private set
            {
                if (Equals(value, this._messages))
                {
                    return;
                }

                this._messages = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        private ServerCommunicator _server;
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                if (Equals(value, this._message))
                {
                    return;
                }
                this._message = value;

                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        public ChatViewModel()
        {
            this._users = new();
            this._messages = new();
            this._server = new ServerCommunicator();
            this._server.connectedEvent += CreateConnected;
            this._server.msgReceivedEvent += MessageReceived;
            this._server.UserDisconnectedEvent += RemoveUser;
            
            
            
            Messages.Add("Start de chat wanneer u wil chatten");
        }

        private void RemoveUser()
        {
            var uid = this._server._packetReader.ReadIncomingMessage();

            var disconnectedUser = this.Users.Where(u => u.UID.ToString() == uid).FirstOrDefault();
            //Remove user to WPF control from non-mainthread (UI thread)
            Application.Current.Dispatcher.Invoke(() => this._users.Remove(disconnectedUser));
        }

        private void MessageReceived()
        {
            var msg = this._server._packetReader.ReadIncomingMessage();

            //Adding message to WPF control from non-mainthread (UI thread)
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }


        /// <summary>
        /// Method that adds a new client to the listview
        /// </summary>
        private void CreateConnected()
        {
            bool isSuperUser = false;
            if(CurrentUser.CampingCustomer != null)
            {
                isSuperUser = true;
            }

            var user = new Client(isSuperUser)
            {
                UID = this._server._packetReader.ReadIncomingMessage()
            };


            //check if client isn't in userslist
            if (!this.Users.Any(x => x.UID == user.UID))
            {
                //Adding user to WPF control from non-mainthread (UI thread)
                Application.Current.Dispatcher.Invoke(() => this._users.Add(user));
            }
        }

        private void ExecuteConnectingToServer()
        {
            this._server.ConnectToServer();
        }

        public ICommand ConnectToServerICommand => new RelayCommand(ExecuteConnectingToServer);


        private void ExecuteSendingMessageToServer()
        {
            this._server.SendMessageToServer(this.Message);
            this.Message = "";//set textbox empty
        }

        private bool CanExecuteSendingMessageToServer()
        {
            return !string.IsNullOrEmpty(this.Message) && this._server.isConnected();//cannot send 'message' if empty and no serverconnection
        }

        public ICommand SendMessageICommand => new RelayCommand(ExecuteSendingMessageToServer, CanExecuteSendingMessageToServer);
    }
}
