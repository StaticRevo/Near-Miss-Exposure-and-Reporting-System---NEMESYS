using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nemesis.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        public DateTime DateOfReport { get; set; }

        [Required]
        [StringLength(255)]
        public string HazardLocation { get; set; }

        [Required]
        public DateTime DateAndTimeSpotted { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeOfHazard { get; set; }

        // This was missing in your initial model
        [StringLength(255)]
        public string TitleOfReport { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        // This was missing in your initial model
        public string ImageUrl { get; set; }

        public int Upvotes { get; set; }

        // Foreign key for the reporter
        [ForeignKey("Reporter")]
        public int ReporterId { get; set; }

        // Navigation properties
        public virtual Profile Reporter { get; set; }

        // Assuming one-to-one relationship with Investigation
        public virtual Investigation Investigation { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
