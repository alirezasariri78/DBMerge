using DBDiff.Models;
using MergeDataBase.Utilities;

namespace DBDiff.queries
{
    internal class ColumnQueries
    {
        public static string GetColumnQuery(string tablename)
        => $"SELECT * FROM {tablename}";


        public static string UpdateColumnQuery(ColumnInstance col, string tablename, string type, bool nullable, string sourceDefualtvalue)
        {

            string nullState = nullable ? "Null" : "Not Null";
            string result = "";


            if (ColumnIntermediaryType.HasConflict(col.ColumnType.ToLower(), type.ToLower()))
            {
                result += DeleteColumnQuery(col.Name, tablename);
                result += AddColumnQuery(col, type, tablename, sourceDefualtvalue);
                return result;
            }
            else
            {
                result = $@"ALTER TABLE {tablename}
                            ALTER COLUMN {col.Name} {type} {nullState};";


                if (!string.IsNullOrEmpty(sourceDefualtvalue))
                    result += $@"ALTER TABLE {tablename}
                             ADD CONSTRAINT df_{col.Name.Replace("[", "").Replace("]", "")}_{tablename}
                             DEFAULT {sourceDefualtvalue} FOR {col.Name} ;";
            }
            return result;
        }


        public static string DeleteColumnQuery(string ColumnName, string tablename)
            => $@"ALTER TABLE {tablename}
                  DROP COLUMN {ColumnName};";

        public static string AddColumnQuery(ColumnInstance col, string type, string tablename, string sourceDefualtvalue)
        {
            string nullState = col.Nullable ? "Null" : "Not Null";
            string defualtQuery = string.IsNullOrEmpty(sourceDefualtvalue) ? "" : $" DEFAULT {sourceDefualtvalue}";
            return $@"ALTER TABLE {tablename}
                ADD {col.Name} {type} {nullState} {defualtQuery};";
        }

        public static string SetColumnData(string columnName, string tableName, string data = "NULL")
            => $@"update {tableName} 
                 set {columnName}={data}";
    }
}
