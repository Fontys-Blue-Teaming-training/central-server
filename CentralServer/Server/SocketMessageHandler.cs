using Newtonsoft.Json;
using System;

namespace CentralServer.Server
{
    public static class SocketMessageHandler
    {

        public static void HandleMessage(SocketMessage message)
        {
            switch (message.SocketMessageType)
            {
                case SocketMessageType.START_ACTION:
                    HandleStartAction(message);
                    break;
                case SocketMessageType.INFO:
                    HandleInfo(message);
                    break;
                default:
                    HandleUnknown(message);
                    break;
            }
        }

        private static void HandleStartAction(SocketMessage message)
        {
            // Do some sending
            var action = JsonConvert.DeserializeObject<SocketAction>(message.Data);
            Console.WriteLine($"[ACTION] \t {action}");

        }

        private static void HandleInfo(SocketMessage message)
        {
            // Send data back to the Teacher?
            var socketInfoMessage = JsonConvert.DeserializeObject<SocketInfoMessage>(message.Data);
            Console.WriteLine($"[{socketInfoMessage.Type}] \t {socketInfoMessage.Device} -> {socketInfoMessage.Message}");

        }

        private static void HandleUnknown(SocketMessage message)
        {
            Console.WriteLine($"[WARNING] \t Received unknown SocketMessageType: {JsonConvert.SerializeObject(message)}");
        }
    }
}
