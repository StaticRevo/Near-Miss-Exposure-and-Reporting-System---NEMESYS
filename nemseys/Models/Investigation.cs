using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nemesis.Models
{
    public class Investigation
    {
        [Key]
        public int InvestigationId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateOfAction { get; set; }

        // Foreign key
        [ForeignKey("Investigator")]
        public int InvestigatorId { get; set; }

        // Navigation properties
        public virtual Profile Investigator { get; set; }

        [ForeignKey("Report")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }
    }
}

