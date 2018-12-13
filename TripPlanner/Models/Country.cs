using System.Collections.Generic;

namespace TripPlanner.Models
{
    /// <summary>
    /// IMPORTANT: when editing the model, make sure to edit the model diagram in DBModel project first
    /// </summary>
    public class Country : GeoBaseData
    {
        public ICollection<City> Cities { get; set; }
    }
}
