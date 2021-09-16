namespace CentralServer.Server
{
    public class SocketMessage
    {
        public string IpPort { get; set; }
        public SocketMessageType SocketMessageType { get; set; }
        public string Data { get; set; }
    }
}
