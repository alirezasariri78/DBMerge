using DBDiff.Models;
using DBDiff.queries;
using DBDiff.Repo;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace DBDiff.Repositories
{
    public class DataBaseSqlCommunication : SqlCommunication<DataBaseInstance, ServerInstance>
    {
        public DataBaseSqlCommunication(string connectionstring) : base(connectionstring) { }
        public override IEnumerable<DataBaseInstance> GetAll(ServerInstance parent)
        {
            List<DataBaseInstance> Result = new List<DataBaseInstance>();
            int DbId = 0;
            using (OleDbCommand com = new OleDbCommand(SqlQueries.GetDbQuery, _sqlconnection))
            {
                com.CommandType = CommandType.Text;
                using (OleDbDataReader dr = com.ExecuteReader())
                    while (dr.Read())
                        Result.Add(new DataBaseInstance()
                        {
                            Name = dr.IsDBNull(dr.GetOrdinal("name")) ? "" : dr.GetString(dr.GetOrdinal("name")),
                            DataBaseId = ++DbId,
                            ServerId = parent.ServerId,
                            Server = parent,
                        });
            }
            return Result;
        }

        public bool CreateForeignKeyConstraint(List<TableInstance> destinyTables, List<ForeignKeyCONSTRAINT> sourceForeignKeys)
        => ExecQuery(TableQueries.CreateForeignKeyQuery(destinyTables, sourceForeignKeys));

    }
}
