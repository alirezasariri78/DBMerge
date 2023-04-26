using DBDiff.Manager;
using DBDiff.Models;
using DBDiff.Repo;

namespace DBDiff
{
    public class Program
    {
        static void Main(string[] args)
        {
            var Destinyservers = new ServerRepository(ServerType.Local).GetServersInstances(new Models.ServerAuthentication()
            {
                Password = "1234",
                UserName = "alireza",
                UseWindowsAuthentication = false
            });
            var desserver = Destinyservers[0];


            var sourceservers = new ServerRepository(ServerType.Remote).GetServersInstances(new Models.ServerAuthentication()
            {
                Password = "",
                UserName = "",
                UseWindowsAuthentication = false
            }, "");

            var sourceserver = sourceservers[0];

            var sourcedbrep = new DataBaseRepository(sourceserver);
            var destinydbrep = new DataBaseRepository(desserver);

            // merge databases
            new DataBaseManager().Merge(sourcedbrep.Find(c => c.Name == ""), destinydbrep.Find(c => c.Name == ""), new MergeOption());

            // merge tables 
            new TableManager().Merge(new TableInstance(), new TableInstance(), new MergeOption());
        }
    }
}
