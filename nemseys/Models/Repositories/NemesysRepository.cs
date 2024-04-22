using Nemesys.Models.Contexts;
using Nemesys.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models.Contexts;

namespace Nemesys.Models.Repositories
{
    public class NemesysRepository : INemeseysRepository
    {
        private readonly AppDbContext _appDbContext;

        public NemesysRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Report> GetAllReports()
        {
            return _appDbContext.Reports.OrderBy(report => report.DateOfReport);
        }
        public Report GetReportById(int reportId)
        {
            return _appDbContext.Reports.FirstOrDefault(p => p.ReportId == reportId);
        }
        public void CreateReport(Report report)
        {
            _appDbContext.Reports.Add(report);
            _appDbContext.SaveChanges();
        }
        public void UpdateReport(Report report)
        {
            var existingReport = _appDbContext.Reports.SingleOrDefault(bp => bp.ReportId == report.ReportId);
            if (existingReport != null)
            {
                // Update properties
                existingReport.DateOfReport = report.DateOfReport;
                existingReport.HazardLocation = report.HazardLocation;
                existingReport.DateAndTimeSpotted = report.DateAndTimeSpotted;
                existingReport.TypeOfHazard = report.TypeOfHazard;
                existingReport.TitleOfReport = report.TitleOfReport;
                existingReport.Description = report.Description;
                existingReport.Status = report.Status;
                existingReport.ImageUrl = report.ImageUrl;
                existingReport.Upvotes = report.Upvotes;

                _appDbContext.Entry(existingReport).State = EntityState.Modified;
                _appDbContext.SaveChanges();
            }
        }
        public void DeleteReport(Report report)
        {
            _appDbContext.Reports.Remove(report);
            _appDbContext.SaveChanges();
        }

    }
}
