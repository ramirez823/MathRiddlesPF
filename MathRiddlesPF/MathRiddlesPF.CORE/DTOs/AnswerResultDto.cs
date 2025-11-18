// DTOs/AnswerResultDto.cs
using MathRiddlesPF.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathRiddlesPF.CORE.DTOs
{
    public class AnswerResultDto
    {
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public AnswerStatus Status { get; set; }
    }
}