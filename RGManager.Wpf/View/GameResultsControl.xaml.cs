using RGManager.Entites;
using RGManager.Entites.Entities;
using RGManager.Wpf.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace RGManager.Wpf.View
{
    /// <summary>
    /// Interaction logic for GameResultsControl.xaml
    /// </summary>
    public partial class GameResultsControl : UserControl
    {
        private const string FILE_NAME = "\\RGManager.Entites\\Database\\GameResults.json";

        public GameResultsControl()
        {
            InitializeComponent();
            ClearDataGrid();
        }

        public void SetCombox()
        {
            //cmbPlayers.Items.Clear();

            //if (Global.Players.Any())
            //    foreach (var player in Global.Players)
            //        cmbPlayers.Items.Add(new ComboBoxItem { Content = player.Name });
            ClearDataGrid();
            FillInTheCurrentGameResultsDataGrid();
            FillInTheAllGameResultsDataGrid();

        }

        //private void cmbPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ClearDataGrid();
        //    Player player = new Player();
        //    player = null;

        //    var comboBoxItem = (ComboBoxItem)cmbPlayers.SelectedItem;
        //    if (comboBoxItem is not null)
        //        player = Global.Players.FirstOrDefault(p => p.Name == comboBoxItem.Content.ToString());

        //    if (player is null)
        //        return;

        //    //FillInTheAllGameResultsDataGrid(player);
        //    //FillInTheCurrentGameResultsDataGrid(player);
        //}

        private void FillInTheAllGameResultsDataGrid()
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + FILE_NAME;
            var games = GameRepository.ReadFromJsonFile<List<Game>>(path);

            var players = new List<Player>();

            if (games is not null)
                players = games.SelectMany(x => x.Players.Select(y => y)).Where(p => p.Results.Any()).ToList();

            if (Global.Players.Any())
                players.AddRange(Global.Players.Where(p => p.Results.Any()));

            if (players.Any())
            {
                var numberOfPlayers = players.Count;
                if (numberOfPlayers > 15)
                    numberOfPlayers = 15;

                var allGameResults = SortTheBestResultsFromTheAllSessionDesc(players, numberOfPlayers);

                if (allGameResults.Any())
                {
                    foreach (var result in allGameResults)
                        AllGameResultsDataGrid.Dispatcher.Invoke(() => AllGameResultsDataGrid.Items.Add(result));
                }
            }
        }

        private void FillInTheCurrentGameResultsDataGrid()
        {
            var players = new List<Player>();

            if (Global.Players.Any())
                players.AddRange(Global.Players.Where(p => p.Results.Any()));

            if (players.Any())
            {
                var resultsFromTheCurrentGame = SortTheBestResultsFromTheCurrentGameDesc(players);

                if (resultsFromTheCurrentGame.Any())
                {
                    foreach (var result in resultsFromTheCurrentGame)
                        CurrentGameResultsDataGrid.Dispatcher.Invoke(() => CurrentGameResultsDataGrid.Items.Add(result));
                }
            }
        }

        private List<CurrentGameResults> SortTheBestResultsFromTheCurrentGameDesc(List<Player> players)
        {
            var selectedListOfPlayers = players.Where(x => x.Results.Any()).ToList();

            var listOfPlayers = new List<Player>();

            if (!selectedListOfPlayers.Any())
                return new List<CurrentGameResults>();

            foreach (var player in selectedListOfPlayers)
            {
                foreach (var result in player.Results)
                {
                    listOfPlayers.Add(new Player
                    {
                        Name = player.Name,
                        Results = new List<GameResult> { result }
                    });
                }
            }

            var list = listOfPlayers.Select((x, index) => new ClassHelper2
            {
                Name = x.Name,
                Rounds = x.Results.Count,
                BestTime = x.Results.First().BestTime,
                //BestTime = x.Results.OrderBy(o => o.BestTime).FirstOrDefault().BestTime,
                Results = CreateResult(x.Results.First())
            })
            .OrderBy(o => o.BestTime.Ticks)
            .ToList();

            for (int i = 1; i <= list.Count; i++)
            {
                list[i - 1].Index = i;
            }

           // var currentPlayerInList = list.Where(x => x.Name == currentSelectedPlayer.Name).OrderBy(o => o.BestTime).First();

            var currentGameResults = new List<CurrentGameResults>();

            //currentGameResults.Add(new CurrentGameResults
            //{
            //    Index = currentPlayerInList.Index,
            //    Name = currentPlayerInList.Name,
            //    Rounds = currentPlayerInList.Rounds,
            //    BestTime = string.Format("{0}:{1}", Math.Floor(currentPlayerInList.BestTime.TotalMinutes), currentPlayerInList.BestTime.ToString("ss\\.ff")) + " min",
            //    Results = currentPlayerInList.Results
            //});

            currentGameResults.AddRange(list.Select(x => new CurrentGameResults
            {
                Index = x.Index,
                Name = x.Name,
                Rounds = x.Rounds,
                BestTime = string.Format("{0}:{1}", Math.Floor(x.BestTime.TotalMinutes), x.BestTime.ToString("ss\\.ff")) + " min",
                Results = x.Results
            })
            .ToList());

            return currentGameResults;
        }

        private string CreateResult(GameResult gameResult)
        {
            string result = "";

            foreach (var item in gameResult.LapResults)
                result += string.Format("{0}:{1}", Math.Floor(item.TotalMinutes), item.ToString("ss\\.ff")) + " min";

            return result;
        }

        private List<AllGameResult> SortTheBestResultsFromTheAllSessionDesc(List<Player> players, int numbersOfPlayers = 15)
        {
            var selectedListOfPlayers = players.Where(x => x.Results.Any()).ToList();

            var list = selectedListOfPlayers.Select((x, index) => new ClassHelper
            {
                Name = x.Name,
                Time = x.Results.OrderBy(o => o.BestTime.Ticks).FirstOrDefault().BestTime
            })
            .OrderBy(o => o.Time.Ticks)
            .ToList();

            for (int i = 1; i <= list.Count; i++)
            {
                list[i - 1].Index = i;
            }

            //var currentPlayerInList = list.Where(x => x.Name == currentPlayer.Name).LastOrDefault();

            var allGameResults = new List<AllGameResult>();

            //allGameResults.Add(new AllGameResult
            //{
            //    Index = currentPlayerInList.Index,
            //    Name = currentPlayerInList.Name,
            //    Time = string.Format("{0}:{1}", Math.Floor(currentPlayerInList.Time.TotalMinutes), currentPlayerInList.Time.ToString("ss\\.ff")) + " min"
            //});

            allGameResults.AddRange(list.Select(x => new AllGameResult
            {
                Index = x.Index,
                Name = x.Name,
                Time = string.Format("{0}:{1}", Math.Floor(x.Time.TotalMinutes), x.Time.ToString("ss\\.ff")) + " min"
            })
            .Take(numbersOfPlayers)
            .ToList());

            return allGameResults;
        }

        private void ClearDataGrid()
        {
            CurrentGameResultsDataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            AllGameResultsDataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            CurrentGameResultsDataGrid.Items.Clear();
            AllGameResultsDataGrid.Items.Clear();
            CurrentGameResultsDataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
        }
    }

    public class ClassHelper
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
    }

    public class ClassHelper2
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int Rounds { get; set; }
        public TimeSpan BestTime { get; set; }
        public string Results { get; set; }
    }
}
