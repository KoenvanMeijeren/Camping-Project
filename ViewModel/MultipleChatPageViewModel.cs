using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;
using ViewModel.EventArguments;
using SystemCore;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModel
{
    public class MultipleChatPageViewModel : ObservableObject
    {
        private Chat _selectedChat;
        private Chat _chat;
        private ObservableCollection<Chat> _chats;
        private MessageJSON _selectedChatMessages;

        public string ChatTextInput { get; private set; }
        public List<MessageJSON> ShownChatMessages { get; private set; }
        public string CurrenCustomerName { get; private set; }


        public MessageJSON SelectedChatMessages
        {
            get => this._selectedChatMessages;
            set
            {
                if (value == this._selectedChatMessages)
                {
                    return;
                }

                this._selectedChatMessages = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        public ObservableCollection<Chat> Chats
        {
            get => this._chats;
            set
            {
                if (value == this._chats)
                {
                    return;
                }

                this._chats = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        public Chat SelectedChat
        {
            get => this._selectedChat;
            set
            {
                if (value == this._selectedChat)
                {
                    return;
                }

                this._selectedChat = value;
                //this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                ShowChatMessages();
            }
        }

        // TODO: Fetch database row, loop through and display it
        public MultipleChatPageViewModel()
        {
            //TODO: fetch all chats from database, where unsolved?
            _chat = new Chat();
            this._chats = new ObservableCollection<Chat>(_chat.Select());
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        private void ShowChatMessages()
        {
            this.ShownChatMessages = JsonConvert.DeserializeObject<List<MessageJSON>>(this._selectedChat.Messages);
            
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        /// <summary>
        /// Converts whole chat into a JSON
        /// </summary>
        /// <returns>String in JSON format with all messages in current chat</returns>
        public string ChatToJSON()
        {
            return JsonConvert.SerializeObject(ShownChatMessages, Formatting.Indented);
        }

        // Close chat button
        public ICommand CloseChatButton => new RelayCommand(ExecuteCloseChat);
        public static event EventHandler FromChatToContactEvent;

        // Send chat button
        public ICommand SendChatButton => new RelayCommand(SendChatButtonExecute);
        public static event EventHandler<ChatEventArgs> SendChatEvent;

        /// <summary>
        /// Function that executes when "Send" button has been pressed in Chat View.
        /// </summary>
        private void SendChatButtonExecute()
        {
            this.ExecuteSendChatEvent(this.ChatTextInput, MessageSender.Sender);

            //TODO: DOesnt work in constructor yet :p
            foreach (MessageJSON message in ShownChatMessages)
            {
                this.ExecuteSendChatEvent(message.Message, (MessageSender)Convert.ToInt32(message.UserRole));
            }
        }

        /// <summary>
        /// Closes the chat, returns to contact page
        /// </summary>
        private void ExecuteCloseChat()
        {
            FromChatToContactEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires event to CreateChatTextBlocKEvent() in View
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="messageSender">ENUM of who sent the message</param>
        private void ExecuteSendChatEvent(string message, MessageSender messageSender)
        {
            SendChatEvent?.Invoke(this, new ChatEventArgs(message, messageSender));
        }
    }
}
