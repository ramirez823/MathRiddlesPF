using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathRiddlesPF.CORE.DTOs;
using MathRiddlesPF.CORE.Enums;
using MathRiddlesPF.CORE.Interfaces;
using MathRiddlesPF.DATA.Entities;
using MathRiddlesPF.DATA.Repositories;
using System.Text.Json;


namespace MathRiddlesPF.CORE.Services
{
    public class RiddleService : IRiddleService
    {
        private readonly IRiddleRepository _riddleRepository;

        public RiddleService(IRiddleRepository riddleRepository)
        {
            _riddleRepository = riddleRepository;
        }

        public async Task<RiddleDto> GetRiddleByIdAsync(int riddleId)
        {
            var riddle = await _riddleRepository.GetByIdAsync(riddleId);
            if (riddle == null) return null;

            return MapToDto(riddle);
        }

        public async Task<RiddleDto> GetRandomRiddleByLevelAsync(int levelId)
        {
            var riddles = await _riddleRepository.GetByLevelIdAsync(levelId);
            if (!riddles.Any()) return null;

            var random = new Random();
            var riddle = riddles[random.Next(riddles.Count)];

            return MapToDto(riddle);
        }

        public async Task<List<RiddleDto>> GetRiddlesByLevelAsync(int levelId)
        {
            var riddles = await _riddleRepository.GetByLevelIdAsync(levelId);
            return riddles.Select(MapToDto).ToList();
        }

        public async Task<AnswerResultDto> ValidateAnswerAsync(int riddleId, string userAnswer, int timeRemaining)
        {
            var riddle = await _riddleRepository.GetByIdAsync(riddleId);
            if (riddle == null)
            {
                return new AnswerResultDto
                {
                    IsCorrect = false,
                    Status = AnswerStatus.Incorrect,
                    PointsEarned = 0,
                    CorrectAnswer = "Error: Pregunta no encontrada"
                };
            }

            var isCorrect = ValidateAnswer(userAnswer, riddle.CorrectAnswer);
            var status = timeRemaining <= 0 ? AnswerStatus.Timeout :
                         (isCorrect ? AnswerStatus.Correct : AnswerStatus.Incorrect);

            var points = isCorrect && timeRemaining > 0
                ? CalculatePoints(timeRemaining, riddle.Level?.Difficulty ?? 1)
                : 0;

            return new AnswerResultDto
            {
                IsCorrect = isCorrect,
                PointsEarned = points,
                CorrectAnswer = riddle.CorrectAnswer,
                Status = status,
                Explanation = $"Respuesta correcta: {riddle.CorrectAnswer}"
            };
        }

        private bool ValidateAnswer(string userAnswer, string correctAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer)) return false;

            return userAnswer.Trim().Equals(correctAnswer.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private int CalculatePoints(int timeRemaining, int difficulty)
        {
            // Fórmula: Base (difficulty * 50) + Time Bonus (timeRemaining * 2)
            int basePoints = difficulty * 50;
            int timeBonus = timeRemaining * 2;
            return basePoints + timeBonus;
        }

        private RiddleDto MapToDto(Riddle riddle)
        {
            var options = new List<string>();
            if (!string.IsNullOrEmpty(riddle.OptionsJson))
            {
                options = JsonSerializer.Deserialize<List<string>>(riddle.OptionsJson) ?? new List<string>();
            }

            return new RiddleDto
            {
                Id = riddle.Id,
                Question = riddle.Question,
                Options = options,
                TimeLimit = riddle.TimeLimit,
                LevelId = riddle.LevelId,
                Difficulty = riddle.Level?.Name ?? "Unknown",
                HasMultipleChoice = options.Any()
            };
        }
    }
}