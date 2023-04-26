using Microsoft.SqlServer.Server;
using System;
using System.Data;
using System.Data.OleDb;

namespace DBDiff.Utilities
{
    public static class TypeExtension
    {
        public static SqlDbType ToSqlDbType(this Type clrType)
        {
            var s = new SqlMetaData("", SqlDbType.NVarChar, clrType);
            return s.SqlDbType;
        }


        public static Type ToClrType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return typeof(long?);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return typeof(bool?);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    return typeof(DateTime?);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return typeof(decimal?);

                case SqlDbType.Float:
                    return typeof(double?);

                case SqlDbType.Int:
                    return typeof(int?);

                case SqlDbType.Real:
                    return typeof(float?);

                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid?);

                case SqlDbType.SmallInt:
                    return typeof(short?);

                case SqlDbType.TinyInt:
                    return typeof(byte?);

                case SqlDbType.Variant:
                case SqlDbType.Udt:
                    return typeof(object);

                case SqlDbType.Structured:
                    return typeof(DataTable);

                case SqlDbType.DateTimeOffset:
                    return typeof(DateTimeOffset?);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }

        public static int ToInt(this string val)
        => string.IsNullOrEmpty(val) ? 0 : int.Parse(val);
        public static string ToOleDbTypeToString(this OleDbType type)
        {
            switch (type)
            {
                case OleDbType.Binary:
                    return "Binary";
                case OleDbType.Boolean:
                    return "Bit";
                case OleDbType.UnsignedTinyInt:
                    return "Byte";
                case OleDbType.TinyInt:
                    return "TinyInt";
                case OleDbType.Integer:
                    return "int";
                case OleDbType.Currency:
                    return "Currency";
                case OleDbType.Date:
                case OleDbType.DBDate:
                case OleDbType.DBTimeStamp:
                    return "DateTime";
                case OleDbType.Double:
                    return "Float";
                case OleDbType.Guid:
                    return "UniqueIdentifier";
                case OleDbType.Char:
                    return "Text";
                case OleDbType.WChar:
                case OleDbType.VarWChar:
                    return "NVarchar";
                case OleDbType.Single:
                    return "Real";
                case OleDbType.SmallInt:
                    return "SmallInt";
                case OleDbType.Numeric:
                case OleDbType.Decimal:
                    return "Decimal";
                default:
                    return "Unknown";
            }
        }
    }
}
