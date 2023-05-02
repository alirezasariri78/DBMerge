namespace DBDiff.Models
{
    public class ServerAuthentication
    {
        public ServerAuthentication()
        => UseWindowsAuthentication = true;

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseWindowsAuthentication { get; set; }
    }
}
