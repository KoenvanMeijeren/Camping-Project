using Newtonsoft.Json.Linq;
using System;

namespace SystemCore
{

    /// <summary>
    /// Role of who sent the message.
    /// </summary>
    public enum Role
    {
        Customer = 0,
        Admin = 1

    }
    public class MessageJSON
    {
        public string Message;
        public DateTime MessageSentTime;
        public Role UserRole;

        public MessageJSON(string json)
        {
            JObject jObject = JObject.Parse(json);

            this.Message = (string)jObject["message"];
            this.MessageSentTime = DateTimeParser.TryParse((string)jObject["messageSent"]);
            this.UserRole = (Role)(Convert.ToInt32((string)jObject["role"]));
        }
    }
}
