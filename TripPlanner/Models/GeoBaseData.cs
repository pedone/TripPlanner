using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TripPlanner.Models
{
    /// <summary>
    /// IMPORTANT: when editing the model, make sure to edit the model diagram in DBModel project first
    /// </summary>
    public class GeoBaseData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        //public string[] AlternateNames { get; set; }
        [StringLength(2, MinimumLength = 2)]
        public string CountryCode { get; set; }
        public float Longitude { get; set; }
        public float Lattitude { get; set; }
        public ICollection<TimeZone> TimeZones { get; set; }
    }
}
