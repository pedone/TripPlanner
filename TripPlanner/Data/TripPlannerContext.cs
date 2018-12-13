using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TripPlanner.Models
{
    public class TripPlannerContext : DbContext
    {
        public TripPlannerContext (DbContextOptions<TripPlannerContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<TimeZone> TimeZones { get; set; }
    }
}
