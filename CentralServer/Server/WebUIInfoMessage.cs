namespace CentralServer.Server
{
    public class WebUIInfoMessage
    {
        public Host Host { get;set }
        public string Message { get; set; }
        public SocketInfoMessageType InfoType { get; set; }

        public WebUIInfoMessage(Host host, string message, SocketInfoMessageType infoType)
        {
            Host = host;
            Message = message;
            InfoType = infoType;
        }
    }
}
