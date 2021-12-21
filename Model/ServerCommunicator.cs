using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Visualization
{
    public class ServerCommunicator
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
            this._client = new TcpClient();
        }

        public void ConnectToServer()
        {
            if (!this._client.Connected)
            {
                this._client.Connect(ServerCommunicator.IpAdress, ServerCommunicator.port);
                this._packetReader = new PacketReader(this._client.GetStream());

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
                    var taskCode = this._packetReader.ReadByte();//get first packet
                    switch (taskCode)
                    {
                        case 2:
                            connectedEvent?.Invoke();
                            break;
                        case 6:
                            msgReceivedEvent?.Invoke();
                            break;
                        case 9:
                            UserDisconnectedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine($"No clear taskCode code received: {taskCode}");
                            break;
                    }
                }
            });
        }


        public void SendMessageToServer(string message)
        {
            Packetbuilder messagePacket = new Packetbuilder();
            messagePacket.WriteTaskTypeToMemoryStream(6);
            messagePacket.WriteMessageToStream(message);
            this._client.Client.Send(messagePacket.GetPacketBytes());
        }

        public bool isConnected()
        {
            return this._client.Connected;
        }
    }
}
