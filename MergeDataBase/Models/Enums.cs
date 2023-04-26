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
    public enum ServerType
    { 
        Local,Remote
    }

}
