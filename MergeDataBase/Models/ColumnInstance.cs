using DBDiff.Utilities;

namespace DBDiff.Models
{
    public class ColumnInstance
    {
        private string _name;

        public int ColumnId { get; set; }
        public int TableId { get; set; }
        public string ColumnType { get; set; }
        public string Name { get => $"[{_name}]"; set => _name = value; }
        public TableInstance Table { get; set; }
        public bool Nullable { get; set; }
        public int CharacterMaxLength { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool HasMaxCharLen { get => CharacterMaxLength > 4000 ? true : false; }
        public int NumericPrecision { get; set; }
        public int NumericScale { get; set; }
        public string DefualtValue { get; set; }
        public bool IsFloatType { get => ColumnType.IsFloatType(); }
        public ForeignKeyCONSTRAINT ForeignKeyCONSTRAINT { get; set; }

    }

    public class ForeignKeyCONSTRAINT
    {
        public string FK_NAME { get; set; }
        public string FK_TABLE_NAME { get; set; }
        public string FK_COLUMN_NAME { get; set; }
        public string PK_TABLE_NAME { get; set; }
        public string PK_COLUMN_NAME { get; set; }

    }
}
