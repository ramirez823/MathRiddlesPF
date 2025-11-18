using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MathRiddlesPF.CORE.Interfaces
{
    public interface ILevelService
    {
        Task<List<LevelDto>> GetAllLevelsAsync();
        Task<LevelDto> GetLevelByIdAsync(int levelId);
        Task<bool> IsLevelUnlockedAsync(string sessionId, int levelId);
        Task UnlockNextLevelAsync(string sessionId, int currentLevelId);
    }

    public class LevelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Difficulty { get; set; }
        public bool IsUnlocked { get; set; }
        public int TotalRiddles { get; set; }
    }

    
}
