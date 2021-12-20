using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Model;

namespace Model
{
    class ChatViewModel : ObservableObject
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
        private Server _server;
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
        /*       private string _username;

               public string Username
               {
                   get => _username;
                   set
                   {
                       if (Equals(value, this._username))
                       {
                           return;
                       }
                       this._username = value; 

                       this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                   }
               }*/


        public ChatViewModel()
        {
            this._users = new();
            this._messages = new();
            _server = new Server();
            _server.connectedEvent += UserConnected;
            _server.msgReceivedEvent += MessageReceived;
            _server.UserDisconnectedEvent += RemoveUser;

            _messages.Add("Start de chat wanneer u wil chatten");
        }

        private void RemoveUser()
        {
            var uid = _server._packetReader.ReadMessage();

            var disconnectedUser = _users.Where(u => u.UID.ToString() == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => _users.Remove(disconnectedUser));//will remove the user overal
        }

        private void MessageReceived()
        {
            var msg = _server._packetReader.ReadMessage();
            //explain this line code
            Application.Current.Dispatcher.Invoke(() => _messages.Add(msg));
        }


        /// <summary>
        /// Method that adds a new client to the listview
        /// </summary>
        private void UserConnected()
        {
            var user = new UserModel
            {
                UID = _server._packetReader.ReadMessage()
            };

            //check if client isn't in userslist
            if (!Users.Any(x => x.UID == user.UID))
            {
                //add user also in other thread => client
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }

        private void ExecuteConnectingToServer()
        {
            _server.ConnectToServer();
        }

        /*        public ICommand ConnectToServerICommand => new RelayCommand(ExecuteConnectingToServer, CanExecuteConnectingToServer);
        */
        public ICommand ConnectToServerICommand => new RelayCommand(ExecuteConnectingToServer);


        private void ExecuteSendingMessageToServer()
        {
            _server.SendMessageToServer(this.Message);
        }

        private bool CanExecuteSendingMessageToServer()
        {
            return !string.IsNullOrEmpty(this.Message) && _server.isConnected();//cannot send 'message' is empty
        }

        public ICommand SendMessageICommand => new RelayCommand(ExecuteSendingMessageToServer, CanExecuteSendingMessageToServer);

    }
}
