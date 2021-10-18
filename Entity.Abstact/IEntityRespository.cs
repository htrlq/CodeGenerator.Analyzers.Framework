namespace Entity.Abstact
{
    public interface IEntityRespository<TEntity,TId>
        where TEntity:class, IEntity<TId>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delte(TEntity entity);
        TEntity GetById(TId id);
    }
}
