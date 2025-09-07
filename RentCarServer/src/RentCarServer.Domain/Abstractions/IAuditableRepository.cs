using GenericRepository;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Abstractions
{
    public interface IAuditableRepository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
    {
        IQueryable<EntityWithAuditDto<TEntity>> GetAllWithAudit();
    }

    public sealed class EntityWithAuditDto<TEntity>
        where TEntity : Entity
    {
        public TEntity Entity { get; set; } = default!;
        public User CreatedUser { get; set; } = default!;
        public User? UpdatedUser { get; set; }
    }
}
