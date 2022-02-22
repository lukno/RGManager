using System.Collections.Generic;

namespace RGManager.Model.Entities
{
    public class Game
    {
        public string GameId { get; set; }
        public List<Player> Players { get; set; } = new();
    }
}
