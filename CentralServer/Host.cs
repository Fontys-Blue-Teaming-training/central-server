using System;
using System.Collections.Generic;
using System.Text;

namespace CentralServer
{
    public class Host
    {
        public string Ip { get; set; }
        public Hosts HostName { get; set; }

        public Host(Hosts host, string ip)
        {
            this.Ip = ip;
            this.HostName = host;
        }
    }
}
