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

namespace ViewModel
{
    public class MultipleChatPageViewModel : ObservableObject
    {
        public string ChatTextInput { get; set; }
        //niet nodig bij owner?
        public List<MessageJSON> ChatMessages { get; private set; }
        public ObservableCollection<Chat> Chats { get; private set; }

        // TODO: Fetch database row, loop through and display it
        public MultipleChatPageViewModel()
        {
            //TODO: fetch all chats from database
            Chat chat = new Chat();
            this.Chats = new ObservableCollection<Chat>(chat.Select());
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

        /// <summary>
        /// Function that executes when "Send" button has been pressed in Chat View.
        /// </summary>
        private void SendChatButtonExecute()
        {
            this.ExecuteSendChatEvent(this.ChatTextInput, MessageSender.Sender);

            //TODO: DOesnt work in constructor yet :p
            foreach (MessageJSON message in ChatMessages)
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
