using DBDiff.Models;
using DBDiff.Repo;
using System;
using System.Collections.Generic;
using System.Data;

namespace DBDiff.Repositories
{
    internal class TableSqlCommunication : SqlCommunication<TableInstance, DataBaseInstance>
    {
        public TableSqlCommunication(string connectionstring) : base(connectionstring)
        {

        }
        public override IEnumerable<TableInstance> GetAll(DataBaseInstance parent)
        {         
            List<TableInstance> Result = new List<TableInstance>();
            DataTable schema = _sqlconnection.GetSchema("Tables");
            int tableid = 0;
            foreach (DataRow row in schema.Rows)
            {
                if (row["TABLE_TYPE"].ToString()!= "TABLE")
                    continue;

                Result.Add(new TableInstance()
                {
                    Name = row["TABLE_NAME"].ToString(),
                    DataBase = parent,
                    TableId = ++tableid,
                    Schema = row["TABLE_SCHEMA"].ToString()
                });
            }
            return Result;
        }
    }
}
