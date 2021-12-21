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
        public static int port = 3360;//Port used for TCP data transfer
        static List<Client> _users;
        private static Client CampingOwner { get; set; }

        static void Main(string[] args)
        {
            _users = new List<Client>();
           // _listener = new TcpListener(IPAddress.Parse(IpAdress), port);
            _listener = new TcpListener(IPAddress.Parse(IpAdress), port);
            _listener.Start();

            //Us always listening for clients
            while (true)
            {
                var superClient = new Client(_listener.AcceptTcpClient());               
                _users.Add(superClient);
            }
        }

        

        public static void ShowAllMessages(string message)
        {
            foreach (var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteTypeOfMessage(6);
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
                msgPacket.WriteTypeOfMessage(9);
                msgPacket.WriteMessage(uidParam);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }

            ShowAllMessages($"[{disconnectedUser.Username}] is offline!");*/


        }
    }
}

