
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nemesys.Models
{
    public class Report
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


        //Foreign Key - Navigation property
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
