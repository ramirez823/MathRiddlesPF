using MathRiddlesPF.CORE.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MathRiddlesPF.CORE.Interfaces
{
    public interface IRiddleService
    {
        Task<RiddleDto> GetRiddleByIdAsync(int riddleId);
        Task<RiddleDto> GetRandomRiddleByLevelAsync(int levelId);
        Task<List<RiddleDto>> GetRiddlesByLevelAsync(int levelId);
        Task<AnswerResultDto> ValidateAnswerAsync(int riddleId, string userAnswer, int timeRemaining);
    }
}
