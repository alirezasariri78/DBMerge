namespace DBDiff.Models
{
    public class TableInstance
    {
        public DataBaseInstance DataBase { get; set; }
        public int TableId { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        public string FullName { get => (string.IsNullOrEmpty(Schema) ? "dbo" : Schema) + "." + Name; }
    }
}
