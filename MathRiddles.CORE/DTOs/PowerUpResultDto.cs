// DTOs/PowerUpResultDto.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MathRiddlesPF.CORE.DTOs

{
    public class PowerUpResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> RemainingOptions { get; set; }
        public string Hint { get; set; }
        public int ExtraTimeAdded { get; set; }
    }
}