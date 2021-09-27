﻿using CentralServer.Hosts;
using CentralServer.Server;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CentralServer
{
    static class Program
    {

        // https://github.com/jchristn/WatsonWebsocket
        static async Task Main(string[] args)
        {
            Console.WriteLine("Setting up server...");
            SocketServer.SetupServer();
            await SocketServer.StartServer();
            Console.WriteLine("Server started!");
            while(true)
            {
                Console.WriteLine("Press enter to send message");
                Console.ReadLine();
                await SocketServer.SendMessage(HostIps.hosts.First(x => x.HostEnum == HostNames.TEACHER_INTERFACE).Ip, "Hey");
            }


            Console.ReadLine();
        }
    }
}
