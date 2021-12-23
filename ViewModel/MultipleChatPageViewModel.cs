﻿using System;
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
        private List<MessageJSON> _shownChatMessages;
        private int _refreshRateInMilliseconds = 2000;

        public string ChatTextInput { get; set; }

        public string CurrenCustomerName { get; private set; }
        public static event EventHandler<ChatEventArgs> OpenChatEvent;
        public static event EventHandler<ChatEventArgs> NewSelectedChatEvent;


        #region property
        public List<MessageJSON> ShownChatMessages
        {
            get => this._shownChatMessages;
            set
            {
                if (value == this._shownChatMessages)
                {
                    return;
                }

                this._shownChatMessages = value;
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
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                NewSelectedChatEvent?.Invoke(this, null);
                GetChatConversation();
               
            }
        }
        #endregion


        // TODO: Fetch database row, loop through and display it
        public MultipleChatPageViewModel()
        {
            //TODO: fetch all chats from database, where unsolved?
            _chat = new Chat();
            this._chats = new ObservableCollection<Chat>(_chat.Select());
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            //this.RefreshChatMessages();//always listen for new chat messages?
        }


        public void DisplayMessages()
        {
            // Loops through all 'old'/already sent messages
            foreach (var message in this.ShownChatMessages)
            {
                OpenChatEvent?.Invoke(this, new ChatEventArgs(message.Message, (MessageSender)Convert.ToInt32(message.UserRole)));
            }            
        }

        private void GetChatConversation()
        {
            this._shownChatMessages = null;
            this.ShownChatMessages = JsonConvert.DeserializeObject<List<MessageJSON>>(this._selectedChat.Messages);
            DisplayMessages();
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        /// <summary>
        /// Async function that checks for new messages
        /// </summary>
        /// <returns>Nothing</returns>
        /*public async Task RefreshChatMessages()
        {
            // Automatically updating chats
            while (true)
            {
                foreach (Chat ChatConversation in _chats)
                {
                    // Fetch the messages from the database
                    string GetChatMessages = ChatConversation.GetChatMessagesForCampingGuest(CurrentUser.CampingCustomer.Account);
                    // Convert database JSON value to List<MesssageJson>
                    List<MessageJSON> GetChatMessagesToList = JsonConvert.DeserializeObject<List<MessageJSON>>(GetChatMessages);

                    // Check if the current chat does NOT match with chats in database (aka new message)
                    if (!ChatConversation.ChatMessages.Count.Equals(GetChatMessagesToList.Count))
                    {
                        // Calculate the amount of new messages
                        int differenceBetweenCountOfMessages = GetChatMessagesToList.Count - ChatConversation.ChatMessages.Count;

                        // Loop from first new message, to last new message
                        for (int i = ChatConversation.ChatMessages.Count; i < GetChatMessagesToList.Count; i++)
                        {
                            MessageSender chatMessageSender = (MessageSender)Convert.ToInt32(GetChatMessagesToList[i].UserRole);
                            this.ExecuteSendChatEvent(GetChatMessagesToList[i].Message, chatMessageSender);
                        }
                        // Overwrite the old list with messages to the full new list with messages
                        ChatConversation.ChatMessages = GetChatMessagesToList;
                    }

                    // Async wait before executing this again
                    await Task.Delay(_refreshRateInMilliseconds);
                }               
            }
        }*/

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
