using System.Collections.Generic;
using Nemesis.Models;


namespace Nemesis.Interfaces
{
    public interface IInvestigationRepository
    {
        IEnumerable<Investigation> GetAllInvestigations();
        Investigation GetInvestigationById(int investigationId);
        void AddInvestigation(Investigation investigation, int reportId);
        void UpdateInvestigation(Investigation investigation);
        void DeleteInvestigation(int investigationId);
    }
}

