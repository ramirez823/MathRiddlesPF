// DTOs/ScoreDto.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathRiddlesPF.CORE.DTOs
{
    public class ScoreDto
    {
        public int TotalPoints { get; set; }
        public string IQCategory { get; set; }
        public int ProgressPercent { get; set; }
        public int QuestionsAnswered { get; set; }
        public int CorrectAnswers { get; set; }
    }
}