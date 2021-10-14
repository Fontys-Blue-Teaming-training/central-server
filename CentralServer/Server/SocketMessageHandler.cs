﻿using CentralServer.Hosts;
using CentralServer.Messages;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace CentralServer.Server
{
    public static class SocketMessageHandler
    {
        public static async Task HandleStartAction(ScenarioMessage scenarioMessage, WatsonWsServer server)
        {
            Console.WriteLine($"[ACTION] \t {scenarioMessage.Action} {scenarioMessage.Scenario}");
            await server.SendAsync(HostIps.hosts.First(x => x.HostEnum == HostNames.LINUX_HACKER).Ip, JsonConvert.SerializeObject(scenarioMessage));
        }

        public static async Task HandleInfo(InfoMessage message, WatsonWsServer server)
        {
            Console.WriteLine($"[{message.Type}] {(message.Type == InfoMessageType.INFO || message.Type == InfoMessageType.DEBUG ? "\t" : "")} \t {message.Host.HostEnum} -> {message.Message}");

            var uiMessage = new WebUIInfoMessage(message.Host, message.Message, message.Type);
            if(server.ListClients().Any(x => x.Contains(HostIps.hosts.First(x => x.HostEnum == HostNames.TEACHER_INTERFACE).Ip)))
            {
                await server.SendAsync(HostIps.hosts.First(x => x.HostEnum == HostNames.TEACHER_INTERFACE).Ip, JsonConvert.SerializeObject(uiMessage));
            }
            else
            {
                // Add Queue later..
                Console.WriteLine($"[ERROR] \t Teacher interface is not connected. Dropping packet...");
            }
        }
    }
}
