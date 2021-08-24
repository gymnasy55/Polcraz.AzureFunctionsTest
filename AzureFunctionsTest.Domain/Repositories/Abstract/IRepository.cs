using System.Collections.Generic;

namespace AzureFunctionsTest.Domain.Repositories.Abstract
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetByKey(TKey key);
        TEntity Add(TEntity entity);
        void Delete(TEntity entity);
    }
}
