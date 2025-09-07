using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Abstractions
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.CreateVersion7();
            IsActive = true;
        }
        public Guid Id { get; private set; }
        public bool IsActive { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public Guid CreatedBy { get; private set; } = default!;
        public DateTimeOffset? UpdatedAt { get; private set; }
        public Guid? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }
        public Guid? DeletedBy { get; private set; }

        public void SetStatus(bool isActive) => IsActive = isActive;
        public void Delete() => IsDeleted = true;

    }

}
