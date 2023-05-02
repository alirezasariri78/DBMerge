using DBDiff.Models;
using DBDiff.Repo;
using System.Collections.Generic;

namespace DBDiff.Manager
{
    public class ServerManager
    {
        private readonly ServerRepository _serverRepository;
        public ServerManager(ServerAuthentication authentication, string serverName = ".")
          => _serverRepository = new ServerRepository(authentication, serverName);

        public IEnumerable<ServerInstance> GetAllLocalServers(ServerAuthentication authentication, string servername)
          => _serverRepository.GetLocalServersInstances();

        public ServerInstance GetInstance()
            => _serverRepository.GetInstance();

    }
}
