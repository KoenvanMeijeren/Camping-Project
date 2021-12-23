using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.EventArguments
{
    public class ChatEventArgs : EventArgs
    {

        public String message;
        public MessageSender messageSender;

        public ChatEventArgs(string message, MessageSender messageSender)
        {
            this.message = message;
            this.messageSender = messageSender;
        }
    }
}
