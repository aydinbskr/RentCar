﻿using RentCarServer.Domain.Vehicles;
using RentCarServer.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Repositories
{
    internal sealed class VehicleRepository : AuditableRepository<Vehicle, ApplicationDbContext>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
