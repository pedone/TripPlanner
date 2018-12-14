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
    public class FeatureCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public FeatureCategory FeatureCategory { get; set; }

        public ICollection<GeoData> GeoData { get; set; }
    }
}
