using CentralServer.Hosts;
using CentralServer.Messages;
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
        private const bool SHOULD_LOG = true;
        private readonly static List<string> hostnames = new List<string>
        {
            "192.168.1.2",
            "172.16.1.4"
        };

        private readonly static WatsonWsServer wsServer = new WatsonWsServer(hostnames, 3002, false);

        public static void SetupServer()
        {
            wsServer.PermittedIpAddresses = HostIps.hosts.Select(x => x.Ip).ToList();
            wsServer.ClientConnected += ClientConnected;
            wsServer.ClientDisconnected += ClientDisconnected;
            wsServer.MessageReceived += MessageReceived;

            if (SHOULD_LOG)
            {
                wsServer.Logger = Logger;
            }
        }

        private static void Logger(string message)
        {
            Console.WriteLine($"[DEBUG] \t {message}");
        }

        public static async Task StartServer()
        {
            await wsServer.StartAsync();
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
                if(HostIps.GetHostByIp(args.IpPort) == HostNames.TEACHER_INTERFACE)
                {
                    // This should always be an action. The teacher will never send info
                    var action = JsonConvert.DeserializeObject<ScenarioMessage>(Encoding.UTF8.GetString(args.Data));
                    Task.Run(async () => await SocketMessageHandler.HandleStartAction(action, wsServer));
                }
                else if(HostIps.hosts.Any(x => args.IpPort.Contains(x.Ip)))
                {
                    // This should always be info. 
                    var message = JsonConvert.DeserializeObject<InfoMessage>(Encoding.UTF8.GetString(args.Data));
                    message.Host = HostIps.hosts.First(x => args.IpPort.Contains(x.Ip));
                    Task.Run(async () => await SocketMessageHandler.HandleInfo(message, wsServer));
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"[ERROR] \t Received unknown packet from {HostIps.GetHostByIp(args.IpPort)} ({args.IpPort}): {Encoding.UTF8.GetString(args.Data)}");
            }
        }
    }
}
