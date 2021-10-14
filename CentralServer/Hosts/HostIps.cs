using System.Linq;

namespace CentralServer.Hosts
{
    public static class HostIps
    {
        public static readonly Host[] hosts = new Host[]
            {
                new Host(HostNames.TEACHER_INTERFACE, "192.168.1.3", "Teacher interface"),
                new Host(HostNames.LINUX_HACKER, "172.16.1.3", "Linux hacker"),
                new Host(HostNames.LINUX_SSH_TARGET, "192.168.1.5", "Linux SSH bruteforce target")
            };

        public static HostNames GetHostByIp(string ip) => hosts.First(x => ip.Contains(x.Ip)).HostEnum;
    }
}
