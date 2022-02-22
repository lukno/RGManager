using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Formatter;
using RGManager.Entites.Entities;
using RGManager.Wpf.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RGManager.Wpf.View
{
    public partial class GameControl : UserControl
    {
        IMqttClient client;
        public TimeSpan startTime;
        public TimeSpan roundTime;
        public TimeSpan startTimeCurrentGame;
        Player currentPlayer;
        public int rounds;

        public int Count { get; set; }
        public Stopwatch Timer { get; set; } = new();
        public GameResult GameResult { get; set; } = new();

        public GameControl()
        {
            InitializeComponent();

            CreateClient();
            ConnectAsync();
        }

        public void SetCombox()
        {
            cmbPlayers.Items.Clear();

            if (Global.Players.Any())
                foreach (var player in Global.Players)
                    cmbPlayers.Items.Add(new ComboBoxItem { Content = player.Name });

            CurrentGameDataGrid.Items.Clear();
            GameResultGrid.Visibility = Visibility.Hidden;
        }

        private void cmbPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBoxItem = (ComboBoxItem)cmbPlayers.SelectedItem;
            if (comboBoxItem is not null)
                currentPlayer = Global.Players.FirstOrDefault(p => p.Name == comboBoxItem.Content.ToString());
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            if (!CheckNumeberOfRounds())
                return;

            if (!CheckIfPlayerIsSelected())
                return;

            Count = 0;
            GameResultGrid.Visibility = Visibility.Visible;


            Round.Text = $"0/{rounds}";
        }

        private async Task CountToZero()
        {
            for (int i = 3; i >= 0; i--)
            {
                Round.Text = $"{i}";
                System.Threading.Thread.Sleep(1000);
            }
            Round.Text = $"Start";
            System.Threading.Thread.Sleep(500);

            Round.Text = $"0/{rounds}";
        }

        private bool CheckNumeberOfRounds()
        {
            bool success = int.TryParse(NumberOfRounds.Text.Trim(), out int x);

            if (success)
            {
                if (x <= 1)
                {
                    AlertRounds.Text = "Podaj liczbę wiekszą niż 1";
                    return false;
                }
                AlertRounds.Text = "";
                rounds = x;
                return true;
            }
            else
            {
                AlertRounds.Text = "Podaj liczbe";
                return false;
            }
        }

        private bool CheckIfPlayerIsSelected()
        {
            if ((ComboBoxItem)cmbPlayers.SelectedItem is null)
            {
                AlertRounds.Text = "Wybierz gracza";
                return false;
            }

            AlertPlayer.Text = "";
            return true;
        }

        private void CreateClient()
        {
            client = new MqttFactory().CreateMqttClient()
              .UseConnectedHandler(ConnectHandler)
              .UseApplicationMessageReceivedHandler(ApplicationMessageReceiveHandler)
              .UseDisconnectedHandler(DisconnectHandler);
        }

        private async Task ConnectHandler(MqttClientConnectedEventArgs e)
        {
            await client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("university/race/time").WithExactlyOnceQoS().Build());
        }

        private async Task ApplicationMessageReceiveHandler(MqttApplicationMessageReceivedEventArgs e)
        {
            Count++;

            if (Count == 1)
            {
                Round.Dispatcher.Invoke(() => Round.Text = $"{Count}/{rounds}");
                startTime = Timer.Elapsed;
                startTimeCurrentGame = Timer.Elapsed;
                Timer.Start();
            }
            else if (Count > 1 && Count <= (rounds + 1))
            {
                Round.Dispatcher.Invoke(() => Round.Text = $"{Count}/{rounds}");
                roundTime = Timer.Elapsed - startTime;
                startTime = Timer.Elapsed;
                GameResult.LapResults.Add(roundTime);
                var item = new CurrentGame
                {
                    Index = Count - 1,
                    Time = string.Format("{0}:{1}", Math.Floor(Timer.Elapsed.TotalMinutes), Timer.Elapsed.ToString("ss\\.ff")) + " min"
                };

                CurrentGameDataGrid.Dispatcher.Invoke(() => CurrentGameDataGrid.Items.Add(item));
                if (Count == (rounds + 1))
                {
                    Round.Dispatcher.Invoke(() => Round.Text = "KONIEC! ");

                    //AddResultToJson
                    GameResult.BestTime = GameResult.LapResults.OrderBy(o => o.Ticks).First();
                    Global.Players.FirstOrDefault(p => p.Name == currentPlayer.Name).Results.Add(GameResult);
                    Timer.Stop();
                    Timer.Reset();
                    GameResult = new GameResult();
                }
            }
        }

        public static async Task DisconnectHandler(MqttClientDisconnectedEventArgs e)
        {
            MessageBox.Show("Rozłączono MQTT");
        }

        private async void ConnectAsync()
        {
            try
            {
                var mqttFactory = new MqttFactory();

                var options = new MqttClientOptionsBuilder()
                    .WithClientId("test-publisher")
                    .WithTcpServer("test.mosquitto.org", 1883)
                    .WithCleanSession(false)
                    .WithProtocolVersion(MqttProtocolVersion.V500)
                    .Build();

                await client.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie można się połączyć do brokera. {ex.Message}");
            }
        }
    }
}
