// Services/LevelService.cs

using MathRiddlesPF.DATA.Repositories;
using MathRiddlesPF.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Services
{
    public class LevelService : ILevelService
    {
        private readonly ILevelRepository _levelRepository;
        private readonly IUserProgressRepository _progressRepository;

        public LevelService(ILevelRepository levelRepository, IUserProgressRepository progressRepository)
        {
            _levelRepository = levelRepository;
            _progressRepository = progressRepository;
        }

        public async Task<List<LevelDto>> GetAllLevelsAsync()
        {
            var levels = await _levelRepository.GetAllAsync();
            return levels.Select(l => new LevelDto
            {
                Id = l.Id,
                Name = l.Name,
                Difficulty = l.Difficulty,
                IsUnlocked = l.Difficulty == 1, // Primer nivel siempre desbloqueado
                TotalRiddles = l.Riddles?.Count ?? 0
            }).ToList();
        }

        public async Task<LevelDto> GetLevelByIdAsync(int levelId)
        {
            var level = await _levelRepository.GetByIdAsync(levelId);
            if (level == null) return null;

            return new LevelDto
            {
                Id = level.Id,
                Name = level.Name,
                Difficulty = level.Difficulty,
                IsUnlocked = true,
                TotalRiddles = level.Riddles?.Count ?? 0
            };
        }

        public async Task<bool> IsLevelUnlockedAsync(string sessionId, int levelId)
        {
            var progress = await _progressRepository.GetBySessionIdAsync(sessionId);
            if (progress == null) return levelId == 1; // Solo primer nivel disponible

            return progress.CurrentLevelId >= levelId;
        }

        public async Task UnlockNextLevelAsync(string sessionId, int currentLevelId)
        {
            var progress = await _progressRepository.GetBySessionIdAsync(sessionId);

            if (progress == null)
            {
                progress = new Data.Entities.UserProgress
                {
                    SessionId = sessionId,
                    CurrentLevelId = currentLevelId + 1,
                    ProgressPercent = 10
                };
                await _progressRepository.AddAsync(progress);
            }
            else
            {
                progress.CurrentLevelId = Math.Max(progress.CurrentLevelId, currentLevelId + 1);
                await _progressRepository.UpdateAsync(progress);
            }
        }
    }
}