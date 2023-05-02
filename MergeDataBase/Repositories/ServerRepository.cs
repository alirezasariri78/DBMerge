using DBDiff.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBDiff.Repo
{
    internal class ServerRepository
    {
        private readonly ServerAuthentication _auth;
        private readonly string _serverName;
        public ServerRepository(ServerAuthentication authentication, string serverName = ".")
        {
            _auth = authentication;
            _serverName = serverName;
        }

        private const string RegistarySqlInstacePath = @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL";
        public List<ServerInstance> GetLocalServersInstances()
        {
            List<ServerInstance> ServerInstances = new List<ServerInstance>();
            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey registrtKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = registrtKey.OpenSubKey(RegistarySqlInstacePath, false);
                if (instanceKey != null)
                    ServerInstances.AddRange(instanceKey.GetValueNames().Select((c, i) => new ServerInstance()
                    {
                        Name = c.ToLower() == "mssqlserver" ? "." : $@".\{c}",
                        ServerId = ++i,
                        Authentication = _auth
                    }));

            }
            return ServerInstances;
        }

        public ServerInstance GetInstance()
        {
            return new ServerInstance()
            { Authentication = _auth, Name = _serverName, ServerId = 1 };
        }
    }
}
