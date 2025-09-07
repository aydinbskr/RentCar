using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Shared
{
    public sealed record Contact(
    string PhoneNumber1,
    string PhoneNumber2,
    string Email);
}
