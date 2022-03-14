using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TemboShared.Model;
using TemboShared.Service;
using Websocket.Client;

namespace TemboAI.Core.Communication
{
    public class TSC : INotifyPropertyChanged
    {
        private WebsocketClient webSocketClient;
        private List<Candlestick> recentCandlesticks;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged<T>(ref T var, T value, [CallerMemberName] string name = null)
        {
            var = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Contains most recent candles as per request
        /// </summary>
        public List<Candlestick> RecentCandlesticks
        {
            get => recentCandlesticks;
            set => OnPropertyChanged(ref recentCandlesticks, value);
        }

        public void Connect()
        {
            //var url = new Uri("ws://127.0.0.1:8080/echo");
            var url = new Uri("ws://127.0.0.1:8080/echo");
            webSocketClient = new WebsocketClient(url)
            {
                ReconnectTimeout = TimeSpan.FromMinutes(60)
            };
            webSocketClient.ReconnectionHappened.Subscribe(type => { OnRecconnect(type.Type); });
            webSocketClient.MessageReceived.Subscribe(OnMessageReceived);
            webSocketClient.Start();

            //Task.Run(() => StartSendingPing(WSClient));
        }

        private static void OnRecconnect(ReconnectionType type)
        {
            Console.WriteLine($"Connected. {type}");
        }

        private void OnMessageReceived(ResponseMessage msg)
        {
            try
            {
                RecentCandlesticks = JsonConvert.DeserializeObject<List<Candlestick>>(msg.Text);
                if (RecentCandlesticks.Count > 1)
                {
                    $"{RecentCandlesticks.Count} rates received".Log(3);
                }
            }
            catch
            {
                RecentCandlesticks = null;
            }
        }

        public void Send(string command)
        {
            webSocketClient.Send(command);
        }

        private static async Task StartSendingPing(WebsocketClient client)
        {
            var request = 0;
            while (request == 0)
            {
                request = 1;
                await Task.Delay(5000);
                dynamic tickRequest = new ExpandoObject();
                tickRequest.type = "Candles";
                tickRequest.timeFrame = "M1";
                tickRequest.symbol = "EURUSD";
                tickRequest.count = 10;
                await client.Send(JsonConvert.SerializeObject(tickRequest));
            }
        }

        public void Dispose()
        {
            webSocketClient.Dispose();
        }
    }
}
