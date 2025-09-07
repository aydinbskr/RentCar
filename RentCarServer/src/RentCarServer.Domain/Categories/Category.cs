using RentCarServer.Domain.Abstractions;

namespace RentCarServer.Domain.Categories
{
    public sealed class Category : Entity
    {
        public string Name { get; private set; } = string.Empty;

        private Category() { }

        public Category(string name, bool isActive)
        {
            SetName(name);
            SetStatus(isActive);
        }

        public void SetName(string name)
        {
            Name = name;
        }
    }
}