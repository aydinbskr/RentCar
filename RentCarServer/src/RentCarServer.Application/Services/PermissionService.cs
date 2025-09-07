using RentCarServer.Application.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Services
{
    public sealed class PermissionService
    {
        public List<string> GetAll()
        {
            var permissions = new HashSet<string>();

            permissions.Add("dashboard:view");

            var assembly = Assembly.GetExecutingAssembly();

            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                var permissinAttr = type.GetCustomAttribute<PermissionAttribute>();

                if (permissinAttr is not null && !string.IsNullOrEmpty(permissinAttr.Permission))
                {
                    permissions.Add(permissinAttr.Permission);
                }
            }

            return permissions.ToList();
        }
    }
}
