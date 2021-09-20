using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace CentralServer.Server
{
    public static class SocketServer
    {
        private readonly static WatsonWsServer server = new WatsonWsServer("145.93.164.36", 3002, false);

        public static void SetupServer()
        {
            server.PermittedIpAddresses = HostIps.hosts.Select(x => x.Ip).ToList();
            server.ClientConnected += ClientConnected;
            server.ClientDisconnected += ClientDisconnected;
            server.MessageReceived += MessageReceived;
        }

        public static async Task StartServer()
        {
            await server.StartAsync();
        }

        public static async Task SendMessage(string ip, string message)
        {
            await server.SendAsync(server.ListClients().First(x => x.Contains(ip)), message);
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
                Task.Run(async () => await SocketMessageHandler.HandleMessage(message, server));
            }
            catch (Exception)
            {
                Console.WriteLine($"[ERROR] \t Received unknown packet from {HostIps.GetHostByIp(args.IpPort)} ({args.IpPort}): {Encoding.UTF8.GetString(args.Data)}");
            }
        }
    }
}
