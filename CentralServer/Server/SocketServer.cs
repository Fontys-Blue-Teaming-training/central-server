﻿using CentralServer.Hosts;
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
        private readonly static WatsonWsServer server = new WatsonWsServer("192.168.1.2", 3002, false);



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
            JsonConvert.SerializeObject(new WebUIInfoMessage(HostIps.hosts.First(), "DDOS attack detected", InfoMessageType.INFO));
            await server.SendAsync(server.ListClients().First(x => x.Contains(ip)), JsonConvert.SerializeObject(new WebUIInfoMessage(HostIps.hosts.First(), "DDOS attack detected", InfoMessageType.INFO)));
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
                    Task.Run(async () => await SocketMessageHandler.HandleStartAction(action, server));
                }
                else if(HostIps.hosts.Any(x => args.IpPort.Contains(x.Ip)))
                {
                    // This should always be info. 
                    var message = JsonConvert.DeserializeObject<InfoMessage>(Encoding.UTF8.GetString(args.Data));
                    message.Host = HostIps.hosts.First(x => args.IpPort.Contains(x.Ip));
                    Task.Run(async () => await SocketMessageHandler.HandleInfo(message, server));
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"[ERROR] \t Received unknown packet from {HostIps.GetHostByIp(args.IpPort)} ({args.IpPort}): {Encoding.UTF8.GetString(args.Data)}");
            }
        }
    }
}
