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
    public class GeoData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string[] AlternateNames { get; set; }

        public float Longitude { get; set; }

        public float Lattitude { get; set; }

        public long Population { get; set; }

        [Required]
        public TimeZone TimeZone { get; set; }

        [Required]
        public Country Country { get; set; }

        [Required]
        public FeatureCode FeatureCode { get; set; }
    }
}
