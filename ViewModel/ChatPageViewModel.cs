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
        private string _chatTextInput;
        private bool _stopAsyncTask;
        public List<MessageJSON> ChatMessages { get; private set; }

        private readonly Chat _chatModel = new Chat();
        public Chat ChatConversation { get; private set; }

        private readonly int _refreshRateInMilliseconds = 2000;

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
        public static event EventHandler<ChatEventArgs> SendChatEvent;
        public static event EventHandler<ChatEventArgs> OpenChatEvent;

        public ChatPageViewModel()
        {
            this.ChatTextInput = "";
            this.ChatMessages = new List<MessageJSON>();
            this._stopAsyncTask = true;
            this.ChatConversation = new Chat();
            
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
            this._stopAsyncTask = false;
        }

        private void ExecuteChatAfterLogin(object sender, AccountEventArgs accountEventArgs)
        {
            if (CurrentUser.Account.Rights == AccountRights.Admin)
            {
                return;
            }
            this._stopAsyncTask = false ;

            this.ChatConversation = this._chatModel.SelectOrCreateNewChatForLoggedInUser(CurrentUser.CampingCustomer);
            this.ChatMessages = JsonConvert.DeserializeObject<List<MessageJSON>>(this.ChatConversation.Messages);

            // Loops through all 'old'/already sent messages
            foreach(var message in this.ChatMessages)
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
                // Fetch the messages from the database
                string GetChatMessages = this.ChatConversation.GetChatMessagesForCampingCustomer(this.ChatConversation.Customer);

                // Convert database JSON value to List<MesssageJson>
                List<MessageJSON> messageList = JsonConvert.DeserializeObject<List<MessageJSON>>(GetChatMessages);

                // Check if the current chat does NOT match with chats in database (aka new message)
                if (!this.ChatMessages.Count.Equals(messageList.Count))
                {
                    // Calculate the amount of new messages
                    int differenceBetweenCountOfMessages = messageList.Count - this.ChatMessages.Count;

                    // Loop from first new message, to last new message
                    for (int i = this.ChatMessages.Count; i < messageList.Count; i++)
                    {
                        MessageSender chatMessageSender = (MessageSender)Convert.ToInt32(messageList[i].UserRole);
                        this.ExecuteSendChatEvent(messageList[i].Message, chatMessageSender);
                    }
                    // Overwrite the old list with messages to the full new list with messages
                    this.ChatMessages = messageList;
                }

                // Async wait before executing this again
                await Task.Delay(this._refreshRateInMilliseconds);
            }
        }

        // Close chat button
        public ICommand CloseChatButton => new RelayCommand(ExecuteCloseChat);
        public static event EventHandler FromChatToContactEvent;

        // Send chat button
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
            MessageSender sender = (this.ChatConversation.Customer.Id.Equals(CurrentUser.Account.Id)) ? MessageSender.Sender : MessageSender.Receiver;

            // Displays message on screen
            this.ExecuteSendChatEvent(sentMessage, sender);

            // Add message to whole conversation
            this.ChatMessages.Add(new MessageJSON(sentMessage, Convert.ToInt32(sender).ToString()));

            this.UpdateChatInDatabase();
        }

        /// <summary>
        /// Updates the chat conversation.
        /// </summary>
        private void UpdateChatInDatabase()
        {
            this.ChatConversation.UpdateChat(JsonConvert.SerializeObject(this.ChatMessages, Formatting.Indented));
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
