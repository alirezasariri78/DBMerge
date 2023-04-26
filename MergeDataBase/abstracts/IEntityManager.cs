using DBDiff.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DBDiff.abstracts
{
    public interface IEntityManager<ModelType> where ModelType : class
    {
        List<ModelType> GetAll();
        List<ModelType> GetAll(Func<ModelType, bool> where);
        ModelType Find(Func<ModelType, bool> where);
    }
}
