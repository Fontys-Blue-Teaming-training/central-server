using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralServer
{
    public static class HostIps
    {
        public static readonly Host[] hosts = new Host[]
            {
                new Host(Hosts.TEACHER_INTERFACE, "145.93.164.36"),
                new Host(Hosts.LINUX_HACKER, "145.93.164.xx"),
            };

        public static Hosts GetHostByIp(string ip) => hosts.First(x => ip.Contains(x.Ip)).HostName;
    }
}
