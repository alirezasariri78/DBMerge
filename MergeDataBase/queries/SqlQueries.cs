using DBDiff.queries;

namespace DBDiff.Models
{
    internal class SqlQueries
    {
        public static string GetDbQuery = @"use master  select * from sys.databases";
    }
}
