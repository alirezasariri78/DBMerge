using DBDiff.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace DBDiff.Repo
{
    internal abstract class SqlCommunication<Entity, ParentEntity> : IDisposable where Entity : class
    {
        protected OleDbConnection _sqlconnection;
        public SqlCommunication(string connectionstring)
        {
            _sqlconnection = new OleDbConnection();
            _sqlconnection.ConnectionString = connectionstring;
            _sqlconnection.Open();
        }
        public bool ExecQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
                return true;
            using (OleDbCommand command = new OleDbCommand(query, _sqlconnection))
            {
                command.CommandType = CommandType.Text;
                return command.ExecuteNonQuery() >= 0;
            }
        }

        public List<ForeignKeyCONSTRAINT> GetAllForeignKeyCONSTRAINT(params string[] tables)
        {
            List<ForeignKeyCONSTRAINT> foreignKeyCONSTRAINTs = new List<ForeignKeyCONSTRAINT>();
            DataTable ForeignKeysSchema;
            ForeignKeysSchema = _sqlconnection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new string[0]);
            foreach (DataRow row in ForeignKeysSchema.Rows)
            {
                if (tables.Length > 0 && !tables.Contains(row["FK_TABLE_NAME"].ToString()))
                    continue;

                if (tables.Length > 0 && !tables.Contains(row["PK_TABLE_NAME"].ToString()))
                    continue;

                foreignKeyCONSTRAINTs.Add(new ForeignKeyCONSTRAINT()
                {
                    FK_NAME = row["FK_NAME"].ToString(),
                    FK_COLUMN_NAME = row["FK_COLUMN_NAME"].ToString(),
                    FK_TABLE_NAME = row["FK_TABLE_NAME"].ToString(),
                    PK_COLUMN_NAME = row["PK_COLUMN_NAME"].ToString(),
                    PK_TABLE_NAME = row["PK_TABLE_NAME"].ToString(),
                });
            }


            return foreignKeyCONSTRAINTs;
        }

        public string GetTablePrimaryKey(string tablename)
        {
            DataTable ForeignKeysSchema =
                _sqlconnection.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, new string[] { });

            foreach (DataRow item in ForeignKeysSchema.Rows)
                if (item["TABLE_NAME"].ToString().ToLower() == tablename.ToLower())
                    return item["COLUMN_NAME"].ToString();
            return "";
        }

        public abstract IEnumerable<Entity> GetAll(ParentEntity parent);

        public void Dispose()
        {
            _sqlconnection.Close();
            _sqlconnection?.Dispose();
        }
    }
}
