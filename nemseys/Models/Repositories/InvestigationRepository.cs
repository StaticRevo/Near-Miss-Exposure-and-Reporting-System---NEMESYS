using System;
using Nemesis.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Nemesis.Interfaces;

namespace Nemesis.Models.Repositories
{
    public class InvestigationRepository : IInvestigationRepository
    {
        private readonly AppDbContext _appDbcontext;

        public InvestigationRepository(AppDbContext context)
        {
            _appDbcontext = context;
        }

        public IEnumerable<Investigation> GetAllInvestigations()
        {
            return _appDbcontext.Investigations.ToList();
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            return _appDbcontext.Investigations.Find(investigationId);
        }

        public void AddInvestigation(Investigation investigation, int reportId)
        {
            investigation.ReportId = reportId; 
            _appDbcontext.Investigations.Add(investigation);
            _appDbcontext.SaveChanges();
        }

        public void UpdateInvestigation(Investigation investigation)
        {
            _appDbcontext.Entry(investigation).State = EntityState.Modified;
            _appDbcontext.SaveChanges();
        }

        public void DeleteInvestigation(int investigationId)
        {
            var investigation = _appDbcontext.Investigations.Find(investigationId);
            if (investigation != null)
            {
                _appDbcontext.Investigations.Remove(investigation);
                _appDbcontext.SaveChanges();
            }
        }
    }
}
