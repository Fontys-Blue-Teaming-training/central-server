using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace CentralServer.Server
{
    public class SocketServer
    {
        private readonly WatsonWsServer server;
        private readonly Host[] hosts;

        public SocketServer(string ip, int port, bool ssl, Host[] hosts)
        {
            server = new WatsonWsServer(ip, port, ssl);
            this.hosts = hosts;
            SetupServer();
        }

        private void SetupServer()
        {
            server.PermittedIpAddresses = hosts.Select(x => x.Ip).ToList();
            server.ClientConnected += ClientConnected;
            server.ClientDisconnected += ClientDisconnected;
            server.MessageReceived += MessageReceived;
        }

        public async Task StartServer()
        {
            await server.StartAsync();
        }

        public async Task SendMessage(string ip)
        {
            await server.SendAsync(server.ListClients().First(x => x.Contains(ip)), "hey!");
        }

        static void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine($"[CONNECTED] \t {HostIps.GetHostByIp(args.IpPort)} ({args.IpPort})");
        }

        private static void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            Console.WriteLine($"[DISCONNECTED] \t {HostIps.GetHostByIp(args.IpPort)} ({args.IpPort})");
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
                Console.WriteLine($"[ERROR] \t Received unknown packet from {HostIps.GetHostByIp(args.IpPort)} ({args.IpPort}): {Encoding.UTF8.GetString(args.Data)}");
            }
        }
    }
}
