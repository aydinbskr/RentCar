using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Reservations
{
    public sealed record PaymentInformation(
    string CartNumber,
    string Owner);
}
