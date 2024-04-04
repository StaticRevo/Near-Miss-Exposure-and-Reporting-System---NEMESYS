using System;
using Nemesis.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Nemesis.Interfaces;

namespace Nemesis.Models.Repositories
{
	public class ReportRepository: IReportRepository
    {
        private readonly AppDbContext _appDbcontext;

        public ReportRepository(AppDbContext appDbContext)
		{
			_appDbcontext = appDbContext;
		}

        public IEnumerable<Report> GetAllReports()
        {
            return _appDbcontext.Reports.ToList();
        }

        public Report GetReportById(int reportId)
        {
            return _appDbcontext.Reports.Find(reportId);
        }

        public void AddReport(Report report)
        {
            _appDbcontext.Reports.Add(report);
            _appDbcontext.SaveChanges();
        }

        public void UpdateReport(Report report)
        {
            _appDbcontext.Entry(report).State = EntityState.Modified;
            _appDbcontext.SaveChanges();
        }

        public void DeleteReport(int reportId)
        {
            var report = _appDbcontext.Reports.Find(reportId);
            if (report != null)
            {
                _appDbcontext.Reports.Remove(report);
                _appDbcontext.SaveChanges();
            }
        }
    }
}

