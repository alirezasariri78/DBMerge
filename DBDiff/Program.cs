using DBDiff.Manager;
using DBDiff.Models;
using DBDiff.Repo;

namespace DBDiff
{
    public class Program
    {
        static void Main(string[] args)
        {
            var LocalServerManager = new DBDiff.Manager.ServerManager(new DBDiff.Models.ServerAuthentication() { Password = "1234", UserName = "alireza", UseWindowsAuthentication = false });
            var HubServerManager = new DBDiff.Manager.ServerManager(new DBDiff.Models.ServerAuthentication()
            {
                UseWindowsAuthentication = false,
                Password = "ZG[%8)ns9j2k7HPx",
                UserName = "sa"
            }, "45.159.150.100");

            var DestinyDataBaseManager = new DBDiff.Manager.DataBaseManager(LocalServerManager.GetInstance());
            var SourceDataBaseManager = new DBDiff.Manager.DataBaseManager(HubServerManager.GetInstance());

            const string DbName = "MatchManagerDb";
            var LocalMMDb = DestinyDataBaseManager.Find(c => c.Name == DbName);
            var HubMMDb = SourceDataBaseManager.Find(c => c.Name == DbName);

            DestinyDataBaseManager.Merge(HubMMDb, LocalMMDb, new DBDiff.Models.MergeOption());
        }
    }
}
