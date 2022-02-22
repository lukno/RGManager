using System;
using System.Collections.Generic;

namespace RGManager.Entites.Entities
{
    public class GameResult
    {
        public List<TimeSpan> LapResults { get; set; } = new List<TimeSpan>();
        public TimeSpan BestTime { get; set; }
    }
}
