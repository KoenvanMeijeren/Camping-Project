using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {
        public string Username { get; set; }
        public bool IsSuperClient { get; private set; }//campingowner
        public Guid UID { get; set; }//global unique identifier => for identifying a client
        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;


/*        public Client(TcpClient client)
        {
            this.ClientSocket = client;
            UID = Guid.NewGuid();
            this._packetReader = new PacketReader(this.ClientSocket.GetStream());
            this.IsSuperClient = false;

            //deze code loopt door op een andere thread
            Task.Run(() => Process());
        }*/

        public Client(TcpClient client, bool isSuperClient)
        {
            this.ClientSocket = client;
            UID = Guid.NewGuid();
            this._packetReader = new PacketReader(this.ClientSocket.GetStream());
            this.IsSuperClient = isSuperClient;

            if (IsSuperClient)
            {
                this.Username = "Campingeigenaar";
                Console.WriteLine($"[{DateTime.Now}]: SUPERClient has connected");
            }
            else
            {
                this.Username = "Klant";
                Console.WriteLine($"[{DateTime.Now}]: Client has connected");
            }

        }


        private void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 0:
                            Console.WriteLine($"[{DateTime.Now}] : Campingowner has joined the chat");
                            this.IsSuperClient = true;
                            break;
                        case 1:
                            Console.WriteLine($"[{DateTime.Now}] : Klant has joined the chat");
                            break;
                        case 6:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}] : Message received! {msg} ");
                            Program.ShowAllMessages($"[{DateTime.Now}]: {msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[{UID.ToString()}]: Disconnected!");
                    Program.Disconnect(this.UID.ToString());
                    ClientSocket.Close();
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
