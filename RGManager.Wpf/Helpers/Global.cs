using RGManager.Entites.Entities;
using System.Collections.Generic;

namespace RGManager.Wpf.Helpers
{
    public class Global
    {
        public static string SessionId;
        public static List<Player> Players { get; set; } = new();
    }
}
