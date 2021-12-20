using System;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            static TcpListener _listener;
        public static string IpAdress = "127.0.0.1";
        public static int port = 3360;
        static List<ClientServerSide> _users;
        private static ClientServerSide CampingOwner { get; set; }

        static void Main(string[] args)
        {
            _users = new List<ClientServerSide>();
            _listener = new TcpListener(IPAddress.Parse(IpAdress), port);
            _listener.Start();

            //campingowner toevoegen?

            //_users.Add(new Client(_listener.AcceptTcpClient(), true));
            //altijd luisteren naar nieuw clients
            while (true)
            {
                int index = _users.FindIndex(user => user.IsSuperClient == true);//kijk of er al een camping eigenaar is
                if (index >= 0)//als campingowner nog niet bestaat
                {
                    var client = new ClientServerSide(_listener.AcceptTcpClient(), false);
                    _users.Add(client);
                }
                else
                {
                    var superClient = new ClientServerSide(_listener.AcceptTcpClient(), true);
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
                    var broadcastPacket = new Packetbuilder();
                    broadcastPacket.WriteTypeOfMessage(1);//wat ga je doen? verschil tussen packages
                    broadcastPacket.WriteMessage(user.Username);
                    broadcastPacket.WriteMessage(user.UID.ToString());
                    CampingOwner.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }

            /*  foreach (var user in _users)    
              {
                  foreach (var usr in _users) 
                  {
                      if(usr != user)
                      {
                          var broadcastPacket = new Packetbuilder();
                          broadcastPacket.WriteTypeOfMessage(1);//wat ga je doen? verschil tussen packages
                          broadcastPacket.WriteMessage(usr.Username);
                          broadcastPacket.WriteMessage(usr.UID.ToString());
                          user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                      }

                  }
              }*/
        }

        public static void ShowAllMessages(string message)
        {
            /* var msgPacket = new Packetbuilder();
             msgPacket.WriteTypeOfMessage(5);
             msgPacket.WriteMessage(message);
             CampingOwner.ClientSocket.Client.Send(msgPacket.GetPacketBytes());*/


            foreach (var user in _users)
            {
                var msgPacket = new Packetbuilder();
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


            foreach (var user in _users)
            {
                var msgPacket = new Packetbuilder();
                msgPacket.WriteTypeOfMessage(10);
                msgPacket.WriteMessage(uidParam);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }

            ShowAllMessages($"[{disconnectedUser.Username}] is offline!");


        }
    }
}
}
