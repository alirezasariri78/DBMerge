using DBDiff.Models;
using DBDiff.Repo;
using System.Collections.Generic;

namespace MergeDataBase.Manager
{
    public class ServerManager
    {
        private readonly ServerType _serverType;
        public ServerManager(ServerType type)
        {
            _serverType = type;
        }
        public IEnumerable<ServerInstance> GetAll(ServerAuthentication authentication, string servername)
          => new ServerRepository(_serverType).GetServersInstances(authentication, servername);
    }
}
