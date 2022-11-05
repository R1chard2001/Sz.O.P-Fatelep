using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace server
{
    internal class Client
    {
        public static List<Client> clients = new List<Client>();
        public static void CloseAllClient()
        {
            foreach (Client c in new List<Client>(Client.clients))
            {
                c.Close();
            }
        }
        private User currentUser = null;
        public Client(TcpClient TcpClient)
        {
            this.tcpClient = TcpClient;
            reader = new StreamReader(TcpClient.GetStream());
            writer = new StreamWriter(TcpClient.GetStream());
            clients.Add(this);
            ClientThread = new Thread(ReadCommands);
            ClientThread.Start();
            Console.WriteLine("New client started!");
        }
        private TcpClient tcpClient;
        private StreamReader reader;
        private StreamWriter writer;
        public Thread ClientThread;
        private void ReadCommands()
        {
            try
            {
                while (true)
                {
                    string command = reader.ReadLine();
                    if (command == null) break;
                    Interpret(command);
                    Console.WriteLine("Command recived: {0}", command);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Close();
                Console.WriteLine("Client disconnected");
            }
        }
        private void Interpret(string cmd)
        {
            string[] data = cmd.Split('|');
            switch (data[0])
            {
                case "EXIT":
                    Close();
                    break;
                case "LOGIN":
                    Login(data);
                    break;
                case "LOGOUT":
                    Logout();
                    break;
                case "LIST":
                    ListTimber(data);
                    break;
                case "PRICE":
                    GetPrice(data);
                    break;
                case "ADD":
                    AddTimber(data);
                    break;
                case "USERS":
                    ListUsers();
                    break;
                case "ONLINEUSERS":
                    ListOnlineUsers();
                    break;
                case "ADDUSER":
                    AddUser(data);
                    break;
                case "DELUSER":
                    DeleteUser(data);
                    break;
                default:
                    SendInformation("Unknown command!");
                    break;
            }
        }
        private void ListOnlineUsers()
        {
            if (currentUser == null || currentUser.IsAdmin < 1)
            {
                SendInformation("Only admins can use this feature!");
                return;
            }
            List<string> onlineU = new List<string>();
            foreach (Client c in Client.clients)
            {
                if (c.currentUser != null &&
                    !onlineU.Contains(c.currentUser.Name))
                {
                    onlineU.Add(c.currentUser.Name);
                }
            }
            foreach (string name in onlineU)
            {
                SendInformation(name);
            }
        }
        private void DeleteUser(string[] data)
        {
            if (currentUser == null || currentUser.IsAdmin < 1)
            {
                SendInformation("Only admins can use this feature!");
                return;
            }
            if (data.Length != 2)
            {
                SendInformation("Incorrect parameter list!");
                return;
            }
            User del = User.UserList.Find(u => u.Name == data[1]);
            if (del == null)
            {
                SendInformation("User not found!");
            }
            else
            {
                User.UserList.Remove(del);
                SendInformation("User deleted!");
            }
        }
        private void AddUser(string[] data)
        {
            if (currentUser == null || currentUser.IsAdmin < 1)
            {
                SendInformation("Only admins can use this feature!");
                return;
            }
            if (data.Length != 3)
            {
                SendInformation("Incorrect parameter list!");
                return;
            }
            User.UserList.Add(new User(data[1], data[2], 0));
            SendInformation("New user added!");
        }
        private void ListUsers()
        {
            if (currentUser == null || currentUser.IsAdmin < 1)
            {
                SendInformation("Only admins can use this feature!");
                return;
            }
            foreach (User u in User.UserList)
            {
                SendInformation(u.Name);
            }
        }
        private void AddTimber(string[] data)
        {
            if (currentUser == null)
            {
                SendInformation("Log in to use this feature!");
                return;
            }
            if (data.Length != 4)
            {
                SendInformation("Incorrect parameter list!");
                return;
            }
            int width;
            int price;
            if (!int.TryParse(data[2], out width))
            {
                SendInformation("Width must be integer!");
                return;
            }
            if (!int.TryParse(data[3], out price))
            {
                SendInformation("Price must be integer!");
                return;
            }
            Timber t = Timber.TimberList.Find(timber =>
               timber.Wood == data[1] && timber.Width == width);
            if (t == null)
            {
                Timber.TimberList.Add(new Timber(
                    data[1],
                    width,
                    price));
                SendInformation("New timber added!");
            }
            else
            {
                t.Price = price;
                SendInformation("Timber updated!");
            }
        }
        private void GetPrice(string[] data)
        {
            if (data.Length != 4)
            {
                SendInformation("Incorrect parameter list!");
                return;
            }
            int width;
            int fm;
            if (!int.TryParse(data[2], out width))
            {
                SendInformation("Width must be integer!");
                return;
            }
            if (!int.TryParse(data[3], out fm))
            {
                SendInformation("Amount must be integer!");
                return;
            }
            Timber t = Timber.TimberList.Find(
                timber => timber.Wood == data[1] && timber.Width == width
                );
            if (t == null)
            {
                SendInformation("Missing timber!");
            }
            else
            {
                SendInformation(string.Format("Price: {0}", t.Price * fm));
            }
        }
        private void ListTimber(string[] data)
        {
            if (data.Length != 2)
            {
                SendInformation("Incorrect parameter list!");
                return;
            }
            int width;
            if (!int.TryParse(data[1], out width))
            {
                SendInformation("Parameter must be integer!");
                return;
            }
            foreach (Timber t in Timber.TimberList)
            {
                if (t.Width == width)
                {
                    SendInformation(
                     string.Format("Timber:\n - wood: {0}\n - width: {1}\n - price: {2}",
                     t.Wood, t.Width, t.Price));
                }
            }
        }
        private void Login(string[] data)
        {
            if (data.Length != 3)
            {
                SendInformation("Incorrect parameter list!");
                return;
            }
            if (currentUser != null)
            {
                SendInformation("You are already logged in!");
                return;
            }
            User u = User.LoginTry(data[1], data[2]);
            if (u == null)
            {
                SendInformation("Incorrect username or password!");
            }
            else
            {
                SendInformation(String.Format("User {0} logged in.", u.Name));
                currentUser = u;
            }
        }
        private void Logout()
        {
            if (currentUser == null)
            {
                SendInformation("You are not logged in!");
            }
            else
            {
                currentUser = null;
                SendInformation("Logging out");
            }
        }
        
        private void SendInformation(string info)
        {
            writer.WriteLine(info);
            
            writer.Flush();
        }
        public void Close()
        {
            reader.Close();
            writer.Close();
            tcpClient.Close();
            clients.Remove(this);
        }
    }
}
