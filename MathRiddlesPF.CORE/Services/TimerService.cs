// Services/TimerService.cs
using MathRiddlesPF.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathRiddlesPF.CORE.Services
{
    public class TimerService : ITimerService
    {
        public int GetTimeLimitForDifficulty(int difficulty)
        {
            return difficulty switch
            {
                1 => 60,  // Easy: 60 segundos
                2 => 45,  // Medium: 45 segundos
                3 => 30,  // Hard: 30 segundos
                _ => 60
            };
        }

        public bool IsTimeValid(int timeRemaining, int originalTimeLimit)
        {
            return timeRemaining >= 0 && timeRemaining <= originalTimeLimit;
        }

        public int CalculateTimeBonus(int timeRemaining)
        {
            return Math.Max(0, timeRemaining * 2);
        }
    }
}