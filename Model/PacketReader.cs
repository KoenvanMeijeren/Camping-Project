using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Visualization
{
    public class PacketReader : BinaryReader
    { 
        private Stream _networkStream;
        public PacketReader(Stream ns) : base(ns)
        {
            this._networkStream = ns;
        }

        public string ReadIncomingMessage()
        {
            byte[] messageBuffer;
            var length = ReadInt32();//reads first 4 byte from stream
            messageBuffer = new byte[length];
            _networkStream.Read(messageBuffer, 0, length);

            return Encoding.ASCII.GetString(messageBuffer);
        }
    }
}
