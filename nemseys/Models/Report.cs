
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

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        // Optional: For storing the path or URL to the photo
        public string PhotoPath { get; set; }

        public int Upvotes { get; set; }

        // Foreign key
        [ForeignKey("Reporter")]
        public int ReporterId { get; set; }

        // Navigation properties
        public virtual Profile Reporter { get; set; }
        public virtual Investigation Investigation { get; set; } // Assuming one-to-one relationship
    }

}
