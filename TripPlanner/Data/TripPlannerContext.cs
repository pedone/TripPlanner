using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TripPlanner.Models
{
    public class TripPlannerContext : DbContext
    {
        public TripPlannerContext(DbContextOptions<TripPlannerContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<TimeZone> TimeZones { get; set; }
        public DbSet<GeoData> GeoData { get; set; }
        public DbSet<FeatureCode> FeatureCodes { get; set; }
        public DbSet<FeatureCategory> FeatureCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var alternateNamesConverter = new ValueConverter<string[], string>(
                v => string.Join(",", v),
                v => v.Split(new char[] { ',' }));

            modelBuilder.Entity<GeoData>()
                .Property(e => e.AlternateNames)
                .HasConversion(alternateNamesConverter);
        }
    }
}
