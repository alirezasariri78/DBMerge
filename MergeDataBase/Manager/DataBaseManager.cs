using DBDiff.Models;
using DBDiff.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBDiff.Manager
{
    public class DataBaseManager : Manager<DataBaseInstance>
    {
        private readonly DataBaseRepository _repository;
        public DataBaseManager(ServerInstance serverInstance)
        => _repository = new DataBaseRepository(serverInstance);

        public override bool Merge(DataBaseInstance source, DataBaseInstance destiny, MergeOption option)
        {
            var sourceTableRepository = new TableRepository(source);
            var destinyTableRepository = new TableRepository(destiny);

            var destinyTables = destinyTableRepository.GetAll();
            var sourceTables = sourceTableRepository.GetAll();

            var deletedItems = destinyTables.Where(c => !sourceTables.Any(x => x.Name == c.Name));

            foreach (var item in deletedItems)
            {
                var delres = destinyTableRepository.DeleteTable(item.FullName);
                if (delres)
                    destinyTables.Remove(item);
            }


            foreach (var sourceTable in sourceTables)
            {
                if (!destinyTables.Any(c => c.FullName == sourceTable.FullName)) //Add Table
                {
                    var columnRepository = new ColumnRepository(sourceTable);
                    destinyTableRepository.CreateTable(columnRepository.GetAll(), sourceTable.FullName);
                }
                else //Merge Table
                {
                    var tableManager = new TableManager();
                    tableManager.Merge(sourceTable, destinyTables.Single(c => c.FullName == sourceTable.FullName), option);
                }
            }

            var sourceDataBaseRepository = new DataBaseRepository(source.Server, source);
            var destinyDataBaseRepository = new DataBaseRepository(destiny.Server, destiny);

            var sourceTableForeignKeys = sourceDataBaseRepository.GetDataBase_ForeignKeyCONSTRAINTs(sourceTables.Select(c => c.Name).ToArray());
            var destinyTableForeignKeys = destinyDataBaseRepository.GetDataBase_ForeignKeyCONSTRAINTs(destinyTables.Select(c => c.Name).ToArray());

            List<ForeignKeyCONSTRAINT> foreignKeyDiff = new List<ForeignKeyCONSTRAINT>();
            foreach (var sourceFK in sourceTableForeignKeys)
                if (!destinyTableForeignKeys.Any(c => c.FK_TABLE_NAME == sourceFK.FK_TABLE_NAME))
                    foreignKeyDiff.Add(sourceFK);

            destinyDataBaseRepository.CreateForeignKeyConstraint(sourceTables, foreignKeyDiff);

            destinyTableRepository.Dispose();
            sourceTableRepository.Dispose();

            return true;
        }

        public IEnumerable<DataBaseInstance> GetAll()
      => _repository.GetAll();


        public DataBaseInstance Find(Func<DataBaseInstance, bool> where)
        => _repository.Find(where);



        public IEnumerable<DataBaseInstance> GetAll(Func<DataBaseInstance, bool> where)
        => _repository.GetAll(where);


    }
}
