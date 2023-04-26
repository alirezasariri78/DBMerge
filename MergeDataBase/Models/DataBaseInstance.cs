namespace DBDiff.Models
{
    public class DataBaseInstance
    {
        public int DataBaseId { get; set; }
        public int ServerId { get; set; }
        public string Name { get; set; }
        public ServerInstance Server { get; set; }
    }
}
