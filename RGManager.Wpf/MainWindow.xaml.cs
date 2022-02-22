using RGManager.Entites;
using RGManager.Entites.Entities;
using RGManager.Wpf.Helpers;
using RGManager.Wpf.View;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace RGManager.Wpf
{
    public partial class MainWindow : Window
    {
        private const string FILE_NAME = "\\RGManager.Entites\\Database\\GameResults.json";

        public MainWindow()
        {
            InitializeComponent();
            Global.SessionId = GenerateId();
        }

        private void ButtonPopUpLogout_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + FILE_NAME;
            var game = new Game
            {
                GameId = Global.SessionId,
                Players = Global.Players
            };
            GameRepository.WriteToJsonFile<Game>(path,game);

            Application.Current.Shutdown();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ResultsViewClick(object sender, MouseButtonEventArgs e)
        {
            GameResultsControl.Visibility = Visibility.Visible;
            GameControl.Visibility = Visibility.Hidden;
            AddUserControl.Visibility = Visibility.Hidden;
            GameResultsControl.SetCombox();
        }

        private void GameViewClick(object sender, MouseButtonEventArgs e)
        {
            GameResultsControl.Visibility = Visibility.Hidden;
            GameControl.Visibility = Visibility.Visible;
            AddUserControl.Visibility = Visibility.Hidden;
            GameControl.SetCombox();
        }

        private void AddUserViewClick(object sender, MouseButtonEventArgs e)
        {
            GameResultsControl.Visibility = Visibility.Hidden;
            GameControl.Visibility = Visibility.Hidden;
            AddUserControl.Visibility = Visibility.Visible;
        }

        private string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
