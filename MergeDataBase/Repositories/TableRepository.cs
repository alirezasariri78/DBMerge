using DBDiff.abstracts;
using DBDiff.Models;
using DBDiff.queries;
using DBDiff.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBDiff.Manager
{
    public class TableRepository : IEntityManager<TableInstance>, IDisposable
    {
        private readonly TableSqlCommunication _sqlCommunication;
        private readonly DataBaseInstance _dataBaseInstance;
        public TableRepository(DataBaseInstance databaseinstance)
        {
            _dataBaseInstance = databaseinstance;
            _sqlCommunication = new TableSqlCommunication(ConnectionStrings.GetConnectionString(databaseinstance.Server, databaseinstance));
        }

        public void Dispose()
        => _sqlCommunication.Dispose();

        public TableInstance Find(Func<TableInstance, bool> where)
        => GetAll(where).FirstOrDefault();

        public List<TableInstance> GetAll()
        => _sqlCommunication.GetAll(_dataBaseInstance).ToList();

        public List<TableInstance> GetAll(Func<TableInstance, bool> where)
        => GetAll().Where(where).ToList();

        public bool CreateTable(List<ColumnInstance> columns, string tablename)
        => _sqlCommunication.ExecQuery(TableQueries.AddTable(columns, tablename));


        public bool DeleteTable(string tablename)
        => _sqlCommunication.ExecQuery(TableQueries.DeleteTableQuery(tablename));

        public bool AlterTable(List<ColumnInstance> neededColumns, string tablename, AlterOperationType operationType)
        => _sqlCommunication.ExecQuery(
            string.Join("\n", neededColumns.Select(c => ColumnQueryByOperationType(c, tablename, operationType)).ToArray())
            );

        private string ColumnQueryByOperationType(ColumnInstance columnInstance, string tablename, AlterOperationType operationType)
        {
            switch (operationType)
            {
                case AlterOperationType.ChangeType:
                    return ColumnQueries.UpdateColumnQuery(columnInstance.Name, tablename, columnInstance.ColumnType, columnInstance.Nullable);
                case AlterOperationType.Add:
                    return ColumnQueries.AddColumnQuery(columnInstance.Name, tablename, columnInstance.ColumnType);
                case AlterOperationType.Delete:
                    return ColumnQueries.DeleteColumnQuery(columnInstance.Name, tablename);
                default: return "";
            }
        }
    }
}
