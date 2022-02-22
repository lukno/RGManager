using System.Collections.Generic;

namespace RGManager.Entites.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public List<GameResult> Results { get; set; } = new List<GameResult>();
    }
}
