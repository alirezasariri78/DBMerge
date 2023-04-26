using DBDiff.Models;

namespace DBDiff.Manager
{
    public abstract class Manager<ModelType> where ModelType : class
    {
        public abstract bool Merge(ModelType source, ModelType destiny,MergeOption option);

    }
}
