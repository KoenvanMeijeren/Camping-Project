﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using SystemCore;
using ViewModel.EventArguments;
using System.Windows.Controls;

namespace ViewModel
{
    public enum MessageSender
    {
        Sender = 0,
        Reciever = 1
    }

    public class ChatPageViewModel : ObservableObject
    {
        // Close chat button
        public ICommand CloseChatButton => new RelayCommand(ExecuteCloseChat);
        public static event EventHandler FromChatToContactEvent;

        // Send chat button
        public ICommand SendChatButton => new RelayCommand(SendChatButtonExecute);

        // Text
        private string _chatTextInput;
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
            }
        }

        private void SendChatButtonExecute()
        {
            String test = ChatTextInput;
            
            ChatTextInput = "";
        }

        /// <summary>
        /// Closes the chat, returns to contact page
        /// </summary>
        private void ExecuteCloseChat()
        {
            FromChatToContactEvent?.Invoke(this, null);
        }
    }
}
