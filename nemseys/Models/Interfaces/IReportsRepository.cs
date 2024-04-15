using System.Collections.Generic;
using Nemesis.Models;


namespace Nemesis.Models.Interfaces
{
    public interface IReportRepository
    {
        IEnumerable<Report> GetAllReports();
        Report GetReportByID(int reportId);
        void CreateReport(Report report);
        void UpdateReport(Report report);
        void DeleteReport(int reportId);
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryByID(int categoryId);
    }
}
