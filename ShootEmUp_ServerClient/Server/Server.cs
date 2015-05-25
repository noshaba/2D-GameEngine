using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerData;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace Server
{
    class Server
    {
        static Socket listenerSocket;
        static List<ClientData> clients;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting server on " + Packet.GetIP4Address());

            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new List<ClientData>();

            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Packet.GetIP4Address()), 1337);
            listenerSocket.Bind(ip);

            Thread listenThread = new Thread(ListenThread);

            listenThread.Start();
        }

        // listener - listens for clients trying to connect
        static void ListenThread()
        {
            for (; ; )
            {
                listenerSocket.Listen(0);
                clients.Add(new ClientData(listenerSocket.Accept()));
            }

        }

        // clientdata thread - receives data from each client individually
        public static void DataIN(object cSocket)
        {
            Socket clientSocket = (Socket)cSocket;

            byte[] buffer;
            int readBytes;

            for (; ; )
            {
                buffer = new byte[clientSocket.SendBufferSize];
                readBytes = clientSocket.Receive(buffer);

                if (readBytes > 0) 
                    DataManager(new Packet(buffer));
            }
        }

        // data manager
        public static void DataManager(Packet p)
        {
            switch(p.packetType)
            {
                case PacketType.Chat:
                    // broadcast
                    foreach (ClientData c in clients)
                        c.clientSocket.Send(p.ToBytes());
                    break;
            }
        }
    }

    class ClientData
    {
        public Socket clientSocket;
        public Thread clientThread;
        public string id;

        public ClientData()
        {
            id = Guid.NewGuid().ToString();
            clientThread = new Thread(Server.DataIN);
            clientThread.Start(clientSocket);
            SendRegistrationPacket();
        }

        public ClientData(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            id = Guid.NewGuid().ToString();
            clientThread = new Thread(Server.DataIN);
            clientThread.Start(clientSocket);
            SendRegistrationPacket();
        }

        public void SendRegistrationPacket()
        {
            Packet p = new Packet(PacketType.Registration, "server");
            p.generalData.Add(id);
            clientSocket.Send(p.ToBytes());
        }
    }
}
