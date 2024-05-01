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
        private readonly ILogger<NemesysRepository> _logger;


        public NemesysRepository(AppDbContext appDbContext, ILogger<NemesysRepository> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }
        public IEnumerable<Report> GetAllReports()
        {
            try
            {
                return _appDbContext.Reports.OrderBy(report => report.DateOfReport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            
        }
        public Report GetReportById(int reportId)
        {
            try
            {
                return _appDbContext.Reports.FirstOrDefault(p => p.ReportId == reportId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
        public void CreateReport(Report report)
        {
            try
            {
                _appDbContext.Reports.Add(report);
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }    
        }
        public void UpdateReport(Report report)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            
        }
        public void DeleteReport(Report report)
        {
            try
            {
                _appDbContext.Reports.Remove(report);
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }    
        }


        // Investigation Section
        public IEnumerable<Investigation> GetAllInvestigations()
        {
            try
            {
                return _appDbContext.Investigations.OrderBy(investigation => investigation.DateOfAction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            try
            {
                return _appDbContext.Investigations.FirstOrDefault(p => p.InvestigationId == investigationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            
        }

        public void CreateInvestigation(Investigation investigation)
        {
            try
            {
                _appDbContext.Investigations.Add(investigation);
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            
        }

        public void UpdateInvestigation(Investigation investigation)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            
        }
        public void DeleteInvestigation(Investigation investigation)
        {
            try
            {
                _appDbContext.Investigations.Remove(investigation);
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            
        }


    }
}
