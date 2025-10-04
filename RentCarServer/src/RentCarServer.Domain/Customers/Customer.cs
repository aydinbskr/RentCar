using RentCarServer.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Customers
{
    public sealed class Customer : Entity
    {
        private Customer() { }

        public Customer(
            string firstName,
            string lastName,
            string identityNumber,
            DateOnly dateOfBirth,
            string phoneNumber,
            string email,
            DateOnly drivingLicenseIssuanceDate,
            string fullAddress,
            bool isActive)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetFullName();
            SetIdentityNumber(identityNumber);
            SetDateOfBirth(dateOfBirth);
            SetPhoneNumber(phoneNumber);
            SetEmail(email);
            SetDrivingLicenseIssuanceDate(drivingLicenseIssuanceDate);
            SetFullAddress(fullAddress);
            SetStatus(isActive);
        }

        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string FullName { get; private set; } = default!;
        public string IdentityNumber { get; private set; } = default!;
        public DateOnly DateOfBirth { get; private set; } = default!;
        public string PhoneNumber { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public DateOnly DrivingLicenseIssuanceDate { get; private set; } = default!;
        public string FullAddress { get; private set; } = default!;

        public void SetFirstName(string firstName) => FirstName = firstName;
        public void SetLastName(string lastName) => LastName = lastName;
        public void SetFullName() => FullName = new(string.Join(" ", FirstName, LastName));
        public void SetIdentityNumber(string identityNumber) => IdentityNumber = identityNumber;
        public void SetDateOfBirth(DateOnly dateOfBirth) => DateOfBirth = dateOfBirth;
        public void SetPhoneNumber(string phoneNumber) => PhoneNumber = phoneNumber;
        public void SetEmail(string email) => Email = email;
        public void SetDrivingLicenseIssuanceDate(DateOnly date) => DrivingLicenseIssuanceDate = date;
        public void SetFullAddress(string fullAddress) => FullAddress = fullAddress;
    }
}
