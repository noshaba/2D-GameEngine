using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace ConsoleApplication1
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();

        static void Main(string[] args)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            TcpListener serverSocket = new TcpListener(IPAddress.Any, 1200);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine("Chat Server Started ....");
            counter = 0;
            while ((true))
            {
                counter++;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine("Client " + counter + " connected.");

                NetworkStream networkStream = clientSocket.GetStream();

                byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
                int data = networkStream.Read(bytesFrom,0,clientSocket.ReceiveBufferSize);
                string dataFromClient = Encoding.ASCII.GetString(bytesFrom,0,data);

                Console.WriteLine("Message recieved: " + dataFromClient);

                clientsList.Add(dataFromClient, clientSocket);

                broadcast(dataFromClient + " Joined ", dataFromClient, false);

                Console.WriteLine(dataFromClient + " Joined chat room ");
                handleClinet client = new handleClinet();
                client.startClient(clientSocket, dataFromClient, clientsList);
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine("exit");
            Console.ReadLine();
        }

        public static void broadcast(string msg, string uName, bool flag)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;

                if (flag == true)
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(uName + " says : " + msg);
                }
                else
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(msg);
                }

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }  //end broadcast function
    }//end Main class


    public class handleClinet
    {
        TcpClient clientSocket;
        string clNo;
        Hashtable clientsList;

        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsList = cList;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine("From client - " + clNo + " : " + dataFromClient);
                    rCount = Convert.ToString(requestCount);

                    Program.broadcast(dataFromClient, clNo, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }//end while
        }//end doChat
    } //end class handleClinet
}//end namespace

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net;
//using System.Net.Sockets;

//namespace SimpleServer
//{
//    class Program
//    {
//        // Incoming data from the client.
//        public static string data = null;

//        public static void StartListening()
//        {
//            // Data buffer for incoming data.
//            byte[] bytes = new Byte[1024];

//            // Establish the local endpoint for the socket.
//            // Dns.GetHostName returns the name of the 
//            // host running the application.
//            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
//            IPAddress ipAddress = ipHostInfo.AddressList[0];
//            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

//            // Create a TCP/IP socket.
//            Socket listener = new Socket(ipAddress.AddressFamily,
//                SocketType.Stream, ProtocolType.Tcp);

//            // Bind the socket to the local endpoint and 
//            // listen for incoming connections.
//            try
//            {
//                listener.Bind(localEndPoint);
//                listener.Listen(10);

//                // Start listening for connections.
//                while (true)
//                {
//                    Console.WriteLine("Waiting for a connection...");
//                    // Program is suspended while waiting for an incoming connection.
//                    Socket handler = listener.Accept();
//                    data = null;

//                    // An incoming connection needs to be processed.
//                    while (true)
//                    {
//                        bytes = new byte[1024];
//                        int bytesRec = handler.Receive(bytes);
//                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
//                        if (data.IndexOf("<EOF>") > -1)
//                        {
//                            break;
//                        }
//                    }

//                    // Show the data on the console.
//                    Console.WriteLine("Text received : " + data);

//                    // Echo the data back to the client.
//                    byte[] msg = Encoding.ASCII.GetBytes(data);

//                    handler.Send(msg);
//                    handler.Shutdown(SocketShutdown.Both);
//                    handler.Close();
//                }

//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }

//            Console.WriteLine("\nPress ENTER to continue...");
//            Console.Read();

//        }

//        public static int Main(String[] args)
//        {
//            StartListening();
//            return 0;
//        }
//    }
//}
