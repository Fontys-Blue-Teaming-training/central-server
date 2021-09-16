using CentralServer.Server;
using System;
using System.Threading.Tasks;

namespace CentralServer
{
    static class Program
    {

        // https://github.com/jchristn/WatsonWebsocket
        static async Task Main(string[] args)
        {
            Console.WriteLine("Setting up server...");
            SocketServer server = new SocketServer("localhost", 3002, false);
            await server.StartServer();
            Console.WriteLine("Server started!");

            Console.ReadLine();
        }
    }
}
