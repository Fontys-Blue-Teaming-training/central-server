using CentralServer.Hosts;
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
            string client = string.Empty;
            switch (scenarioMessage.Scenario)
            {
                case Scenarios.LINUX_SSH_ATTACK:
                    client = server.ListClients().FirstOrDefault(x => x.Contains(HostIps.hosts.First(x => x.HostEnum == HostNames.LINUX_HACKER).Ip));
                    break;
                default:
                    break;
            }

            if(!string.IsNullOrWhiteSpace(client))
            {
                Console.WriteLine($"[ACTION] \t {scenarioMessage.Action} {scenarioMessage.Scenario}");
                await server.SendAsync(client, JsonConvert.SerializeObject(scenarioMessage));
            }
            else
            {
                Console.WriteLine($"[ERROR] \t Scenario has no host {scenarioMessage.Action} {scenarioMessage.Scenario}! Dropping packet...");
            }
        }

        public static async Task HandleInfo(InfoMessage message, WatsonWsServer server)
        {
            Console.WriteLine($"[{message.Type}] {(message.Type == InfoMessageType.INFO || message.Type == InfoMessageType.DEBUG ? "\t" : "")} \t {message.Host.HostEnum} -> {message.Message}");

            if (message.Message == "FLAG COMPLETED" || message.Message == "SCENARIO STARTED")
            {
                var studentMachines = HostIps.hosts.Where(x => ((int)x.HostEnum) > 2).ToList();
                foreach (var studentMachine in studentMachines)
                {
                    var studentClient = server.ListClients().FirstOrDefault(x => x.Contains(studentMachine.Ip));
                    if(studentClient != default)
                    {
                        await server.SendAsync(studentClient, JsonConvert.SerializeObject(message));
                    }
                    else
                    {
                        Console.WriteLine($"[ERROR] \t Coud not find client {studentMachine.HostName} ({studentMachine.Ip}). Dropping packet...");
                    }
                }
                return;
            }

            var uiMessage = new WebUIInfoMessage(message.Host, message.Message, message.Type);
            var client = server.ListClients().FirstOrDefault(x => x.Contains(HostIps.hosts.First(x => x.HostEnum == HostNames.TEACHER_INTERFACE).Ip));
            if (client != default)
            {
                await server.SendAsync(client, JsonConvert.SerializeObject(uiMessage));
            }
            else
            {
                // Add Queue later..
                Console.WriteLine($"[ERROR] \t Teacher interface is not connected. Dropping packet...");
            }
        }
    }
}
