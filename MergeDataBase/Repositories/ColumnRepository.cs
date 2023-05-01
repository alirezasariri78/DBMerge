using DBDiff.abstracts;
using DBDiff.Models;
using DBDiff.queries;
using DBDiff.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBDiff.Manager
{
    internal class ColumnRepository : IEntityManager<ColumnInstance>, IDisposable
    {
        private readonly ColumnSqlCommunication _sqlCommunication;
        private readonly TableInstance _tableInstance;
        public ColumnRepository(TableInstance tableinstance)
        {
            _sqlCommunication = new ColumnSqlCommunication(ConnectionStrings.GetConnectionString(tableinstance.DataBase.Server, tableinstance.DataBase));
            _tableInstance = tableinstance;
        }

        public void Dispose()
        => _sqlCommunication.Dispose();

        public ColumnInstance Find(Func<ColumnInstance, bool> where)
        => GetAll(where).FirstOrDefault();

        public List<ColumnInstance> GetAll()
       => _sqlCommunication.GetAll(_tableInstance).ToList();

        public List<ColumnInstance> GetAll(Func<ColumnInstance, bool> where)
        => GetAll().Where(where).ToList();

        public bool UpdateColumn(ColumnInstance DestinycolumnInstance, string newType, bool Nullable)
           => _sqlCommunication.ExecQuery(ColumnQueries.UpdateColumnQuery(DestinycolumnInstance.Name, DestinycolumnInstance.Table.Name, newType, Nullable));


        public bool DeleteColumn(ColumnInstance columnInstance)
                    => _sqlCommunication.ExecQuery(ColumnQueries.DeleteColumnQuery(columnInstance.Name, columnInstance.Table.Name));


        public bool AddColumn(ColumnInstance columnInstance)
                    => _sqlCommunication.ExecQuery(ColumnQueries.AddColumnQuery(columnInstance.Name, columnInstance.Table.Name, columnInstance.ColumnType));

        public bool SetColumnData(string columnName, string tableName,string data)
                    => _sqlCommunication.ExecQuery(ColumnQueries.SetColumnData(columnName, tableName, data));

    }
}
