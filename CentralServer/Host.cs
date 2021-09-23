using System;
using System.Collections.Generic;
using System.Text;

namespace CentralServer
{
    public class Host
    {
        public string Ip { get; set; }
        public Hosts HostEnum { get; set; }
        public string HostName { get; set; }

        public Host(Hosts host, string ip, string hostName)
        {
            this.Ip = ip;
            this.HostEnum = host;
            this.HostName = hostName;
        }
    }
}
