using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application
{
    internal static class ExtensionMethods
    {
        public static IQueryable<EntityWithAuditDto<TEntity>> ApplyAuditDto<TEntity>(this IQueryable<TEntity> entities, IQueryable<User> users)
            where TEntity : Entity
        {
            var res = entities.AsNoTracking()
               .Join(users, m => m.CreatedBy, m => m.Id, (b, user) =>
                       new { entity = b, createdUser = user })
               .GroupJoin(users, m => m.entity.UpdatedBy, m => m.Id, (b, user) =>
                       new { b.entity, b.createdUser, updatedUser = user })
               .SelectMany(s => s.updatedUser.DefaultIfEmpty(),
                   (x, updatedUser) => new EntityWithAuditDto<TEntity>
                   {
                       Entity = x.entity,
                       CreatedUser = x.createdUser,
                       UpdatedUser = updatedUser
                   });

            return res;
        }
    }
}
