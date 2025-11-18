using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathRiddlesPF.CORE.Interfaces
{
    public interface ITimerService
    {
        int GetTimeLimitForDifficulty(int difficulty);
        bool IsTimeValid(int timeRemaining, int originalTimeLimit);
        int CalculateTimeBonus(int timeRemaining);
    }

}
