using RentCarServer.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RentCarServer.Domain.Roles
{
    public sealed class Role : Entity, IAggregate
    {
        private readonly List<Permission> _permissions = new();
        public string Name { get; private set; } = default!;
        public IReadOnlyCollection<Permission> Permissions => _permissions;

        private Role() { }

        public Role(string name, bool isActive)
        {
            SetName(name);
            SetStatus(isActive);
        }

        public void SetName(string name)
        {
            Name = name;
        }
        public void SetPermissions(IEnumerable<Permission> permissions)
        {
            _permissions.Clear();
            _permissions.AddRange(permissions);
        }
    }

    public sealed record Permission(string Value);
}
