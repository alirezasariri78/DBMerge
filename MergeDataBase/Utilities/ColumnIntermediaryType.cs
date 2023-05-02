
using System.Collections.Generic;

namespace MergeDataBase.Utilities
{
    internal class ColumnIntermediaryType
    {
        private static Dictionary<string, string> _conflict = new Dictionary<string, string>
        {
            {"image","nvarchar" },
            {"nvarchar","image" }
        };

        //private static Dictionary<string, string> _typeIntermediary = new Dictionary<string, string>
        //{
        //    {"image-nvarchar","varbinary" },
        //    {"nvarchar-image","varbinary" }
        //};


        public static bool HasConflict(string from, string to)
        => _conflict.ContainsKey(from) && _conflict.ContainsKey(to) && from != to;

        //public static string GetTypeIntermediary(ColumnInstance from, string to)
        //{
        //    int fromCharLocation = from.ColumnType.IndexOf("(", StringComparison.Ordinal);
        //    int toCharLocation = to.IndexOf("(", StringComparison.Ordinal);
        //    var len = to.IndexOf(")", StringComparison.Ordinal) - toCharLocation;
        //    if (fromCharLocation > 0)
        //        from.ColumnType = from.ColumnType.Substring(0, fromCharLocation);

        //    if (toCharLocation > 0)
        //        to = to.Substring(0, toCharLocation);
        //    if (toCharLocation > 0)
        //    {
        //        string parantheses = "(";
        //        parantheses += to.Substring(toCharLocation, len);
        //        parantheses += ")";
        //        return _typeIntermediary[$"{from.ColumnType}-{to}"] + parantheses;
        //    }
        //    else if (from.HasMaxCharLen)
        //        return _typeIntermediary[$"{from.ColumnType}-{to}"] + $"(MAX)";

        //    else if (from.NumericPrecision > 0)
        //        return _typeIntermediary[$"{from.ColumnType}-{to}"] + $"({from.NumericPrecision},{from.NumericScale})";
        //    else
        //        return _typeIntermediary[$"{from.ColumnType}-{to}"];

        //}

    }
}
