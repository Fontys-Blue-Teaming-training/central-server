namespace CentralServer.Hosts
{
    public class Host
    {
        public string Ip { get; set; }
        public HostNames HostEnum { get; set; }
        public string HostName { get; set; }

        public Host(HostNames host, string ip, string hostName)
        {
            Ip = ip;
            HostEnum = host;
            HostName = hostName;
        }
    }
}
