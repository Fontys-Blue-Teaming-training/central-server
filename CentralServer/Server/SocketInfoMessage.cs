namespace CentralServer.Server
{
    public class SocketInfoMessage
    {
        public Host Host { get; set; }
        public SocketInfoMessageType Type { get; set; }
        public string Message { get; set; }
    }
}
