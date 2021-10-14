using CentralServer.Hosts;

namespace CentralServer.Messages
{
    public class WebUIInfoMessage
    {
        public Host Host { get; set; }
        public string Message { get; set; }
        public InfoMessageType InfoType { get; set; }

        public WebUIInfoMessage(Host host, string message, InfoMessageType infoType)
        {
            Host = host;
            Message = message;
            InfoType = infoType;
        }
    }
}
