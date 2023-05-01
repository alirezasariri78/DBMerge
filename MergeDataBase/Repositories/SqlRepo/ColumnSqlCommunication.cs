using DBDiff.Models;
using DBDiff.Repo;
using DBDiff.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;

namespace DBDiff.Repositories
{
    internal class ColumnSqlCommunication : SqlCommunication<ColumnInstance, TableInstance>
    {
        private Dictionary<string, string> ColumnType;
        private readonly string _connectionstring;
        public ColumnSqlCommunication(string connectionstring) : base(connectionstring)
        {
            _connectionstring = connectionstring;
            ColumnType = new Dictionary<string, string>();
        }
        public override IEnumerable<ColumnInstance> GetAll(TableInstance parent)
        {
            // sqloledb does not get type of mssql field correctly 
            // so we use sqlconnection class to get Correct types of columns
            // sqlconnection cant get primary keys . so we use OledBConnection To Get primary keys 
            SetColumeDataType(_connectionstring, parent.Name);

            var foreignKeyCONSTRAINTs = GetAllForeignKeyCONSTRAINT();
            int colid = 0;
            var columnSchema = _sqlconnection.GetSchema(
             OleDbMetaDataCollectionNames.Columns, new string[] { null, null, parent.Name, null });

            string PrimaryKey = GetTablePrimaryKey(parent.Name);

            foreach (DataRow col in columnSchema.Rows)
            {
                var colType = ((OleDbType)col["DATA_TYPE"].ToString().ToInt()).ToOleDbTypeToString();
                yield return new ColumnInstance()
                {
                    Name = col["COLUMN_NAME"].ToString(),
                    CharacterMaxLength = col["CHARACTER_MAXIMUM_LENGTH"].ToString().ToInt(),
                    ColumnId = ++colid,
                    TableId = parent.TableId,
                    Table = parent,
                    ColumnType = ColumnType[col["COLUMN_NAME"].ToString()],
                    Nullable = bool.Parse(col["IS_NULLABLE"].ToString()),
                    IsPrimaryKey = col["COLUMN_NAME"].ToString().ToLower() == PrimaryKey.ToLower() ? true : false,
                    NumericPrecision = col["NUMERIC_PRECISION"].ToString().ToInt(),
                    NumericScale = col["NUMERIC_SCALE"].ToString().ToInt(),
                    ForeignKeyCONSTRAINT = GetForeignKeyCONSTRAINT(foreignKeyCONSTRAINTs, parent.Name, col["COLUMN_NAME"].ToString()),
                };
            }
        }



        private void SetColumeDataType(string connectionstring, string tablename)
        {
            ColumnType.Clear();
            using (SqlConnection sqlconnection = new SqlConnection(connectionstring.Replace("Provider=sqloledb;", "")))
            {
                sqlconnection.Open();
                DataTable ColInfo = sqlconnection.GetSchema("Columns", new string[] { null, null, tablename, null });
                foreach (DataRow row in ColInfo.Rows)
                    ColumnType[row["COLUMN_NAME"].ToString()] = row["DATA_TYPE"].ToString();
            }
        }

        private ForeignKeyCONSTRAINT GetForeignKeyCONSTRAINT(List<ForeignKeyCONSTRAINT> collection, string tableName, string columnName)
           => collection.FirstOrDefault(c => c.FK_TABLE_NAME.ToLower() == tableName.ToLower()
                                         && c.FK_COLUMN_NAME == columnName);
    
    }
}
