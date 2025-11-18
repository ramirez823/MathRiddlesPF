using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathRiddlesPF.CORE.Interfaces
{
    public interface IProgressService
    {
        Task<int> GetProgressPercentAsync(string sessionId);
        Task UpdateProgressAsync(string sessionId, int currentLevelId, int riddlesCompleted);
        Task<bool> ShouldShowCongratsPopupAsync(string sessionId);
        Task MarkCongratsShownAsync(string sessionId, int progressMilestone);
    }
}
