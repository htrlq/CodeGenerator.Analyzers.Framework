using Entity.Abstact;
using System;
using System.Collections.Generic;

namespace SourceEntity
{
    public class Class : IEntity<KeyEntity>
    {
        public KeyEntity Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreateTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime LastTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class KeyEntity
    {

    }

    public class Classx : IEntityRespository<Class, KeyEntity>
    {
        public void Add(Class entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Class> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<Class> entities)
        {
            throw new NotImplementedException();
        }

        public void Delte(Class entity)
        {
            throw new NotImplementedException();
        }

        public Class GetById(KeyEntity id)
        {
            throw new NotImplementedException();
        }

        public void Update(Class entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<Class> entities)
        {
            throw new NotImplementedException();
        }
    }
}
