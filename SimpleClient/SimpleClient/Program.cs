using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client 
{ 
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("127.0.0.1", 1200);
            Console.WriteLine("Trying to connect to server...");
            NetworkStream n = client.GetStream();
            Console.WriteLine("Connected.");
            string ch = Console.ReadLine();
            byte[] msg = Encoding.ASCII.GetBytes(ch);
            n.Write(msg, 0, msg.Length);
            Console.WriteLine("--Sent--");
            client.Close();
            Console.ReadKey();
        }
    }
}

//namespace SimpleClient
//{
//    class Program
//    {
//        public static void StartClient() {
//        // Data buffer for incoming data.
//        byte[] bytes = new byte[1024];

//        // Connect to a remote device.
//        try {
//            // Establish the remote endpoint for the socket.
//            // This example uses port 11000 on the local computer.
//            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
//            IPAddress ipAddress = ipHostInfo.AddressList[0];
//            IPEndPoint remoteEP = new IPEndPoint(ipAddress,11000);

//            // Create a TCP/IP  socket.
//            Socket sender = new Socket(ipAddress.AddressFamily, 
//                SocketType.Stream, ProtocolType.Tcp );

//            string line;

//            // Connect the socket to the remote endpoint. Catch any errors.
//            try {
//                sender.Connect(remoteEP);

//                Console.WriteLine("Socket connected to " +
//                    sender.RemoteEndPoint.ToString());

//                do {
//                    line = Console.ReadLine();

//                    // Encode the data string into a byte array.
//                    byte[] msg = Encoding.ASCII.GetBytes(line + "<EOF>");

//                    // Send the data through the socket.
//                    int bytesSent = sender.Send(msg);

//                    // Receive the response from the remote device.
//                    int bytesRec = sender.Receive(bytes);
//                    Console.WriteLine("Echoed test " +
//                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

//                } while(line != null);

//                // Release the socket.
//                sender.Shutdown(SocketShutdown.Both);
//                sender.Close();
                
//            } catch (ArgumentNullException ane) {
//                Console.WriteLine("ArgumentNullException : " + ane.ToString());
//            } catch (SocketException se) {
//                Console.WriteLine("SocketException : " + se.ToString());
//            } catch (Exception e) {
//                Console.WriteLine("Unexpected exception : " + e.ToString());
//            }

//        } catch (Exception e) {
//            Console.WriteLine( e.ToString());
//        }
//    }

//        public static int Main(String[] args)
//        {
//            StartClient();
//            return 0;
//        }
//    }
//}
