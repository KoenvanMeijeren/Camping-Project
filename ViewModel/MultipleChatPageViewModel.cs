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
        #region Fields

        private readonly Chat _chatModel = new Chat();
        
        private Chat _selectedChat;
        private ObservableCollection<Chat> _chats;
        private List<MessageJson> _shownChatMessages;
        private string _chatTextInput;
        
        private bool _stopAsyncTask;

        private const int RefreshRateInMilliseconds = 2000;
        
        #endregion

        #region Properties
        public string CurrentCustomerName { get; private set; }
        public string ChatTextInput
        {
            get => this._chatTextInput;
            set
            {
                if (value == this._chatTextInput)
                {
                    return;
                }

                this._chatTextInput = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }
        public List<MessageJson> ShownChatMessages
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
                if (value == null)
                {
                    return;
                }
                
                this.CurrentCustomerName = this._selectedChat.CustomerName;
                this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                
                MultipleChatPageViewModel.NewChatContentEvent?.Invoke(this, null);
                this.GetChatConversation();               
            }
        }
        #endregion

        #region Events

        public static event EventHandler<ChatEventArgs> NewChatContentEvent;
        public static event EventHandler<ChatEventArgs> SendChatEvent;
        
        public static event EventHandler FromChatToContactEvent;

        #endregion

        #region View construction

        public MultipleChatPageViewModel()
        {
            SignInViewModel.SignInEvent += this.ExecuteChatAfterLogin;
            AccountViewModel.SignOutEvent += this.OnSignOutEvent;

            this._shownChatMessages = new List<MessageJson>();
            this._chatTextInput = "";
            this._chats = this.GetAllChats();
            this.CurrentCustomerName = "Klant";
            this._stopAsyncTask = false;
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
            
            this.RefreshAllChatMessages();
            this.RefreshChats();
            this.RefreshSelectedChatMessages();

        }

        private void ExecuteChatAfterLogin(object sender, AccountEventArgs e)
        {
            this._stopAsyncTask = false;
            this.RefreshAllChatMessages();
            this.RefreshChats();
            this.RefreshSelectedChatMessages();
        }

        /// <summary>
        /// Updates chats. 
        /// </summary>
        /// <returns></returns>
        private async Task RefreshChats()
        {
            // Automatically updating chats
            while (!this._stopAsyncTask)
            {
                var chatDb = this.GetAllChats();
                
                // Check for new chats
                if (this._chats.Count != chatDb.Count)
                {
                    var selectedChat = this.SelectedChat;
                    this._chats.Clear();
                    this.Chats = chatDb;
                    this.SelectedChat = selectedChat;
                }

                // Async wait before executing this again
                await Task.Delay(RefreshRateInMilliseconds);
            }
        }

        /// <summary>
        /// stop async tasks when logging out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSignOutEvent(object sender, EventArgs e)
        {
            this._stopAsyncTask = true;
        }

        /// <summary>
        /// This method will load the messages in a textblock on the screen
        /// </summary>
        private void DisplayMessages()
        {
            // Loops through all 'old'/already sent messages
            foreach (var message in this.ShownChatMessages)
            {
                MultipleChatPageViewModel.SendChatEvent?.Invoke(this, new ChatEventArgs(message.Message, (MessageSender)Convert.ToInt32(message.UserRole)));
            }            
        }
        
        /// <summary>
        /// Updates messages list and display new list
        /// </summary>
        private void GetChatConversation()
        {
            this._shownChatMessages.Clear();
            this.ShownChatMessages = JsonConvert.DeserializeObject<List<MessageJson>>(this._selectedChat.Messages);
            this.DisplayMessages();
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));
        }
        
         /// <summary>
        /// Async function that checks for new messages
        /// </summary>
        /// <returns>Nothing</returns>
        private async Task RefreshAllChatMessages()
        {           
            // Check if current user is camping owner.
            if (CurrentUser.Account == null || CurrentUser.Account.Rights == AccountRights.Customer) 
            {
                return;
            }

            // Automatically updating chats
            while (!this._stopAsyncTask)
            {               
                foreach (Chat chatConversation in this._chats)
                {
                    // Move to next chat.
                    if(this._selectedChat != null && this._selectedChat.Customer.Id != chatConversation.Customer.Id)
                    {
                        continue;
                    }
                    
                    List<MessageJson> loadedChatMessages = JsonConvert.DeserializeObject<List<MessageJson>>(chatConversation.Messages);
                    
                    // Fetch the messages from the database
                    string dbChatMessages = chatConversation.GetChatMessagesForCampingCustomer(chatConversation.Customer);

                    // Convert database JSON value to List<MessageJson>
                    List<MessageJson> chatMessages = JsonConvert.DeserializeObject<List<MessageJson>>(dbChatMessages);

                    // Check if the current chat does NOT match with chats in database (aka new message)
                    if (loadedChatMessages != null && chatMessages != null && !loadedChatMessages.Count.Equals(chatMessages.Count))
                    {
                        // Overwrite the old list with messages to the full new list with messages in chat object
                        this.UpdateChatInList(chatConversation, chatMessages);
                    }
                    
                    // Async wait before executing this again
                    await Task.Delay(RefreshRateInMilliseconds);
                }
            }
        }

        /// <summary>
        /// Async function that checks for new messages in  SELECTED chat
        /// </summary>
        /// <returns>Nothing</returns>
        private async Task RefreshSelectedChatMessages()
        {
            // Automatically updating chat
            while (!this._stopAsyncTask)
            {
                if(this._selectedChat != null)
                {
                    // Fetch the messages from the database.
                    List<MessageJson> loadedChatMessages = JsonConvert.DeserializeObject<List<MessageJson>>(this._selectedChat.Messages);
                    string dbChatMessages = this._selectedChat.GetChatMessagesForCampingCustomer(this._selectedChat.Customer);
                    
                    // Convert database JSON value to List<MessageJson>
                    List<MessageJson> chatMessages = JsonConvert.DeserializeObject<List<MessageJson>>(dbChatMessages);

                    // Check if the current chat does NOT match with chats in database (aka new message)
                    if (loadedChatMessages != null && chatMessages != null && !loadedChatMessages.Count.Equals(chatMessages.Count))
                    {
                        // Loop from first new message, to last new message
                        for (int i = loadedChatMessages.Count; i < chatMessages.Count; i++)
                        {
                            MessageSender chatMessageSender = (MessageSender)Convert.ToInt32(chatMessages[i].UserRole);
                            this.ExecuteSendChatEvent(chatMessages[i].Message, chatMessageSender);
                        }
                        
                        // Overwrite the old list with messages to the full new list with messages
                        this.UpdateChatInList(this._selectedChat, chatMessages);
                        this.GetChatConversation();
                        MultipleChatPageViewModel.NewChatContentEvent?.Invoke(this, null);
                    }
                }                

                // Async wait before executing this again
                await Task.Delay(RefreshRateInMilliseconds);
            }
        }

        /// <summary>
        /// Updates chat messages 
        /// </summary>
        /// <param name="chatConversation">chat that needs updated chat messages</param>
        /// <param name="chatMessages">new chat message list</param>
        private void UpdateChatInList(Chat chatConversation, List<MessageJson> chatMessages)
        {
            foreach (var chat in this._chats.Where(chat => chat.Customer.Id == chatConversation.Customer.Id))
            {
                chat.Messages = this.ChatToJson(chatMessages);
            }
        }

        /// <summary>
        /// Converts whole chat into a JSON
        /// </summary>
        /// <returns>String in JSON format with all messages in current chat</returns>
        /// 
        private string ChatToJson(List<MessageJson> messages)
        {
            return JsonConvert.SerializeObject(messages, Formatting.Indented);
        }
        
        #endregion

        #region Commands

        public ICommand CloseChatButton => new RelayCommand(ExecuteCloseChat);

        public ICommand SendChatButton => new RelayCommand(SendChatButtonExecute, CanExecuteSendChatButtonExecute);
        
        private bool CanExecuteSendChatButtonExecute()
        {
            return this.ChatTextInput.Length > 0 && this._selectedChat!=null;
        }

        /// <summary>
        /// Function that executes when "Send" button has been pressed in Chat View.
        /// </summary>
        private void SendChatButtonExecute()
        {
            // Message that has just been sent
            string sentMessage = this.ChatTextInput;

            //Check if the message was sent by guest or owner
            MessageSender sender = (this._selectedChat.Customer.Id.Equals(CurrentUser.Account.Id)) ? MessageSender.Sender : MessageSender.Receiver;

            // Displays message on screen
            this.ExecuteSendChatEvent(sentMessage, sender);

            // Add message to whole conversation
            this._shownChatMessages.Add(new MessageJson(sentMessage, Convert.ToInt32(sender).ToString()));
            this.UpdateChatInList(this._selectedChat, this._shownChatMessages);
            this.OnPropertyChanged(new PropertyChangedEventArgs(null));

            this.UpdateChatInDatabase();
        }

        /// <summary>
        /// Updates the chat conversation.
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
            MultipleChatPageViewModel.FromChatToContactEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires event to CreateChatTextBlocKEvent() in View
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="messageSender">ENUM of who sent the message</param>
        private void ExecuteSendChatEvent(string message, MessageSender messageSender)
        {
            MultipleChatPageViewModel.SendChatEvent?.Invoke(this, new ChatEventArgs(message, messageSender));
            this.ChatTextInput = "";
        }

        #endregion

        #region Database interaction

        /// <summary>
        /// Get all chats from database
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<Chat> GetAllChats()
        {
            return new ObservableCollection<Chat>(this._chatModel.Select());
        }

        #endregion
    }
}
