using System;
namespace Nemesis.ViewModels
{
	public class InvestigationViewModel
	{
        public int InvestigationId { get; set; }
        public string Description { get; set; }
        public DateTime DateOfAction { get; set; }
        public int InvestigatorId { get; set; }
        public string InvestigatorName { get; set; } 
        public int ReportId { get; set; }
        public string ReportTitle { get; set; } 

        public InvestigationViewModel()
        {
            // For adding and viewing investigations
        }

    }
}

