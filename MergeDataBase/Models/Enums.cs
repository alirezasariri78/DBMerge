namespace DBDiff.Models
{
    public enum MergeType
    {
        SchemaOnly, DataOnly, SchemaAndData
    }

    public enum AlterOperationType
    {
        Add,ChangeType, Delete
    }
}
