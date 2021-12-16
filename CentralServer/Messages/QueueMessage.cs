using CentralServer.Hosts;

namespace CentralServer.Messages
{
    public class QueueMessage
    {
        public Host Host { get; set; }
        public string Message { get; set; }
    }
}
