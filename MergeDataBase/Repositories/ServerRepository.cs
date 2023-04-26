using DBDiff.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBDiff.Repo
{
    public class ServerRepository
    {
        private readonly ServerType _serverType;
        public ServerRepository(ServerType type) => _serverType = type;


        private const string RegistarySqlInstacePath = @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL";
        public List<ServerInstance> GetServersInstances(ServerAuthentication authentication, string name = "")
        {
            List<ServerInstance> ServerInstances = new List<ServerInstance>();

            if (_serverType == ServerType.Local)
            {
                RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
                using (RegistryKey registrtKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
                {
                    RegistryKey instanceKey = registrtKey.OpenSubKey(RegistarySqlInstacePath, false);
                    if (instanceKey != null)
                        ServerInstances.AddRange(instanceKey.GetValueNames().Select((c, i) => new ServerInstance()
                        {
                            Name = c.ToLower() == "mssqlserver" ? "." : $@".\{c}",
                            IsLocal = true,
                            ServerId = ++i,
                            Authentication = authentication
                        }));

                }
            }
            else
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                ServerInstances.Add(new ServerInstance()
                {
                    Authentication = authentication,
                    IsLocal = false,
                    Name = name,
                    ServerId = 1
                });
            }

            return ServerInstances;
        }

    }
}
