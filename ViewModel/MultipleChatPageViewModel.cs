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
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModel
{
    public class MultipleChatPageViewModel : ObservableObject
    {
        private Chat _selectedChat;
        private readonly Chat _chatModel = new Chat();
        
        private ObservableCollection<Chat> _chats;
        private List<MessageJSON> _shownChatMessages;        
        private int _refreshRateInMilliseconds = 2000;

        public string ChatTextInput { get; set; }
        public string CurrentCustomerName { get; private set; }
        public static event EventHandler<ChatEventArgs> OpenChatEvent;
        public static event EventHandler<ChatEventArgs> NewChatContentEvent;


        #region properties
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
                this.CurrentCustomerName = this._selectedChat.CustomerName;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                NewChatContentEvent?.Invoke(this, null);
                GetChatConversation();               
            }
        }
        #endregion


        public MultipleChatPageViewModel()
        {
            //TODO: fetch all chats from database, where unsolved?
            this._shownChatMessages = new List<MessageJSON>();
            this._chats = GetAllChats();
            this.CurrentCustomerName = "Klant";
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            this.RefreshChatMessages();//always listen for new chat messages?
            this.RefreshChats();
        }

        private ObservableCollection<Chat> GetAllChats()
        {
            return new ObservableCollection<Chat>(_chatModel.Select());
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
            this._shownChatMessages.Clear();
            this.ShownChatMessages = JsonConvert.DeserializeObject<List<MessageJSON>>(this._selectedChat.Messages);
            DisplayMessages();
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }

        /// <summary>
        /// Async function that checks for new chats
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task RefreshChats()
        {
            // Automatically updating chats
            while (true)
            {
                ObservableCollection<Chat> chatDb = GetAllChats();
               if (this._chats.Count != chatDb.Count)//Check for new chats
               {
                    this.Chats = chatDb;
               }

                // Async wait before executing this again
                await Task.Delay(_refreshRateInMilliseconds);
            }
        }

        /// <summary>
        /// Async function that checks for new messages
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task RefreshChatMessages()
        {
            // Automatically updating chats
            while (true)
            {
                foreach (Chat chatConversation in _chats)
                {
                    List<MessageJSON> _chatMessagesInApplication = JsonConvert.DeserializeObject<List<MessageJSON>>(chatConversation.Messages);
                    // Fetch the messages from the database
                    string GetChatMessagesFromDb = chatConversation.GetChatMessagesForCampingCustomer(chatConversation.Customer);
                    // Convert database JSON value to List<MesssageJson>
                    List<MessageJSON> GetChatMessagesToList = JsonConvert.DeserializeObject<List<MessageJSON>>(GetChatMessagesFromDb);

                    // Check if the current chat does NOT match with chats in database (aka new message)
                    if (!_chatMessagesInApplication.Count.Equals(GetChatMessagesToList.Count))
                    {
                        // Calculate the amount of new messages
                        int differenceBetweenCountOfMessages = GetChatMessagesToList.Count - _chatMessagesInApplication.Count;

                        // Loop from first new message, to last new message
                        for (int i = _chatMessagesInApplication.Count; i < GetChatMessagesToList.Count; i++)
                        {
                            MessageSender chatMessageSender = (MessageSender)Convert.ToInt32(GetChatMessagesToList[i].UserRole);
                            this.ExecuteSendChatEvent(GetChatMessagesToList[i].Message, chatMessageSender);
                        }

                        // Overwrite the old list with messages to the full new list with messages
                        foreach (var chat in this._chats.Where(c => c.Customer.Id == chatConversation.Customer.Id))
                        {
                            chat.Messages = ChatToJSON(GetChatMessagesToList); 
                        }

                        //update chat
                        NewChatContentEvent?.Invoke(this, null);
                        if (chatConversation.Customer.Id == this._selectedChat.Customer.Id)
                        {
                            GetChatConversation();
                        }


                    }

                    // Async wait before executing this again
                    await Task.Delay(_refreshRateInMilliseconds);
                }
            }
        }

        /// <summary>
        /// Converts whole chat into a JSON
        /// </summary>
        /// <returns>String in JSON format with all messages in current chat</returns>
        /// 
        public string ChatToJSON(List<MessageJSON> messages)
        {
            return JsonConvert.SerializeObject(messages, Formatting.Indented);
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
            // Message that has just been sent
            string sentMessage = this.ChatTextInput;

            //Check if the message was sent by guest or owner
            MessageSender sndr = (this.SelectedChat.Customer.Id.Equals(CurrentUser.Account.Id)) ? MessageSender.Sender : MessageSender.Receiver;

            // Displays message on screen
            this.ExecuteSendChatEvent(sentMessage, sndr);

            // Add message to whole conversation
            this._shownChatMessages.Add(new MessageJSON(sentMessage, Convert.ToInt32(sndr).ToString()));

            this.OnPropertyChanged(new PropertyChangedEventArgs(null));

            this.UpdateChatInDatabase();
        }

        /// <summary>
        /// Updates the chat converstion.
        /// </summary>
        private void UpdateChatInDatabase()
        {
            // List of text messages to JSON
            var messagesListToJson = JsonConvert.SerializeObject(this._shownChatMessages, Formatting.Indented);
            this._selectedChat.UpdateChat(messagesListToJson);
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
            this.ChatTextInput = "";
        }
    }
}
