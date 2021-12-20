using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class ServerCommunicator
    {
        private TcpClient _client;
        public static string IpAdress = "127.0.0.1";
        public static int port = 3360;
        public PacketReader _packetReader;

        public event Action connectedEvent;//will return nothing
        public event Action msgReceivedEvent;
        public event Action UserDisconnectedEvent;
        public ServerCommunicator()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer()
        {
            if (!this._client.Connected)
            {
                this._client.Connect(ServerCommunicator.IpAdress, ServerCommunicator.port);
                _packetReader = new PacketReader(this._client.GetStream());


                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            //check on other thread for incoming bytes
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            msgReceivedEvent?.Invoke();
                            break;
                        case 10:
                            UserDisconnectedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("Default in switch Chatclient.Server");
                            break;
                    }
                }
            });
        }


        public void SendMessageToServer(string message)
        {
            var messagePacket = new Packetbuilder();
            messagePacket.WriteToMemoryStream(5);
            messagePacket.WriteString(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }

        public bool isConnected()
        {
            return this._client.Connected;
        }
    }
}
