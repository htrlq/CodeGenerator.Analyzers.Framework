using System;

namespace Entity.Abstact
{
    public interface IEntity<Type>
    {
        Type Id { get; set; }
        DateTime CreateTime { get; set; }
        DateTime LastTime { get; set; }
    }
}
