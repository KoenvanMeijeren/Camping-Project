using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class PacketBuilder
    {
        private MemoryStream _ms;
        public PacketBuilder()
        {
            this._ms = new MemoryStream();
        }

        public void WriteTypeOfMessage(byte data)
        {
            this._ms.WriteByte(data);
        }

        public void WriteMessage(string message)
        {
            var msgLength = message.Length;
            _ms.Write(BitConverter.GetBytes(msgLength));
            _ms.Write(Encoding.ASCII.GetBytes(message));
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
