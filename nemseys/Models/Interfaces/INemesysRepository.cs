namespace Nemesys.Models.Interfaces
{
    public interface INemeseysRepository
    {
        IEnumerable<Report> GetAllReports();
        Report GetReportById(int reportId);
        void CreateReport(Report report);
        void UpdateReport(Report updatedReport);
        void DeleteReport(Report report);

        IEnumerable<Investigation> GetAllInvestigations();
        Investigation GetInvestigationById(int investigationId);
        void CreateInvestigation(Investigation investigation);
        void UpdateInvestigation(Investigation updatedInvestigation);
        void DeleteInvestigation(Investigation investigation);

    }
}
