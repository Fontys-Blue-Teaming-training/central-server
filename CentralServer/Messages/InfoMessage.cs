using CentralServer.Hosts;

namespace CentralServer.Messages
{
    public class InfoMessage
    {
        public Host Host { get; set; } = null;
        public InfoMessageType Type { get; set; }
        public string Message { get; set; }

        public InfoMessage()
        {

        }

        public InfoMessage(Host host, InfoMessageType type, string message)
        {
            Host = host;
            Type = type;
            Message = message;
        }

        public InfoMessage(InfoMessageType type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}
