using CentralServer.Hosts;
using CentralServer.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace CentralServer.Server
{
    public static class SocketMessageHandler
    {
        private static Dictionary<HostNames, string> teamHosts = new Dictionary<HostNames, string>();
        // private static readonly List<QueueMessage> queueMessages = new List<QueueMessage>();

        public static async Task HandleStartAction(ScenarioMessage scenarioMessage, WatsonWsServer server)
        {
            string client = string.Empty;
            switch (scenarioMessage.Scenario)
            {
                case Scenarios.LINUX_SSH_ATTACK:
                    client = server.ListClients().FirstOrDefault(x => x.Contains(HostIps.hosts.First(x => x.HostEnum == HostNames.LINUX_HACKER).Ip));
                    break;
                case Scenarios.MALWARE:
                    client = server.ListClients().FirstOrDefault(x => x.Contains(HostIps.hosts.First(x => x.HostEnum == HostNames.LINUX_SSH_TARGET).Ip));
                    break;
                default:
                    break;
            }

            if(!string.IsNullOrWhiteSpace(client) || scenarioMessage.Scenario == Scenarios.FORENSICS)
            {
                Console.WriteLine($"[ACTION] \t {scenarioMessage.Action} {scenarioMessage.Scenario}");
                await server.SendAsync(client, JsonConvert.SerializeObject(scenarioMessage));
                if(teamHosts.Any())
                {
                    foreach (var teamHost in teamHosts)
                    {
                        Console.WriteLine($"[INFO] \t Sending {scenarioMessage.Action} {scenarioMessage.Scenario} to {teamHost.Key} ({teamHost.Value})");
                        var host = server.ListClients().FirstOrDefault(x => x.Contains(HostIps.hosts.First(x => x.HostEnum == teamHost.Key).Ip));
                        await server.SendAsync(host, JsonConvert.SerializeObject(new InfoMessage(InfoMessageType.INFO, scenarioMessage.Action == ScenarioActions.START ? "SCENARIO STARTED" : "SCENARIO STOPPED")));
                    }

                    if(scenarioMessage.Action == ScenarioActions.STOP)
                    {
                        Console.WriteLine($"[INFO] \t Received STOP message. Resetting teams...");
                        teamHosts = new Dictionary<HostNames, string>();
                    }
                }
                else
                {
                    Console.WriteLine($"[WARNING] \t No active teams found");
                }
            }
            else
            {
                Console.WriteLine($"[ERROR] \t Scenario has no host {scenarioMessage.Action} {scenarioMessage.Scenario}! Dropping packet...");
            }
        }

        public static async Task HandleInfo(InfoMessage message, WatsonWsServer server)
        {
            Console.WriteLine($"[{message.Type}] {(message.Type == InfoMessageType.INFO || message.Type == InfoMessageType.DEBUG ? "\t" : "")} \t {message.Host.HostEnum} -> {message.Message}");

            if (message.Type == InfoMessageType.FLAG_COMPLETED)
            {
                await HandleFlagCompleted(message, server);
                return;
            }

            if(message.Type == InfoMessageType.TEAM_CREATED)
            {
                HandleTeamCreated(message);
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

        private static async Task HandleFlagCompleted(InfoMessage message, WatsonWsServer server)
        {
            foreach (var studentMachine in teamHosts)
            {
                var studentClient = server.ListClients().FirstOrDefault(x => x.Contains(HostIps.hosts.First(x => x.HostEnum == studentMachine.Key).Ip));
                if (studentClient != default)
                {
                    Console.WriteLine($"[INFO] \t Sending flag completed to host {studentMachine.Key} with teamname {studentMachine.Value}");
                    await server.SendAsync(studentClient, JsonConvert.SerializeObject(message));
                }
                else
                {
                    Console.WriteLine($"[ERROR] \t Could not find client {studentMachine.Key} ({studentMachine.Value}). Dropping packet...");
                }
            }
        }

        private static void HandleTeamCreated(InfoMessage message)
        {
            if(!teamHosts.Any(x => x.Key == message.Host.HostEnum))
            {
                Console.WriteLine($"[INFO] \t Team {message.Message} has been created on host {message.Host}");
                teamHosts.Add(message.Host.HostEnum, message.Message);
            }
            else
            {
                Console.WriteLine($"[WARNING] \t Overwriting team {teamHosts[message.Host.HostEnum]} with team {message.Message} on host {message.Host}");
                teamHosts[message.Host.HostEnum] = message.Message;
            }
        }
    }
}
