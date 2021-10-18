using System;
using System.Collections.Generic;
using Entity.Abstact;
using SourceEntity;

namespace SourceEntity.CodeGenerator
{
    public class ClassBatchRespositor : IEntityBatchRespository<Class, SourceEntity.KeyEntity>
    {
        public void Add(Class entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Class> entities)
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

        public void Delete(Class entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<Class> entities)
        {
            throw new NotImplementedException();
        }

        public Class GetById(SourceEntity.KeyEntity id)
        {
            throw new NotImplementedException();
        }
    }
}