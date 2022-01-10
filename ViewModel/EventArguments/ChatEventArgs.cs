using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.EventArguments
{
    public class ChatEventArgs : EventArgs
    {

        public readonly string Message;
        public readonly MessageSender MessageSender;

        public ChatEventArgs(string message, MessageSender messageSender)
        {
            this.Message = message;
            this.MessageSender = messageSender;
        }
    }
}
