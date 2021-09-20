using CentralServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralServer
{
    static class Program
    {

        // https://github.com/jchristn/WatsonWebsocket
        static async Task Main(string[] args)
        {
            Console.WriteLine("Setting up server...");
            SocketServer server = new SocketServer("145.93.164.36", 3002, false, HostIps.hosts);
            await server.StartServer();
            Console.WriteLine("Server started!");
            while(true)
            {
                Console.WriteLine("Press enter to send message");
                Console.ReadLine();
                await server.SendMessage(HostIps.hosts.First(x => x.HostName == Hosts.TEACHER_INTERFACE).Ip);
            }


            Console.ReadLine();
        }
    }
}
