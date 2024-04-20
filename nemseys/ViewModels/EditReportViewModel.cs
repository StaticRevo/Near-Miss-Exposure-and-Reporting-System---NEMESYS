using System.ComponentModel.DataAnnotations;

namespace Nemesys.ViewModels
{
    public class EditReportViewModel
    {
        public int ReportId { get; set; }

        [Required(ErrorMessage = "Date of report is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date of Report")]
        public DateTime DateOfReport { get; set; }

        [Required(ErrorMessage = "Hazard location is required")]
        [StringLength(255)]
        [Display(Name = "Hazard Location")]
        public string HazardLocation { get; set; }

        [Required(ErrorMessage = "Date and time spotted is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date and Time Spotted")]
        public DateTime DateAndTimeSpotted { get; set; }

        [Required(ErrorMessage = "Type of hazard is required")]
        [StringLength(50)]
        [Display(Name = "Type of Hazard")]
        public string TypeOfHazard { get; set; }

        [StringLength(255)]
        [Display(Name = "Title of Report")]
        public string TitleOfReport { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(int.MaxValue, MinimumLength = 20, ErrorMessage = "Description must be at least 20 characters long")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50)]
        [Display(Name = "Status")]
        public string Status { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Upload Image")]
        public IFormFile? ImageToUpload { get; set; }

        [Display(Name = "Upvotes")]
        public int Upvotes { get; set; }

    }
}
