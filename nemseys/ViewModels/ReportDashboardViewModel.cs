using System.ComponentModel.DataAnnotations;

namespace Nemesys.ViewModels
{
    public class ReportDashboardViewModel
    {
        [Display(Name = "Total Blog Posts")]
        public int TotalEntries { get; set; }
        [Display(Name = "Total Registered Users")]
        public int TotalRegisteredUsers { get; set; }
    }

}
