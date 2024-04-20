using Bloggy.ViewModels;
using System;
namespace Nemesys.ViewModels
{
    public class ReportViewModel
    {
        public int ReportId { get; set; }

        public DateTime DateOfReport { get; set; }

        public string HazardLocation { get; set; }


        public DateTime DateAndTimeSpotted { get; set; }


        public string TypeOfHazard { get; set; }


        public string TitleOfReport { get; set; }


        public string Description { get; set; }


        public string Status { get; set; }

        public string ImageUrl { get; set; }

        public int Upvotes { get; set; }
        public AuthorViewModel Author { get; set; }
    }
}
