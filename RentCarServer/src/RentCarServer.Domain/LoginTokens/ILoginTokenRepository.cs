﻿using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.LoginTokens
{
    public interface ILoginTokenRepository : IRepository<LoginToken>
    {
    }
}
