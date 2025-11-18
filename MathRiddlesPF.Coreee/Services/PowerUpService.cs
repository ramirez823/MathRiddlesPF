
using MathRiddlesPF.DATA.Entities;
using MathRiddlesPF.DATA.Repositories;
using MathRiddlesPF.CORE.DTOs;
using MathRiddlesPF.CORE.Enums;
using MathRiddlesPF.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MathRiddlesPF.Services
{
    public class PowerUpService : IPowerUpService
    {
        private readonly IPowerUpRepository _powerUpRepository;
        private readonly IRiddleRepository _riddleRepository;

        public PowerUpService(IPowerUpRepository powerUpRepository, IRiddleRepository riddleRepository)
        {
            _powerUpRepository = powerUpRepository;
            _riddleRepository = riddleRepository;
        }

        public async Task<List<PowerUpType>> GetAvailablePowerUpsAsync(string sessionId)
        {
            var powerUps = await _powerUpRepository.GetBySessionIdAsync(sessionId);

            if (!powerUps.Any())
            {
                // Crear power-ups iniciales para nueva sesión
                await InitializePowerUpsAsync(sessionId);
                powerUps = await _powerUpRepository.GetBySessionIdAsync(sessionId);
            }

            return powerUps
                .Where(p => !p.IsUsed)
                .Select(p => Enum.Parse<PowerUpType>(p.Type))
                .ToList();
        }

        public async Task<PowerUpResultDto> UsePowerUpAsync(string sessionId, PowerUpType powerUpType, int riddleId)
        {
            var powerUp = await _powerUpRepository.GetByTypeAsync(sessionId, powerUpType.ToString());

            if (powerUp == null || powerUp.IsUsed)
            {
                return new PowerUpResultDto
                {
                    Success = false,
                    Message = "Power-up no disponible"
                };
            }

            var riddle = await _riddleRepository.GetByIdAsync(riddleId);
            if (riddle == null)
            {
                return new PowerUpResultDto
                {
                    Success = false,
                    Message = "Pregunta no encontrada"
                };
            }

            var result = powerUpType switch
            {
                PowerUpType.FiftyFifty => await ApplyFiftyFiftyAsync(riddle),
                PowerUpType.Hint => ApplyHint(riddle),
                PowerUpType.ExtraTime => ApplyExtraTime(),
                _ => new PowerUpResultDto { Success = false, Message = "Power-up inválido" }
            };

            if (result.Success)
            {
                powerUp.IsUsed = true;
                await _powerUpRepository.UpdateAsync(powerUp);
            }

            return result;
        }

        public async Task<bool> IsPowerUpAvailableAsync(string sessionId, PowerUpType powerUpType)
        {
            var powerUp = await _powerUpRepository.GetByTypeAsync(sessionId, powerUpType.ToString());
            return powerUp != null && !powerUp.IsUsed;
        }

        public async Task ResetPowerUpsAsync(string sessionId)
        {
            var powerUps = await _powerUpRepository.GetBySessionIdAsync(sessionId);
            foreach (var powerUp in powerUps)
            {
                powerUp.IsUsed = false;
                await _powerUpRepository.UpdateAsync(powerUp);
            }
        }

        private async Task<PowerUpResultDto> ApplyFiftyFiftyAsync(Riddle riddle)
        {
            if (string.IsNullOrEmpty(riddle.OptionsJson))
            {
                return new PowerUpResultDto
                {
                    Success = false,
                    Message = "Esta pregunta no tiene opciones múltiples"
                };
            }

            var options = JsonSerializer.Deserialize<List<string>>(riddle.OptionsJson);
            var incorrectOptions = options.Where(o => o != riddle.CorrectAnswer).ToList();

            if (incorrectOptions.Count < 2)
            {
                return new PowerUpResultDto
                {
                    Success = false,
                    Message = "No hay suficientes opciones para eliminar"
                };
            }

            // Eliminar 2 opciones incorrectas aleatoriamente
            var random = new Random();
            incorrectOptions.RemoveAt(random.Next(incorrectOptions.Count));
            incorrectOptions.RemoveAt(random.Next(incorrectOptions.Count));

            var remainingOptions = new List<string> { riddle.CorrectAnswer };
            remainingOptions.AddRange(incorrectOptions);

            return new PowerUpResultDto
            {
                Success = true,
                Message = "Se eliminaron 2 opciones incorrectas",
                RemainingOptions = remainingOptions
            };
        }

        private PowerUpResultDto ApplyHint(Riddle riddle)
        {
            return new PowerUpResultDto
            {
                Success = true,
                Message = "Pista revelada",
                Hint = $"La respuesta correcta empieza con: {riddle.CorrectAnswer[0]}"
            };
        }

        private PowerUpResultDto ApplyExtraTime()
        {
            return new PowerUpResultDto
            {
                Success = true,
                Message = "¡30 segundos extra agregados!",
                ExtraTimeAdded = 30
            };
        }

        private async Task InitializePowerUpsAsync(string sessionId)
        {
            var powerUpTypes = new[] { PowerUpType.FiftyFifty, PowerUpType.Hint, PowerUpType.ExtraTime };

            foreach (var type in powerUpTypes)
            {
                await _powerUpRepository.AddAsync(new PowerUp
                {
                    SessionId = sessionId,
                    Type = type.ToString(),
                    Name = GetPowerUpName(type),
                    IsUsed = false
                });
            }
        }

        private string GetPowerUpName(PowerUpType type)
        {
            return type switch
            {
                PowerUpType.FiftyFifty => "50/50",
                PowerUpType.Hint => "Pista",
                PowerUpType.ExtraTime => "Tiempo Extra",
                _ => "Desconocido"
            };
        }
    }
}