using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {        
        static TcpListener _listener;
        public static string IpAdress = "127.0.0.1";
        public static int port = 3360;
        static List<Client> _users;
        private static Client CampingOwner { get; set; }

        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse(IpAdress), port);
            _listener.Start();

            //altijd luisteren naar nieuw clients
            while (true)
            {
                int index = _users.FindIndex(user => user.IsSuperClient == true);//kijk of er al een camping eigenaar is
                if (index >= 0)//als campingowner nog niet bestaat
                {
                    var client = new Client(_listener.AcceptTcpClient(), false);
                    _users.Add(client);
                }
                else
                {
                    var superClient = new Client(_listener.AcceptTcpClient(), true);
                    Program.CampingOwner = superClient;
                    _users.Add(superClient);
                }

                //share message to other client
                ShareConnectedusers();
            }
        }

        //deze vervalt met project
        static void ShareConnectedusers()
        {
            //laat voor elke user zien welke andere users er zijn
            foreach (var user in _users)
            {
                if (user != CampingOwner)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteTypeOfMessage(1);//Verschil tussen packages
                    broadcastPacket.WriteMessage(user.Username);
                    broadcastPacket.WriteMessage(user.UID.ToString());
                    CampingOwner.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }

        }

        public static void ShowAllMessages(string message)
        {
            foreach (var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteTypeOfMessage(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }

        public static void Disconnect(string uidParam)
        {
            //explain use of firstOrDefault 
            var disconnectedUser = _users.Where(u => u.UID.ToString() == uidParam).FirstOrDefault();

            _users.Remove(disconnectedUser);

            //_users.count >1 => anders kan het naar niemand worden verstuurd
           /* foreach (var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteTypeOfMessage(10);
                msgPacket.WriteMessage(uidParam);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }

            ShowAllMessages($"[{disconnectedUser.Username}] is offline!");*/


        }
    }
}

