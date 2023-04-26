namespace DBDiff.Models
{
    public class ServerInstance
    {
        public ServerInstance()
        {
            Authentication = new ServerAuthentication()
            {
                UseWindowsAuthentication = true
            };
        }
        public ServerInstance(ServerAuthentication serverAuthentication)
        => Authentication = serverAuthentication;

        public int ServerId { get; set; }
        public string Name { get; set; }
        public bool IsLocal { get; set; }
        public ServerAuthentication Authentication { get; set; }
    }
}
