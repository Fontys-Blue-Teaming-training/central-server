using System.Linq;

namespace CentralServer.Hosts
{
    public static class HostIps
    {
        public static readonly Host[] hosts = new Host[]
            {
                new Host(HostNames.TEACHER_INTERFACE, "145.93.164.36", "Teacher interface"),
                new Host(HostNames.LINUX_HACKER, "145.93.164.xx", "Linux hacker"),
            };

        public static HostNames GetHostByIp(string ip) => hosts.First(x => ip.Contains(x.Ip)).HostEnum;
    }
}
