using CentralServer.Hosts;

namespace CentralServer.Messages
{
    public class InfoMessage
    {
        public Host Host { get; set; }
        public InfoMessageType Type { get; set; }
        public string Message { get; set; }
    }
}
