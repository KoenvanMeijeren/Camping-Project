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

namespace ViewModel
{
    public enum MessageSender
    {
        Sender = 0,
        Receiver = 1
    }

    public class ChatPageViewModel : ObservableObject
    {
        public string ChatTextInput { get; set; }
        public List<MessageJSON> ChatMessages { get; private set; }

        private Chat _chatModel = new Chat();
        public Chat ChatConversation { get; private set; }

        private int _refreshRateInMilliseconds = 2000;

        public ChatPageViewModel()
        {
            // Executes when user has logged in
            SignInViewModel.SignInEvent += ExecuteChatAfterLogin;
            AccountViewModel.SignOutEvent += this.OnSignOutEvent;

        }

        private void OnSignOutEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //TODO: stop running task
        }

        public void ExecuteChatAfterLogin(object o, AccountEventArgs accountEventArgs)
        {
            if (CurrentUser.Account.Rights == AccountRights.Admin)
            {
                return;
            }

            this.ChatConversation = _chatModel.SelectOrCreateNewChatForLoggedInUser(CurrentUser.CampingCustomer);
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
            while (true)
            {
                // Fetch the messages from the database
                string GetChatMessages = ChatConversation.GetChatMessagesForCampingCustomer(CurrentUser.CampingCustomer.Account);
                // Convert database JSON value to List<MesssageJson>
                List<MessageJSON> GetChatMessagesToList = JsonConvert.DeserializeObject<List<MessageJSON>>(GetChatMessages);

                // Check if the current chat does NOT match with chats in database (aka new message)
                if (!this.ChatMessages.Count.Equals(GetChatMessagesToList.Count))
                {
                    // Calculate the amount of new messages
                    int differenceBetweenCountOfMessages = GetChatMessagesToList.Count - this.ChatMessages.Count;

                    // Loop from first new message, to last new message
                    for (int i = this.ChatMessages.Count; i < GetChatMessagesToList.Count; i++)
                    {
                        MessageSender chatMessageSender = (MessageSender)Convert.ToInt32(GetChatMessagesToList[i].UserRole);
                        this.ExecuteSendChatEvent(GetChatMessagesToList[i].Message, chatMessageSender);
                    }
                    // Overwrite the old list with messages to the full new list with messages
                    this.ChatMessages = GetChatMessagesToList;
                }

                // Async wait before executing this again
                await Task.Delay(_refreshRateInMilliseconds);
            }
        }

        /// <summary>
        /// Converts whole chat into a JSON
        /// </summary>
        /// <returns>String in JSON format with all messages in current chat</returns>
        public string ChatToJSON()
        {
            return JsonConvert.SerializeObject(ChatMessages, Formatting.Indented);
        }

        // Close chat button
        public ICommand CloseChatButton => new RelayCommand(ExecuteCloseChat);
        public static event EventHandler FromChatToContactEvent;

        // Send chat button
        public ICommand SendChatButton => new RelayCommand(SendChatButtonExecute);
        public static event EventHandler<ChatEventArgs> SendChatEvent;
        public static event EventHandler<ChatEventArgs> OpenChatEvent;

        /// <summary>
        /// Function that executes when "Send" button has been pressed in Chat View.
        /// </summary>
        private void SendChatButtonExecute()
        {
            // Message that has just been sent
            string sentMessage = this.ChatTextInput;

            //Check if the message was sent by guest or owner
            MessageSender sndr = (ChatConversation.Customer.Id.Equals(CurrentUser.Account.Id)) ? MessageSender.Sender : MessageSender.Receiver;

            // Displays message on screen
            this.ExecuteSendChatEvent(sentMessage, sndr);

            // Add message to whole conversation
            this.ChatMessages.Add(new MessageJSON(sentMessage, Convert.ToInt32(sndr).ToString()));

            this.UpdateChatInDatabase();
        }

        /// <summary>
        /// Updates the chat converstion.
        /// </summary>
        private void UpdateChatInDatabase()
        {
            // List of text messages to JSON
            var messagesListToJson = JsonConvert.SerializeObject(ChatMessages, Formatting.Indented);
            this.ChatConversation.UpdateChat(messagesListToJson);
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
