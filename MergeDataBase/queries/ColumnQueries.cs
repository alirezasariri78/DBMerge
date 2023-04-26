namespace DBDiff.queries
{
    public class ColumnQueries
    {
        public static string GetColumnQuery(string tablename)
       => $"SELECT * FROM {tablename}";


        public static string UpdateColumnQuery(string ColumnName, string tablename, string type, bool nullable)
        {
            string nullState = nullable ? "Null" : "Not Null";
            return $@"ALTER TABLE {tablename}
                ALTER COLUMN {ColumnName} {type} {nullState};";
        }


        public static string DeleteColumnQuery(string ColumnName, string tablename)
            => $@"ALTER TABLE {tablename}
                  DROP COLUMN {ColumnName};";

        public static string AddColumnQuery(string ColumnName, string tablename, string colType)
          => $@"ALTER TABLE {tablename}
                ADD {ColumnName} {colType};";

        public static string SetColumnData(string columnName, string tableName,string data="NULL")
            => $@"update {tableName} 
                 set {columnName}={data}";
    }
}
