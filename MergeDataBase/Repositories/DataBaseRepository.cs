using DBDiff.abstracts;
using DBDiff.Models;
using DBDiff.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBDiff.Repo
{
    internal class DataBaseRepository : IEntityManager<DataBaseInstance>, IDisposable
    {
        private readonly DataBaseSqlCommunication _sqlCommunication;
        private readonly ServerInstance _server;
        public DataBaseRepository(ServerInstance serverInstance, DataBaseInstance dataBaseInstance = null)
        {
            _server = serverInstance;
            _sqlCommunication = new DataBaseSqlCommunication(ConnectionStrings.GetConnectionString(serverInstance, dataBaseInstance));
        }

        public void Dispose()
        => _sqlCommunication.Dispose();

        public DataBaseInstance Find(Func<DataBaseInstance, bool> where)
        => GetAll(where).FirstOrDefault();

        public List<DataBaseInstance> GetAll()
        => _sqlCommunication.GetAll(_server).ToList();

        public List<DataBaseInstance> GetAll(Func<DataBaseInstance, bool> where)
        => GetAll().Where(where).ToList();

        public List<ForeignKeyCONSTRAINT> GetDataBase_ForeignKeyCONSTRAINTs(params string[] tables)
             => _sqlCommunication.GetAllForeignKeyCONSTRAINT(tables);


        /// <summary>
        /// Create CreateForeignKeyConstraint For Given Tables by Givent foreignKeys
        /// </summary>
        /// <param name="destinyTables"> Destiny Database Tables </param>
        /// <param name="sourceForeignKeys"> Foreign Keys From SourceTable </param>
        /// <returns></returns>
        public bool CreateForeignKeyConstraint(List<TableInstance> destinyTables, List<ForeignKeyCONSTRAINT> sourceForeignKeys)
            => _sqlCommunication.CreateForeignKeyConstraint(destinyTables, sourceForeignKeys);

    }
}