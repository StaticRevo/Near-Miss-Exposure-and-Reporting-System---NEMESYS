using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nemesys.Models
{
    public class Investigation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestigationId { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        [MaxLength(255)]
        public string InvestigationTitle { get; set; } // Title of the investigation

        [Required]
        public string Feedback { get; set; } // Description of the investigation

        [Required]
        public DateTime DateOfAction { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } // Current status of the investigation

        [Required]
        [MaxLength(50)]
        public string Outcome { get; set; } // Outcome of the investigation

        // Properties from Report
        [NotMapped]
        public DateTime DateOfReport { get; set; }
        [NotMapped]
        public string HazardLocation { get; set; }
        [NotMapped]
        public DateTime DateAndTimeSpotted { get; set; }
        [NotMapped]
        public string TypeOfHazard { get; set; }
        [NotMapped]
        public string TitleOfReport { get; set; }
        [NotMapped]
        public string ReportDescription { get; set; }
        [NotMapped]
        public string ReportStatus { get; set; }
        [NotMapped]
        public string ImageUrl { get; set; }

        //Foreign Key - Navigation property
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Report Report { get; set; } // Assuming you have a Report entity
    }
}
