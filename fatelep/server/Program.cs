using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

namespace server
{
    internal class Program
    {
        
        private static bool listen = true;
        private static TcpListener listener;
        static void Main(string[] args)
        {
            string userFile = "Users.xml";
            string resourceFile = "timber.xml";

            User.LoadUsers(userFile);
            Console.WriteLine("Users loaded successfully!");

            Timber.LoadResources(resourceFile);
            Console.WriteLine("Resources loaded successfully!");

            IPAddress ip = IPAddress.Parse(ConfigurationManager.AppSettings["ip"].ToString());
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            Console.WriteLine("Configuration loaded!\n - IP: {0}\n - Port: {1}", ip, port);

            listener = new TcpListener(ip, port);
            listener.Start();
            Thread listenerThread = new Thread(WaitingForClients);
            listenerThread.Start();
            Console.WriteLine("Listener started!");

            Console.WriteLine("Press ENTER to close the program!");
            Console.ReadLine();

            listen = false;
            listener.Stop();
            Client.CloseAllClient();
            User.SaveUsers(userFile);
            Timber.SaveResources(resourceFile);
        }
        static private void WaitingForClients()
        {
            while (listen)
            {
                if (listener.Pending())
                {
                    TcpClient client = listener.AcceptTcpClient();
                    new Client(client);
                }
            }
        }
    }
}
