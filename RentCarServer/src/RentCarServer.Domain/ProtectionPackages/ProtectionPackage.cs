using RentCarServer.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.ProtectionPackages
{
    public sealed class ProtectionPackage : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public bool IsRecommended { get; private set; }
        private readonly List<ProtectionCoverage> _coverages = new();

        private ProtectionPackage() { }

        public ProtectionPackage(string name, decimal price, bool isRecommended, IEnumerable<ProtectionCoverage> coverages, bool isActive)
        {
            SetName(name);
            SetPrice(price);
            SetIsRecommended(isRecommended);
            SetCoverages(coverages);
            SetStatus(isActive);
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetPrice(decimal price)
        {
            Price = price;
        }

        public void SetIsRecommended(bool isRecommended)
        {
            IsRecommended = isRecommended;
        }
        public void SetCoverages(IEnumerable<ProtectionCoverage> coverages)
        {
            _coverages.Clear();
            _coverages.AddRange(coverages);
        }

        public IReadOnlyCollection<ProtectionCoverage> Coverages => _coverages;
    }
}
