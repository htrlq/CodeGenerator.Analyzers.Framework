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

}
