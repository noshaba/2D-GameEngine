using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerData;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client
{
    class Client
    {
        public static Socket clientSocket;
        public static string name;
        public static string id;

        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            name = Console.ReadLine();

            A: Console.Clear();
            Console.Write("Enter host IP address: ");
            string ipAddr = Console.ReadLine();
            // TODO: try IPAddress.Parse(ip);
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(ipAddr), 1337);

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                clientSocket.Connect(ip);
            }
            catch
            {
                Console.WriteLine("Could not connect to host.");
                Thread.Sleep(1000);
                goto A; //TODO remove goto
            }

            Thread clientThread = new Thread(DataIN);
            clientThread.Start();

            for (; ; )
            {
                Console.Write("::>");
                string input = Console.ReadLine();
                // 192.168.178.26
                Packet p = new Packet(PacketType.Chat, id);
                p.generalData.Add(name);
                p.generalData.Add(input);
                clientSocket.Send(p.ToBytes());
            }
        }

        static void DataIN()
        {
            byte[] buffer;
            int readBytes;

            for(; ; )
            {
                try
                {
                    buffer = new byte[clientSocket.SendBufferSize];
                    readBytes = clientSocket.Receive(buffer);

                    if (readBytes > 0)
                        DataManager(new Packet(buffer));
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("The server hs disconnected.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
        }

        static void DataManager(Packet p)
        {
            switch (p.packetType)
            {
                case PacketType.Registration:
                    id = p.generalData[0];
                    break;
                case PacketType.Chat:
                    ConsoleColor c = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(p.generalData[0] + ": " + p.generalData[1]);
                    Console.ForegroundColor = c;
                    break;
            }
        }
    }
}
