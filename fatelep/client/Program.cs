using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.IO;
using System.Threading;

namespace client
{
    internal class Program
    {
        static StreamReader reader;
        static StreamWriter writer;
        static TcpClient tcpClient;
        static Thread ReaderThread = new Thread(ReadInfo);
        static void Main(string[] args)
        {
            string ip = ConfigurationManager.AppSettings["ip"].ToString();
            int port = int.Parse(ConfigurationManager.AppSettings["port"].ToString());
            try
            {
                tcpClient = new TcpClient(ip, port);
                writer = new StreamWriter(tcpClient.GetStream());
                reader = new StreamReader(tcpClient.GetStream());
                ReaderThread.Start();
                Console.WriteLine("Connected to {0}:{1}", ip, port);
                SendCommands();
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong.");
                Console.ReadLine();
            }
            
        }
        static void SendCommands()
        {
            try
            {
                while (true)
                {
                    WriteCommand(Console.ReadLine());
                }
            }
            catch (Exception)
            { }
            finally
            {
                Close();
            }
        }
        static void WriteCommand(string info)
        {
            writer.WriteLine(info);
            writer.Flush();
        }
        static void ReadInfo()
        {
            try
            {
                while (!reader.EndOfStream)
                {
                    Console.WriteLine(reader.ReadLine());
                }
            }
            catch (Exception)
            { }
            finally 
            {
                Close();
                Console.WriteLine("Disconnected from the server!");
            }
        }
        static void Close()
        {
            writer.Close();
            reader.Close();
            tcpClient.Close();
        }
    }
}
