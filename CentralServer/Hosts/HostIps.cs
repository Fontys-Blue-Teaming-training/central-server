﻿using System.Linq;

namespace CentralServer.Hosts
{
    public static class HostIps
    {
        public static readonly Host[] hosts = new Host[]
            {
                new Host(HostNames.TEACHER_INTERFACE, "192.168.1.3", "Teacher interface"),
                new Host(HostNames.LINUX_HACKER, "172.16.1.3", "Linux hacker"),
                new Host(HostNames.TARGET_MACHINE, "192.168.1.5", "Linux SSH bruteforce target"),
                new Host(HostNames.STUDENT_INTERFACE, "192.168.1.7", "Student UI web interface"),
                new Host(HostNames.FORENSICS_MACHINE_1, "192.168.1.22", "Forensics student machine 1"),
                new Host(HostNames.FORENSICS_MACHINE_2, "192.168.1.24", "Forensics student machine 2"),
                new Host(HostNames.FORENSICS_MACHINE_3, "192.168.1.23", "Forensics student machine 3"),
                new Host(HostNames.FORENSICS_MACHINE_4, "192.168.1.25", "Forensics student machine 4"),
                new Host(HostNames.FORENSICS_MACHINE_5, "192.168.1.26", "Forensics student machine 5"),
            };

        public static HostNames GetHostByIp(string ip) => hosts.First(x => ip.Contains(x.Ip)).HostEnum;
    }
}
