using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace CentralServer.Server
{
    public static class SocketMessageHandler
    {

        public static async Task HandleMessage(SocketMessage message, WatsonWsServer server)
        {
            switch (message.SocketMessageType)
            {
                case SocketMessageType.START_ACTION:
                    await HandleStartAction(message, server);
                    break;
                case SocketMessageType.INFO:
                    await HandleInfo(message, server);
                    break;
                default:
                    await HandleUnknown(message, server);
                    break;
            }
        }

        private static async Task HandleStartAction(SocketMessage message, WatsonWsServer server)
        {
            // Do some sending
            var action = JsonConvert.DeserializeObject<SocketAction>(message.Data);
            Console.WriteLine($"[ACTION] \t {action}");

        }

        private static async Task HandleInfo(SocketMessage message, WatsonWsServer server)
        {
            // Send data back to the Teacher?
            var socketInfoMessage = JsonConvert.DeserializeObject<SocketInfoMessage>(message.Data);
            Console.WriteLine($"[{socketInfoMessage.Type}] \t {HostIps.GetHostByIp(message.IpPort)} -> {socketInfoMessage.Message}");

            var uiMessage = new WebUIInfoMessage(HostIps.hosts.First(x => message.IpPort.Contains(x.Ip)), socketInfoMessage.Message, socketInfoMessage.Type);
            await server.SendAsync(message.IpPort, JsonConvert.SerializeObject(uiMessage));

        }

        private static async Task HandleUnknown(SocketMessage message, WatsonWsServer server)
        {
            Console.WriteLine($"[WARNING] \t Received unknown SocketMessageType: {JsonConvert.SerializeObject(message)}");
        }
    }
}
