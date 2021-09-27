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
            // Do some sending
            Console.WriteLine($"[ACTION] \t {scenarioMessage.Action} {scenarioMessage.Scenario}");
        }

        public static async Task HandleInfo(InfoMessage message, WatsonWsServer server)
        {
            Console.WriteLine($"[{message.Type}] \t {message.Host.HostEnum} -> {message.Message}");

            var uiMessage = new WebUIInfoMessage(message.Host, message.Message, message.Type);
            await server.SendAsync(HostIps.hosts.First(x => x.HostEnum == HostNames.TEACHER_INTERFACE).Ip, JsonConvert.SerializeObject(uiMessage));

        }
    }
}
