namespace Nemesis.ViewModels
{
    public class ReportDetailsViewModel
    {
        public int ReportId { get; set; }
        public DateTime DateOfReport { get; set; }
        public string HazardLocation { get; set; }
        public string TypeOfHazard { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // Open, Being Investigated, etc.
        public string ReporterName { get; set; }
        public int Upvotes { get; set; }

        // Properties related to the investigation of this report
        public InvestigationViewModel Investigation { get; set; }

        public ReportDetailsViewModel()
        {
            Investigation = new InvestigationViewModel(); // Ensure there's always an investigation object to prevent null references
        }
    }
}
