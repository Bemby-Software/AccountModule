using System;

namespace Bemby.AccountModule.Domain.Entities
{
    public interface IEntityBase
    {
        public Guid Id { get; }
    }
}