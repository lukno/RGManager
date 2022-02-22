using System.Collections.Generic;

namespace RGManager.Model.Entities
{
    public class Player
    {
        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public List<GameResult> Results { get; set; } = new();
    }
}
