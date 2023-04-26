using DBDiff.Models;
using System.Collections.Generic;
using System.Linq;

namespace DBDiff.queries
{
    public class TableQueries
    {
        public static string DeleteTableQuery(string tableName)
            => $"DROP TABLE {tableName};";

        public static string AddTable(List<ColumnInstance> columns, string tablename)
        {
            string PrimaryKey = "";
            string ColsCreationQuery = "";
            foreach (var col in columns)
            {
                string SizeFormat = "({0}{1})";
                string size = "";
                if (col.ColumnType.ToLower() == "nvarchar")
                    size += string.Format(SizeFormat, col.HasMaxCharLen ? "MAX" : col.CharacterMaxLength.ToString(), "");
                else if (col.IsFloatType)
                    size += string.Format(SizeFormat, col.NumericPrecision, $",{col.NumericScale}");

                if (col.IsPrimaryKey)
                    PrimaryKey = col.Name;

                string NullState = col.Nullable ? "Null" : "Not Null";
                ColsCreationQuery += $"{col.Name} {col.ColumnType}{size} {NullState},\n";
            }
            if (columns.Any(c => c.IsPrimaryKey))
                ColsCreationQuery += $"PRIMARY KEY ({PrimaryKey})";
            return $"CREATE TABLE {tablename} ({ColsCreationQuery});";
        }

        public static string CreateForeignKeyQuery(List<TableInstance> tables, List<ForeignKeyCONSTRAINT> foreignKeys)
        {
            string Result = "";
            foreach (var fk in foreignKeys)
            {
                var table = tables.FirstOrDefault(c => c.Name.ToLower() == fk.FK_TABLE_NAME.ToLower());
                if (table == null)
                    continue;
                var singleAlterQuery = $@"ALTER TABLE {table.FullName}
                                        ADD FOREIGN KEY ({fk.FK_COLUMN_NAME}) REFERENCES {fk.PK_TABLE_NAME}({fk.PK_COLUMN_NAME});";
                Result += singleAlterQuery + "\n";
            }
            return Result;
        }
    }
}
