using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RentCarServer.Domain.Branches
{
    public sealed class Branch : Entity
    {
        private Branch() { }
        public Branch(
            string name,
            Address address,
            Contact contact,
            bool isActive)
        {
            SetName(name);
            SetAddress(address);
            SetContact(contact);
            SetStatus(isActive);
        }
        public string Name { get; private set; } = default!;
        public Address Address { get; private set; } = default!;
        public Contact Contact { get; private set; } = default!;

        #region Behaviors
        public void SetName(string name)
        {
            Name = name;
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }
        public void SetContact(Contact contact)
        {
            Contact = contact;
        }
        #endregion
    }
}
