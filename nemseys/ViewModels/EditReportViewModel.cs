using System;
using System.ComponentModel.DataAnnotations;

namespace Nemesis.ViewModels
{
    public class EditReportViewModel
    {
        public int ReportId { get; set; }

        [Required(ErrorMessage = "Date of report is required")]
        [Display(Name = "Date of Report")]
        public DateTime DateOfReport { get; set; }

        [Required(ErrorMessage = "Hazard location is required")]
        [StringLength(100)]
        [Display(Name = "Hazard Location")]
        public string HazardLocation { get; set; }

        [Required(ErrorMessage = "Date and time spotted is required")]
        [Display(Name = "Date and Time Spotted")]
        public DateTime DateAndTimeSpotted { get; set; }

        [Required(ErrorMessage = "Type of hazard is required")]
        [StringLength(50)]
        [Display(Name = "Type of Hazard")]
        public string TypeOfHazard { get; set; }

        [Required(ErrorMessage = "A title is required")]
        [StringLength(100)]
        [Display(Name = "Title of Report")]
        public string TitleOfReport { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(30)]
        [Display(Name = "Status")]
        public string Status { get; set; }

        public string? ImageUrl { get; set; }
        [Display(Name = "Featured Image")]
        public IFormFile ImageToUpload { get; set; }

        [Required(ErrorMessage = "Reporter email is required")]
        [EmailAddress]
        [Display(Name = "Reporter Email")]
        public string ReporterEmail { get; set; }

        [Display(Name = "Upvotes")]
        public int Upvotes { get; set; }

        [Display(Name = "Category")]
        public ReportCategoryViewModel Category { get; set; }
        public int CategoryId { get; set; }

        public List<ReportCategoryViewModel>? CategoryList { get; set; }

        public EditReportViewModel()
        {
        }
    }
}
