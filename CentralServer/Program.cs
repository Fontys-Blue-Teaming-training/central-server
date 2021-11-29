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
            SocketServer.SetupServer();
            await SocketServer.StartServer();
            Console.WriteLine("Server started!");
            Console.ReadLine();
        }
    }
}
