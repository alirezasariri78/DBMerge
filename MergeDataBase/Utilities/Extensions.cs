using System;
using System.Collections.Generic;

namespace DBDiff.Utilities
{
    public static class Extensions
    {
        public static bool IsFloatType(this string val)
        {
            List<string> FloatTypes = new List<string>()
            {
                "DECIMAL"
            };
            return FloatTypes.Contains(val.ToUpper());
        }

        public static string GetTypeDefualtValue(string val)
        {
            if (val.Contains("varchar"))
                return "'NULL'";
            if (val.Contains("datetime"))
                return "getdate()";
            if (val.Contains("decimal"))
                return "0";
            switch (val.ToLower())
            {
                case "uniqueidentifier": return Guid.Empty.ToString();            
                case "bigint":
                case "real":
                case "int":
                case "bit":
                case "float": return "0";
                default: return "";
            }
        }

    }
}
