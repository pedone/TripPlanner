using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripPlanner.Models;

namespace TripPlanner.Interfaces
{
    public interface IDatabaseSeedData
    {
        void InitializeDatabase();
    }
}
