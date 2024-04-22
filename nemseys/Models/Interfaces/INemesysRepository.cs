namespace Nemesys.Models.Interfaces
{
    public interface INemeseysRepository
    {
        IEnumerable<Report> GetAllReports();
        Report GetReportById(int reportId);
        void CreateReport(Report report);
        void UpdateReport(Report updatedReport);
        void DeleteReport(Report report);
    }
}
