using System.Collections.Generic;
using System.Threading.Tasks;
using Gearment.Summary.Models;

namespace Gearment.Summary.Services
{
    public interface ISummaryService 
    {
        Task<List<Models.Summary>> GetSummarysAsync(int ModuleId);

        Task<Models.Summary> GetSummaryAsync(int SummaryId, int ModuleId);

        Task<Models.Summary> AddSummaryAsync(Models.Summary Summary);

        Task<Models.Summary> UpdateSummaryAsync(Models.Summary Summary);

        Task DeleteSummaryAsync(int SummaryId, int ModuleId);
    }
}
