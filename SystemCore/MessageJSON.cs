using Newtonsoft.Json.Linq;
using System;

namespace SystemCore
{
    /// <summary>
    /// This class defines each chat message sent by the user or owner of the camping.
    /// </summary>
    public class MessageJSON
    {
        public string Message;
        public string MessageSentTime;
        public string UserRole; 
        
        public MessageJSON(string message, string userRole)
        {
            this.Message = message;
            this.MessageSentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.UserRole = userRole;
        }
    }
}
