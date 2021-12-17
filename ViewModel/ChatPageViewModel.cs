using System;
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
using System.Windows.Documents;

namespace ViewModel
{
    public class ChatPageViewModel : ObservableObject
    {
        // Close chat button
        public ICommand CloseChatButton => new RelayCommand(ExecuteCloseChat);
        public static event EventHandler FromChatToContactEvent;

        // Send chat button
        public ICommand SendChatButton => new RelayCommand(SendChatButtonExecute);

        // Text
        private string ChatTextInput;

        private void SendChatButtonExecute()
        {

        }

        private void ExecuteCloseChat()
        {
            FromChatToContactEvent?.Invoke(this, null);
        }
    }
}
