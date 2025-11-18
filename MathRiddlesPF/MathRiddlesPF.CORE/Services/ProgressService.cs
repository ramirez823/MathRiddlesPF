// Services/ProgressService.cs
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
    public class ProgressService : IProgressService
    {
        private readonly IUserProgressRepository _progressRepository;

        public ProgressService(IUserProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }

        public async Task<int> GetProgressPercentAsync(string sessionId)
        {
            var progress = await _progressRepository.GetBySessionIdAsync(sessionId);
            return progress?.ProgressPercent ?? 0;
        }

        public async Task UpdateProgressAsync(string sessionId, int currentLevelId, int riddlesCompleted)
        {
            var progress = await _progressRepository.GetBySessionIdAsync(sessionId);

            // Cálculo simple: cada nivel = 33%, cada pregunta suma
            int progressPercent = (currentLevelId - 1) * 33 + (riddlesCompleted * 3);
            progressPercent = Math.Min(progressPercent, 100);

            if (progress == null)
            {
                progress = new Data.Entities.UserProgress
                {
                    SessionId = sessionId,
                    CurrentLevelId = currentLevelId,
                    ProgressPercent = progressPercent
                };
                await _progressRepository.AddAsync(progress);
            }
            else
            {
                progress.ProgressPercent = progressPercent;
                progress.CurrentLevelId = currentLevelId;
                await _progressRepository.UpdateAsync(progress);
            }
        }

        public async Task<bool> ShouldShowCongratsPopupAsync(string sessionId)
        {
            var progress = await _progressRepository.GetBySessionIdAsync(sessionId);
            if (progress == null) return false;

            // Mostrar cada 10%
            return progress.ProgressPercent % 10 == 0 && progress.ProgressPercent > 0;
        }

        public async Task MarkCongratsShownAsync(string sessionId, int progressMilestone)
        {
            // Implementar si quieres trackear qué popups ya se mostraron
            await Task.CompletedTask;
        }
    }
}