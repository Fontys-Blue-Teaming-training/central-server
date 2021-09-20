using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace CentralServer.Server
{
    public static class SocketMessageHandler
    {
        public static async Task HandleStartAction(SocketAction action, WatsonWsServer server)
        {
            // Do some sending
            Console.WriteLine($"[ACTION] \t {action}");
        }

        public static async Task HandleInfo(SocketInfoMessage message, WatsonWsServer server)
        {
            Console.WriteLine($"[{message.Type}] \t {message.Host.HostName} -> {message.Message}");

            var uiMessage = new WebUIInfoMessage(message.Host, message.Message, message.Type);
            await server.SendAsync(HostIps.hosts.First(x => x.HostName == Hosts.TEACHER_INTERFACE).Ip, JsonConvert.SerializeObject(uiMessage));

        }
    }
}
