using DBDiff.Models;
using DBDiff.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DBDiff.Manager
{
    public class TableManager : Manager<TableInstance>
    {
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
                    if (desColumn.ColumnType != sourceColumn.ColumnType || desColumn.Nullable != sourceColumn.Nullable)
                    {
                        if (desColumn.ColumnType != sourceColumn.ColumnType && !sourceColumn.Nullable)
                        {
                            destinyColumnRepository.UpdateColumn(desColumn, "NVARCHAR(MAX)", true);
                            destinyColumnRepository.SetColumnData(desColumn.Name, destiny.FullName, Extensions.GetTypeDefualtValue(sourceColumn.ColumnType));
                        }
                        else if (desColumn.ColumnType != sourceColumn.ColumnType && sourceColumn.Nullable)
                        {
                            destinyColumnRepository.UpdateColumn(desColumn, "NVARCHAR(MAX)", true);
                            destinyColumnRepository.SetColumnData(desColumn.Name, destiny.FullName, "NULL");
                        }
                        destinyColumnRepository.UpdateColumn(desColumn, sourceColumn.ColumnType, sourceColumn.Nullable);
                    }
                }
            }

            sourceColumnRepository.Dispose();
            destinyColumnRepository.Dispose();

            return true;
        }
    }
}
