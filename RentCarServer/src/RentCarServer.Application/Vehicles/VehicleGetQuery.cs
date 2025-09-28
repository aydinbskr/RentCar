using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Vehicles
{
    [Permission("vehicle:view")]
    public sealed record VehicleGetQuery(Guid Id) : IRequest<Result<VehicleDto>>;

    internal sealed class VehicleGetQueryHandler(
        IVehicleRepository vehicleRepository) : IRequestHandler<VehicleGetQuery, Result<VehicleDto>>
    {
        public async Task<Result<VehicleDto>> Handle(VehicleGetQuery request, CancellationToken cancellationToken)
        {
            var res = await vehicleRepository
                .GetAllWithAudit()
                .MapTo()
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (res is null)
                return Result<VehicleDto>.Failure("Araç bulunamadı");

            return res;
        }
    }
}
