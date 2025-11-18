// DTOs/RiddleDto.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathRiddlesPF.CORE.DTOs
{
    public class RiddleDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int TimeLimit { get; set; }
        public int LevelId { get; set; }
        public string Difficulty { get; set; }
        public bool HasMultipleChoice { get; set; }
    }
}