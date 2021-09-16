namespace CentralServer.Server
{
    public class SocketInfoMessage
    {
        public string Device { get; set; }
        public SocketInfoMessageType Type { get; set; }
        public string Message { get; set; }
    }
}
