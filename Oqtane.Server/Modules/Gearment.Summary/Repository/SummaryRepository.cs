using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Gearment.Summary.Models;

namespace Gearment.Summary.Repository
{
    public class SummaryRepository : ISummaryRepository, IService
    {
        private readonly SummaryContext _db;

        public SummaryRepository(SummaryContext context)
        {
            _db = context;
        }

        public IEnumerable<Models.Summary> GetSummarys(int ModuleId)
        {
            return _db.Summary.Where(item => item.ModuleId == ModuleId);
        }

        public Models.Summary GetSummary(int SummaryId)
        {
            return _db.Summary.Find(SummaryId);
        }

        public Models.Summary AddSummary(Models.Summary Summary)
        {
            _db.Summary.Add(Summary);
            _db.SaveChanges();
            return Summary;
        }

        public Models.Summary UpdateSummary(Models.Summary Summary)
        {
            _db.Entry(Summary).State = EntityState.Modified;
            _db.SaveChanges();
            return Summary;
        }

        public void DeleteSummary(int SummaryId)
        {
            Models.Summary Summary = _db.Summary.Find(SummaryId);
            _db.Summary.Remove(Summary);
            _db.SaveChanges();
        }
    }
}
