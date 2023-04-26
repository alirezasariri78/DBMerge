using DBDiff.queries;

namespace DBDiff.Models
{
    public  class SqlQueries
    {
        public static string GetDbQuery = @"use master  select * from sys.databases";
    }
}
