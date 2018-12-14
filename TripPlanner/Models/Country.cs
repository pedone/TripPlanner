using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.Models
{
    /// <summary>
    /// IMPORTANT: when editing the model, make sure to edit the model diagram in DBModel project first
    /// </summary>
    public class Country
    {
        [Required]
        public string Name { get; set; }

        [Key]
        [Required]
        [StringLength(2, MinimumLength = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CountryCode { get; set; }
    }
}
