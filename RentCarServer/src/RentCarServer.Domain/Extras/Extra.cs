using RentCarServer.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RentCarServer.Domain.Extras
{
    public sealed class Extra : Entity
    {
        private Extra() { }

        public Extra(string name, decimal price, string description, bool isActive)
        {
            SetName(name);
            SetPrice(price);
            SetDescription(description);
            SetStatus(isActive);
        }

        public string Name { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;
        public string Description { get; private set; } = default!;

        public void SetName(string name) => Name = name;
        public void SetPrice(decimal price) => Price = price;
        public void SetDescription(string description) => Description = description;
    }
}
