using System.Collections.Generic;
using Nemesis.Models;


namespace Nemesis.Interfaces
{
    public interface IReportRepository
    {
        IEnumerable<Report> GetAllReports();
        Report GetReportById(int reportId);
        void AddReport(Report report);
        void UpdateReport(Report report);
        void DeleteReport(int reportId);   
    }
}

