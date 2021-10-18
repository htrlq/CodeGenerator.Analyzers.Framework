using System.Collections.Generic;

namespace Entity.Abstact
{
    public interface IEntityBatchRespository<TEntity, TId>: IEntityRespository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
        void AddRange(IEnumerable<TEntity> entities);
        void UpdateRange(IEnumerable<TEntity> entities);
        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
