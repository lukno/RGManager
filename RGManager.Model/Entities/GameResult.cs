using System;
using System.Collections.Generic;

namespace RGManager.Model.Entities
{
    public class GameResult
    {
        public List<TimeSpan> LapResults { get; set; } = new();
        public TimeSpan BestTime { get; set; }
    }
}
