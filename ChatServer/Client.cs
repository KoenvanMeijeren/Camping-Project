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
        public bool _isSuperClient;//campingowner
        public bool IsSuperClient
        {
            get => this._isSuperClient;
            private set
            {
                if (Equals(value, this._isSuperClient))
                {
                    return;
                }

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

                this._isSuperClient = value;
            }
        }
        public Guid UID { get; set; }//global unique identifier => for identifying a client
        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;

        public Client(TcpClient client)
        {
            this.ClientSocket = client;
            UID = Guid.NewGuid();
            this._packetReader = new PacketReader(this.ClientSocket.GetStream());

            //deze code loopt door op een andere thread
            Task.Run(() => Process());
        }

        //methode voor incoming berichten

        private void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
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
