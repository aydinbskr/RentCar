using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Users
{
    public sealed class User:Entity
    {
        public User()
        {
        }

        public User(string firstName, string lastName, string userName ,string email, Password password, Guid branchId,Guid roleId, bool isActive)
        {
            FirstName = firstName;
            LastName = lastName;
            FullName = $"{FirstName} {LastName} ({Email})";
            UserName = userName;
            Email = email;
            Password = password;
            IsForgotPasswordCompleted = true;
            SetTFAStatus(false);
            SetBranchId(branchId);
            SetRoleId(roleId);
            SetStatus(isActive);
        }



        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public Password Password { get; set; } = default!;

        public Guid? ForgotPasswordCode { get; private set; }
        public DateTimeOffset? ForgotPasswordDate { get; private set; }
        public bool IsForgotPasswordCompleted { get; private set; }
        public bool TFAStatus { get; private set; }
        public string? TFACode { get; private set; } = default!;
        public string? TFAConfirmCode { get; private set; } = default!;
        public DateTimeOffset? TFAExpiresDate { get; private set; }
        public bool? TFAIsCompleted { get; private set; }
        public Guid BranchId { get; private set; } = default!;
        public Guid RoleId { get; private set; } = default!;


        public bool VerifyPasswordHash(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(Password.PasswordHash);
        }

        public void CreateForgotPasswordId()
        {
            ForgotPasswordCode = Guid.CreateVersion7();
            ForgotPasswordDate = DateTimeOffset.Now;
            IsForgotPasswordCompleted = false;
        }

        public void SetPassword(Password password)
        {
            Password = password;
        }

        public void SetTFAStatus(bool tfaStatus)
        {
            TFAStatus = tfaStatus;
        }

        public void CreateTFACode()
        {
            //Random rnd = new Random(Guid.NewGuid().GetHashCode());
            //int code = rnd.Next(100000, 1000000);

            //TFACode = code.ToString();
            TFACode = Guid.CreateVersion7().ToString();
            TFAConfirmCode = Guid.CreateVersion7().ToString();
            TFAExpiresDate = DateTimeOffset.Now.AddMinutes(5);
            TFAIsCompleted = false;

        }

        public void SetFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            LastName = lastName;
        }

        public void SetEmail(string email)
        {
            Email = email;
        }

        public void SetUserName(string userName)
        {
            UserName = userName;
        }

        public void SetFullName()
        {
            FullName = new(FirstName + " " + LastName + " (" + Email + ")");
        }

        public void SetBranchId(Guid branchId)
        {
            BranchId = branchId;
        }

        public void SetRoleId(Guid roleId)
        {
            RoleId = roleId;
        }

        public void SetTFACompleted()
        {
            TFAIsCompleted = true;
        }

    }

    public sealed record Password
    {
        private Password()
        {
        }
        public Password(string password)
        {
            CreatePasswordHash(password);
        }
        public byte[] PasswordHash { get; private set; } = default!;
        public byte[] PasswordSalt { get; private set; } = default!;

        private void CreatePasswordHash(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            PasswordSalt = hmac.Key;
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
