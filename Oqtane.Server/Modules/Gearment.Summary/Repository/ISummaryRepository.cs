using System.Collections.Generic;
using Gearment.Summary.Models;

namespace Gearment.Summary.Repository
{
    public interface ISummaryRepository
    {
        IEnumerable<Models.Summary> GetSummarys(int ModuleId);
        Models.Summary GetSummary(int SummaryId);
        Models.Summary AddSummary(Models.Summary Summary);
        Models.Summary UpdateSummary(Models.Summary Summary);
        void DeleteSummary(int SummaryId);
    }
}
