﻿using ParkingZoneApp.Data;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Repositories
{
    public class ParkingSlotsRepository : Repository<ParkingSlot>, IParkingSlotsRepository
    {
        public ParkingSlotsRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
