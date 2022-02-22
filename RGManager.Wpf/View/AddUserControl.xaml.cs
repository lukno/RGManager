using RGManager.Entites;
using RGManager.Entites.Entities;
using RGManager.Wpf.Helpers;
using RGManager.Wpf.View.Customs;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RGManager.Wpf.View
{
    public partial class AddUserControl : UserControl
    {
        private const string FILE_NAME = "\\RGManager.Entites\\Database\\CurrentGamePlayers.json";

        public AddUserControl()
        {
            InitializeComponent();
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName.Text))
            {
                Alert.Text = "Podaj nazwe nowego gracza";
                return;
            }

            if (Global.Players.Any())
            {
                if (Global.Players.Where(p => p.Name.ToLower() == UserName.Text.Trim().ToLower()).Any())
                {
                    Alert.Text = "Gracz już istnieje";
                    return;
                }
            }

            Alert.Text = "";
            var player = new Player { Name = UserName.Text.Trim() };
            Global.Players.Add(player);
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + FILE_NAME;

            GameRepository.WriteToJsonFile(path, player);

            new MessageBoxCustom("Dodałeś gracza", MessageButtons.Ok).ShowDialog();





            ////var  a = GameRepository.ReadFromJsonFile<List<Game>>(path);

            //var user = new Player(UserName.Text);
            ////var games = new List<Game>();
            ////games.Add(new Game() { Players = new List<Player> { user } }) ;
            ////games.Add(new Game() { Players = new List<Player> { user } });

            //var game = new Game() { GameId = Global.SessionId, Players = new List<Player> { user } };

            //var games = new List<Game> { game };

            //GameRepository.WriteToJsonFile(path, game);

            //var b = GameRepository.ReadFromJsonFile<List<Game>>(path);

            //Console.WriteLine();

            //ApplicationDb.Save(user);

            //ApplicationDb.Read();
        }
    }
}
