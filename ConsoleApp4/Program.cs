using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            fileCreation();
            connect();
            output();
            Console.ReadKey();
            
        }

        static void output()
        {
            Console.WriteLine("Here is some info about your system:\n");
            Console.WriteLine("OS: {0}", Environment.OSVersion);
            Console.WriteLine("Number of precessors: {0}", Environment.ProcessorCount);
            Console.WriteLine("Your system directory: {0}", Environment.SystemDirectory);
            Console.WriteLine("Your user domain name: {0}", Environment.UserDomainName);
            Console.WriteLine("User name: {0}", Environment.UserName);

            #region Check 64-bit
            if (Environment.Is64BitOperatingSystem)
            {
                Console.WriteLine("This system is 64-bit operating system");
            }
            else
            {
                Console.WriteLine("This system is 32-bit operating system");
            }
            #endregion

            Console.Write("\nPlease press any key...");
        }

        static void fileCreation()
        {
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\Test.txt"))
            {  
                outputFile.WriteLine(Environment.OSVersion);
                outputFile.WriteLine(Environment.UserDomainName);
                outputFile.WriteLine(Environment.UserName);
                outputFile.WriteLine(Environment.SpecialFolderOption.Create);
                outputFile.WriteLine(Environment.ProcessorCount);
            }

        }

        static void connect()
        {
            // локальный эндпоинт для сокета
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // создаем TCP сокет
            Socket client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // коннект
            client.Connect(ipEndPoint);

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = mydocpath + @"\Test.txt";

            Console.WriteLine("Sending {0} to the host.", fileName);
            client.SendFile(fileName);

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}
