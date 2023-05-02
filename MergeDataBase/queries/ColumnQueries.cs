using DBDiff.Models;
using MergeDataBase.Utilities;
using System;

namespace DBDiff.queries
{
    internal class ColumnQueries
    {
        public static string GetColumnQuery(string tablename)
       => $"SELECT * FROM {tablename}";


        public static string UpdateColumnQuery(ColumnInstance col, string tablename, string type, bool nullable)
        {
            string nullState = nullable ? "Null" : "Not Null";
            string result = "";

            if (ColumnIntermediaryType.HasConflict(col.ColumnType.ToLower(), type.ToLower()))
            {
                result += DeleteColumnQuery(col.Name, tablename);
                result += AddColumnQuery(col.Name, tablename, type,nullable);
                return result;
            }
            else
                return $@"ALTER TABLE {tablename}
                ALTER COLUMN {col.Name} {type} {nullState};";
        }


        public static string DeleteColumnQuery(string ColumnName, string tablename)
            => $@"ALTER TABLE {tablename}
                  DROP COLUMN {ColumnName};";

        public static string AddColumnQuery(string ColumnName, string tablename, string colType, bool nullable)
        {
            string nullState = nullable ? "Null" : "Not Null";
            return $@"ALTER TABLE {tablename}
                ADD {ColumnName} {colType} {nullState};";
        }

        public static string SetColumnData(string columnName, string tableName, string data = "NULL")
            => $@"update {tableName} 
                 set {columnName}={data}";
    }
}
