using GenericRepository;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Repositories
{
    internal sealed class LoginTokenRepository : Repository<LoginToken, ApplicationDbContext>, ILoginTokenRepository
    {
        public LoginTokenRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
