using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Models
{
    /// <summary>
    /// IMPORTANT: when editing the model, make sure to edit the model diagram in DBModel project first
    /// </summary>
    public class City : GeoBaseData
    {
        public uint Population { get; set; }

        [Required]
        public Country Country { get; set; }
    }
}
