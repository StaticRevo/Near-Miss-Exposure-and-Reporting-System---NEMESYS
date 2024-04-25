using Nemesys.Models.Contexts;
using Nemesys.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models.Contexts;
using System.Composition;

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


        // Investigation Section
        public IEnumerable<Investigation> GetAllInvestigations()
        {
            return _appDbContext.Investigations.OrderBy(investigation => investigation.DateOfAction);
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            return _appDbContext.Investigations.FirstOrDefault(p => p.InvestigationId == investigationId);
        }

        public void CreateInvestigation(Investigation investigation)
        {
            _appDbContext.Investigations.Add(investigation);
            _appDbContext.SaveChanges();
        }
        public void UpdateInvestigation(Investigation investigation)
        {
            var existingInvestigation = _appDbContext.Investigations.SingleOrDefault(i => i.InvestigationId == investigation.InvestigationId);
            if (existingInvestigation != null)
            {
                // Update properties
                existingInvestigation.InvestigationTitle = investigation.InvestigationTitle;
                existingInvestigation.Feedback = investigation.Feedback;
                existingInvestigation.DateOfAction = investigation.DateOfAction;
                existingInvestigation.Status = investigation.Status;
                existingInvestigation.Outcome = investigation.Outcome;
                existingInvestigation.UserId = investigation.UserId;

                _appDbContext.Entry(existingInvestigation).State = EntityState.Modified;
                _appDbContext.SaveChanges();
            }
        }
        public void DeleteInvestigation(Investigation investigation)
        {
            _appDbContext.Investigations.Remove(investigation);
            _appDbContext.SaveChanges();
        }


    }
}
