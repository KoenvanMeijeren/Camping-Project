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
using System.Threading;
using System.ComponentModel;

namespace ViewModel
{
    //From customer perspective
    public enum MessageSender
    {
        Sender = 0,
        Receiver = 1
    }

    public class ChatPageViewModel : ObservableObject
    {
        #region Fields

        private readonly Chat _chatModel = new Chat();
        
        private string _chatTextInput;
        private bool _stopAsyncTask;

        private List<MessageJson> _chatMessages;
        private Chat _chatConversation;

        private const int RefreshRateInMilliseconds = 2000;
        
        #endregion

        #region Properties

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

        #endregion

        #region Events

        public static event EventHandler<ChatEventArgs> SendChatEvent;
        public static event EventHandler<ChatEventArgs> OpenChatEvent;
        
        public static event EventHandler FromChatToContactEvent;

        #endregion

        #region View constructions

        public ChatPageViewModel()
        {
            this._chatMessages = new List<MessageJson>();
            this._stopAsyncTask = true;
            this._chatConversation = new Chat();
            
            // This triggers the on property changed event.
            this.ChatTextInput = "";
            
            SignInViewModel.SignInEvent += this.ExecuteChatAfterLogin;
            AccountViewModel.SignOutEvent += this.OnSignOutEvent;
        }

        /// <summary>
        /// stop async tasks when logging out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSignOutEvent(object sender, EventArgs e)
        {
            this._chatMessages.Clear();
            this._stopAsyncTask = false;
        }

        private void ExecuteChatAfterLogin(object sender, AccountEventArgs accountEventArgs)
        {
            if (CurrentUser.Account.Rights == AccountRights.Admin)
            {
                return;
            }
            
            this._stopAsyncTask = false;
            this._chatConversation = this._chatModel.SelectOrCreateNewChatForLoggedInUser(CurrentUser.CampingCustomer);
            this._chatMessages = JsonConvert.DeserializeObject<List<MessageJson>>(this._chatConversation.Messages);
            if (this._chatMessages == null)
            {
                return;
            }

            // Loops through all 'old'/already sent messages
            foreach(var message in this._chatMessages)
            {
                OpenChatEvent?.Invoke(this, new ChatEventArgs(message.Message, (MessageSender)Convert.ToInt32(message.UserRole)));
            }

            this.RefreshChatMessages();
        }

        /// <summary>
        /// Async function that checks for new messages
        /// </summary>
        /// <returns>Nothing</returns>
        private async Task RefreshChatMessages()
        {
            // Automatically updating chat
            while (!this._stopAsyncTask)
            {
                // Fetch the messages from the database.
                string dbChatMessages = this._chatConversation.GetChatMessagesForCampingCustomer(this._chatConversation.Customer);

                // Convert database JSON value to List<MessageJson>
                List<MessageJson> messageList = JsonConvert.DeserializeObject<List<MessageJson>>(dbChatMessages);

                // Check if the current chat does NOT match with chats in database (aka new message)
                if (messageList != null && !this._chatMessages.Count.Equals(messageList.Count))
                {
                    // Loop from first new message, to last new message
                    for (int i = this._chatMessages.Count; i < messageList.Count; i++)
                    {
                        MessageSender chatMessageSender = (MessageSender)Convert.ToInt32(messageList[i].UserRole);
                        this.ExecuteSendChatEvent(messageList[i].Message, chatMessageSender);
                    }
                    // Overwrite the old list with messages to the full new list with messages
                    this._chatMessages = messageList;
                }

                // Async wait before executing this again
                await Task.Delay(RefreshRateInMilliseconds);
            }
        }

        #endregion

        #region Commands

        public ICommand CloseChatButton => new RelayCommand(ExecuteCloseChat);

        public ICommand SendChatButton => new RelayCommand(SendChatButtonExecute, CanExecuteSendChatButtonExecute);

        private bool CanExecuteSendChatButtonExecute()
        {
            return this.ChatTextInput.Length > 0;
        }

        /// <summary>
        /// Function that executes when "Send" button has been pressed in Chat View.
        /// </summary>
        private void SendChatButtonExecute()
        {
            // Message that has just been sent
            string sentMessage = this.ChatTextInput;

            //Check if the message was sent by guest or owner
            MessageSender sender = (this._chatConversation.Customer.Id.Equals(CurrentUser.Account.Id)) ? MessageSender.Sender : MessageSender.Receiver;

            // Displays message on screen
            this.ExecuteSendChatEvent(sentMessage, sender);

            // Add message to whole conversation
            this._chatMessages.Add(new MessageJson(sentMessage, Convert.ToInt32(sender).ToString()));

            this.UpdateChatInDatabase();
        }

        /// <summary>
        /// Updates the chat conversation.
        /// </summary>
        private void UpdateChatInDatabase()
        {
            this._chatConversation.UpdateChat(JsonConvert.SerializeObject(this._chatMessages, Formatting.Indented));
        }

        /// <summary>
        /// Closes the chat, returns to contact page
        /// </summary>
        private void ExecuteCloseChat()
        {
            ChatPageViewModel.FromChatToContactEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires event to CreateChatTextBlocKEvent() in View
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="messageSender">ENUM of who sent the message</param>
        private void ExecuteSendChatEvent(string message, MessageSender messageSender)
        {
            ChatPageViewModel.SendChatEvent?.Invoke(this, new ChatEventArgs(message, messageSender));
        }

        #endregion
    }
}
