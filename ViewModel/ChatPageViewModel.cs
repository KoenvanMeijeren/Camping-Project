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

        public ChatPageViewModel()
        {
            // Executes when user has logged in
            SignInViewModel.SignInEvent += ExecuteChatAfterLogin;
        }

        public void ExecuteChatAfterLogin(object o, AccountEventArgs accountEventArgs)
        {
            this.ChatConversation = _chatModel.SelectOrCreateNewChatForLoggedInUser(CurrentUser.CampingCustomer);
            this.ChatMessages = JsonConvert.DeserializeObject<List<MessageJSON>>(this.ChatConversation.Messages);

            // Loops through all 'old'/already sent messages
            foreach(var message in ChatMessages)
            {
                OpenChatEvent?.Invoke(this, new ChatEventArgs(message.Message, (MessageSender)Convert.ToInt32(message.UserRole)));
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
            MessageSender sndr = (ChatConversation.Customer.Id.Equals(CurrentUser.Account.Id)) ? MessageSender.Receiver : MessageSender.Sender;

            // Displays message on screen
            this.ExecuteSendChatEvent(sentMessage, sndr);

            // Add message to whole conversation
            ChatMessages.Add(new MessageJSON(sentMessage, Convert.ToInt32(sndr).ToString()));

            this.UpdateChatInDatabase();
        }

        private void UpdateChatInDatabase()
        {
            DateTime? LastMessageSeenOwner;
            DateTime? LastMessageSeenCustomer;

            // Check if message was sent by owner, if yes update his time
            if (CurrentUser.Account.Id == ChatConversation.Owner.Id)
            {
                LastMessageSeenOwner = DateTime.Now;
            }

            // Check if message was sent by customer, if yes update his time
            if (CurrentUser.Account.Id == ChatConversation.Customer.Id)
            {
                LastMessageSeenCustomer = DateTime.Now;
            }

            // List of text messages to JSON
            var messagesListToJson = JsonConvert.SerializeObject(ChatMessages, Formatting.Indented);
            ChatConversation.UpdateChat(messagesListToJson);
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
