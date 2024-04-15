namespace Nemesis.ViewModels
{
    public class ReportListViewModel
    {
        public int TotalEntries { get; set; }
        public IEnumerable<ReportViewModel> Reports { get; set; }

        public ReportListViewModel()
        {
            
        }
    }
}
