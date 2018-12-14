using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TripPlanner.Options
{
    public class DatabaseUpdateOptions
    {
        public string SeedDataPath { get; set; }
        public string GeoDataFile { get; set; }
        public string CountryDataFile { get; set; }
        public string FeatureCodeFile { get; set; }
    }
}
