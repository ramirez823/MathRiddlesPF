using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathRiddlesPF.CORE.DTOs;
using MathRiddlesPF.CORE.Interfaces;
using MathRiddlesPF.DATA.Repositories;
using MathRiddlesPF.DATA.Entities;

namespace MathRiddlesPF.CORE.Services
{
    public class ScoreService : IScoreService
    {
        private readonly IUserScoreRepository _scoreRepository;
        private readonly IUserProgressRepository _progressRepository;

        public ScoreService(IUserScoreRepository scoreRepository, IUserProgressRepository progressRepository)
        {
            _scoreRepository = scoreRepository;
            _progressRepository = progressRepository;
        }

        public async Task<int> CalculateScoreAsync(bool isCorrect, int timeRemaining, int difficulty)
        {
            if (!isCorrect) return 0;

            int basePoints = difficulty * 50;
            int timeBonus = Math.Max(0, timeRemaining * 2);

            return basePoints + timeBonus;
        }

        public async Task<ScoreDto> GetUserScoreAsync(string sessionId)
        {
            var score = await _scoreRepository.GetBySessionIdAsync(sessionId);
            var progress = await _progressRepository.GetBySessionIdAsync(sessionId);

            if (score == null)
            {
                return new ScoreDto
                {
                    TotalPoints = 0,
                    IQCategory = "Beginner",
                    ProgressPercent = 0,
                    QuestionsAnswered = 0,
                    CorrectAnswers = 0
                };
            }

            return new ScoreDto
            {
                TotalPoints = score.Points,
                IQCategory = GetIQCategory(score.Points),
                ProgressPercent = progress?.ProgressPercent ?? 0,
                QuestionsAnswered = score.QuestionsAnswered,
                CorrectAnswers = score.CorrectAnswers
            };
        }

        public async Task AddScoreAsync(string sessionId, int points)
        {
            var score = await _scoreRepository.GetBySessionIdAsync(sessionId);

            if (score == null)
            {
                score = new UserScore
                {
                    SessionId = sessionId,
                    Points = points,
                    QuestionsAnswered = 1,
                    CorrectAnswers = points > 0 ? 1 : 0,
                    CreatedAt = DateTime.Now
                };
                await _scoreRepository.AddAsync(score);
            }
            else
            {
                score.Points += points;
                score.QuestionsAnswered++;
                if (points > 0) score.CorrectAnswers++;
                await _scoreRepository.UpdateAsync(score);
            }
        }

        public string GetIQCategory(int totalPoints)
        {
            return totalPoints switch
            {
                < 300 => "Beginner",
                < 600 => "Average",
                < 1000 => "Above Average",
                < 1500 => "Smart",
                < 2500 => "Very Smart",
                < 4000 => "Genius",
                _ => "Einstein Level"
            };
        }

        public async Task<int> GetProgressPercentAsync(string sessionId)
        {
            var progress = await _progressRepository.GetBySessionIdAsync(sessionId);
            return progress?.ProgressPercent ?? 0;
        }
    }
}