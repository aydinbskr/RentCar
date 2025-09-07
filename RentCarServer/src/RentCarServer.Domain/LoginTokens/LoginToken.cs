using RentCarServer.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.LoginTokens
{
    public sealed class LoginToken
    {
        private LoginToken() { }
        public LoginToken(
            string token,
            Guid userId,
            DateTimeOffset expiresDate)
        {
            Id = Guid.CreateVersion7();
            SetToken(token);
            SetUserId(userId);
            SetIsActive(true);
            SetExpiresDate(expiresDate);
        }

        public Guid Id { get; private set; } = default!;
        public string Token { get; private set; } = default!;
        public Guid UserId { get; private set; } = default!;
        public bool IsActive { get; private set; } = default!;
        public DateTimeOffset ExpiresDate { get; private set; } = default!;

        #region Behaviors
        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetToken(string token)
        {
            Token = token;
        }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }

        public void SetExpiresDate(DateTimeOffset expiresDate)
        {
            ExpiresDate = expiresDate;
        }
        #endregion
    }
}
