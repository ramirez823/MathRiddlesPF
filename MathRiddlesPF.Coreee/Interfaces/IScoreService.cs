using MathRiddlesPF.CORE.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MathRiddlesPF.CORE.Interfaces
{
    public interface IScoreService
    {
        Task<int> CalculateScoreAsync(bool isCorrect, int timeRemaining, int difficulty);
        Task<ScoreDto> GetUserScoreAsync(string sessionId);
        Task AddScoreAsync(string sessionId, int points);
        string GetIQCategory(int totalPoints);
        Task<int> GetProgressPercentAsync(string sessionId);
    }
}
