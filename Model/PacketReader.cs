using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class PacketReader : BinaryReader
    { 
        private Stream _networkStream;
        public PacketReader(Stream ns) : base(ns)
        {
            this._networkStream = ns;
        }

        public string ReadMessage()
        {
            byte[] messageBuffer;
            var length = ReadInt32();
            messageBuffer = new byte[length];
            _networkStream.Read(messageBuffer, 0, length);

            var message = Encoding.ASCII.GetString(messageBuffer);
            return message;
        }
    }
}
