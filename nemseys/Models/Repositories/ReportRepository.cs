using System;
using Nemesis.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Nemesis.Models.Interfaces;

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

        public Report GetReportByID(int reportId)
        {
            return _appDbcontext.Reports.Include(b => b.Category).FirstOrDefault(p => p.ReportId == reportId);
        }

        public Report GetReportById(int reportId)
        {
            return _appDbcontext.Reports.Find(reportId);
        }

        public void CreateReport(Report report)
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
        public IEnumerable<Category> GetAllCategories()
        {
            return _appDbcontext.Categories;
        }
        public Category GetCategoryByID(int categoryId)
        {
            return _appDbcontext.Categories.FirstOrDefault(c => c.Id == categoryId);
        }
    }
}

