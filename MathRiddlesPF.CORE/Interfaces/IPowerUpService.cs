using MathRiddlesPF.CORE.DTOs;
using MathRiddlesPF.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathRiddlesPF.CORE.Interfaces
{
    public interface IPowerUpService
    {
        Task<List<PowerUpType>> GetAvailablePowerUpsAsync(string sessionId);
        Task<PowerUpResultDto> UsePowerUpAsync(string sessionId, PowerUpType powerUpType, int riddleId);
        Task<bool> IsPowerUpAvailableAsync(string sessionId, PowerUpType powerUpType);
        Task ResetPowerUpsAsync(string sessionId);
    }
}
