using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace CentralServer.Server
{
    public class SocketServer
    {
        private readonly WatsonWsServer server;
        
        public SocketServer(string ip, int port, bool ssl)
        {
            server = new WatsonWsServer(ip, port, ssl);
            SetupServer();
        }

        private void SetupServer()
        {
            // server.PermittedIpAddresses = new List<string>();
            server.ClientConnected += ClientConnected;
            server.ClientDisconnected += ClientDisconnected;
            server.MessageReceived += MessageReceived;
        }

        public async Task StartServer()
        {
            await server.StartAsync();
        }

        static void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine($"[CONNECTED] \t {args.IpPort}");
        }

        private static void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            Console.WriteLine($"[DISCONNECTED] \t {args.IpPort}");
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            try
            {
                var message = JsonConvert.DeserializeObject<SocketMessage>(Encoding.UTF8.GetString(args.Data));
                message.IpPort = args.IpPort;
                SocketMessageHandler.HandleMessage(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] \t Received unknown packet from IpPort [{args.IpPort}]: {Encoding.UTF8.GetString(args.Data)}");
            }
        }
    }
}
