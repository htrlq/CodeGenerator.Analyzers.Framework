using Entity.Abstact;
using System;
using System.Collections.Generic;

namespace SourceEntity
{
    public class Class1 : IEntity<KeyEntity>
    {
        public KeyEntity Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreateTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime LastTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class KeyEntity
    {

    }

    public class ClassR : IEntityRespository<Class1, KeyEntity>
    {
        public void Add(Class1 entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Class1> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<Class1> entities)
        {
            throw new NotImplementedException();
        }

        public void Delte(Class1 entity)
        {
            throw new NotImplementedException();
        }

        public Class1 GetById(KeyEntity id)
        {
            throw new NotImplementedException();
        }

        public void Update(Class1 entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<Class1> entities)
        {
            throw new NotImplementedException();
        }
    }
}
