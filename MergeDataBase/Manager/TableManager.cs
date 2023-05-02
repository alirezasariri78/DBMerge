using DBDiff.Models;
using DBDiff.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBDiff.Manager
{
    public class TableManager : Manager<TableInstance>
    {
        private readonly TableRepository _repository;

        public TableManager(DataBaseInstance dataBase)
        => _repository = new TableRepository(dataBase);

        public IEnumerable<TableInstance> GetAll()
        => _repository.GetAll();

        public TableInstance Find(Func<TableInstance, bool> where)
        => _repository.Find(where);

        public IEnumerable<TableInstance> GetAll(Func<TableInstance, bool> where)
        => _repository.GetAll(where);

        public override bool Merge(TableInstance source, TableInstance destiny, MergeOption option)
        {
            List<ColumnInstance> SourceColumns = new List<ColumnInstance>();
            List<ColumnInstance> DestinyColumns = new List<ColumnInstance>();

            var sourceColumnRepository = new ColumnRepository(source);
            var destinyColumnRepository = new ColumnRepository(destiny);

            SourceColumns = sourceColumnRepository.GetAll();
            DestinyColumns = destinyColumnRepository.GetAll();

            var deletedItems = DestinyColumns.Where(c => !SourceColumns.Any(x => x.Name == c.Name));

            foreach (var item in deletedItems)
                destinyColumnRepository.DeleteColumn(item);

            foreach (var sourceColumn in SourceColumns)
            {
                if (!DestinyColumns.Any(c => c.Name == sourceColumn.Name)) //Add column
                    destinyColumnRepository.AddColumn(sourceColumn);
                else //Merge column
                {
                    var desColumn = DestinyColumns.Single(c => c.Name == sourceColumn.Name);
                    if (RequireUpdate(desColumn, sourceColumn))
                    {
                        if (desColumn.ColumnType != sourceColumn.ColumnType && !sourceColumn.Nullable)
                        {
                            destinyColumnRepository.UpdateColumn(desColumn, "NVARCHAR(MAX)", true, sourceColumn.DefualtValue);
                            destinyColumnRepository.SetColumnData(desColumn.Name, destiny.FullName, Extensions.GetTypeDefualtValue(sourceColumn.ColumnType));
                        }
                        else if (desColumn.ColumnType != sourceColumn.ColumnType && sourceColumn.Nullable)
                        {
                            destinyColumnRepository.UpdateColumn(desColumn, "NVARCHAR(MAX)", true, sourceColumn.DefualtValue);
                            destinyColumnRepository.SetColumnData(desColumn.Name, destiny.FullName, "NULL");
                        }
                        destinyColumnRepository.UpdateColumn(desColumn, sourceColumn.ColumnType, sourceColumn.Nullable, sourceColumn.DefualtValue);
                    }
                }
            }

            sourceColumnRepository.Dispose();
            destinyColumnRepository.Dispose();

            return true;
        }

        private bool RequireUpdate(ColumnInstance desColumn, ColumnInstance sourceColumn)
       => desColumn.ColumnType != sourceColumn.ColumnType ||
            desColumn.Nullable != sourceColumn.Nullable ||
            desColumn.DefualtValue != sourceColumn.DefualtValue;

    }
}
