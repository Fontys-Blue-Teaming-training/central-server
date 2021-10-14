using CentralServer.Hosts;
using CentralServer.Messages;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace CentralServer.Server
{
    public static class SocketServer
    {
        private readonly static WatsonWsServer vlanBServer = new WatsonWsServer("192.168.1.2", 3002, false);
        private readonly static WatsonWsServer vlanAServer = new WatsonWsServer("172.16.1.4", 3002, false);

        public static void SetupServer()
        {
            vlanBServer.PermittedIpAddresses = HostIps.hosts.Select(x => x.Ip).ToList();
            vlanBServer.ClientConnected += ClientConnected;
            vlanBServer.ClientDisconnected += ClientDisconnected;
            vlanBServer.MessageReceived += MessageReceived;
            vlanAServer.PermittedIpAddresses = HostIps.hosts.Select(x => x.Ip).ToList();
            vlanAServer.ClientConnected += ClientConnected;
            vlanAServer.ClientDisconnected += ClientDisconnected;
            vlanAServer.MessageReceived += MessageReceived;


            // Enable if Watson logging is needed
            // vlanBServer.Logger = Logger;
            // vlanAServer.Logger = Logger;
        }

        //static void Logger(string message)
        //{
        //    Console.WriteLine(message);
        //}

        public static async Task StartServer()
        {
            await vlanBServer.StartAsync();
            await vlanAServer.StartAsync();
        }

        public static async Task SendMessage(string ip)
        {
            await vlanAServer.SendAsync(vlanAServer.ListClients().First(x => x.Contains(ip)), JsonConvert.SerializeObject(new ScenarioMessage() { Action = ScenarioActions.START, Scenario = Scenarios.LINUX_SSH_ATTACK }));
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
                    Task.Run(async () => await SocketMessageHandler.HandleStartAction(action, vlanAServer));
                }
                else if(HostIps.hosts.Any(x => args.IpPort.Contains(x.Ip)))
                {
                    // This should always be info. 
                    var message = JsonConvert.DeserializeObject<InfoMessage>(Encoding.UTF8.GetString(args.Data));
                    message.Host = HostIps.hosts.First(x => args.IpPort.Contains(x.Ip));
                    Task.Run(async () => await SocketMessageHandler.HandleInfo(message, vlanBServer));
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"[ERROR] \t Received unknown packet from {HostIps.GetHostByIp(args.IpPort)} ({args.IpPort}): {Encoding.UTF8.GetString(args.Data)}");
            }
        }
    }
}
