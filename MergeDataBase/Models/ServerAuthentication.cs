namespace DBDiff.Models
{
    public class ServerAuthentication
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseWindowsAuthentication { get; set; } = true;
    }
}
