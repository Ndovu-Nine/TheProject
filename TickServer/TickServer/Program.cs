using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alchemy;
using Alchemy.Classes;
using Newtonsoft.Json;
using System.Net;
using MtApi5;

namespace TickServer
{
    class Program
    {
        /// <summary>
        /// Store the list of online users. Wish I had a ConcurrentList. 
        /// </summary>
        protected static ConcurrentDictionary<User, string> OnlineUsers = new ConcurrentDictionary<User, string>();
        private static MtApi5Client Terminal;
        public static bool IsConnected=false;
        private static int Next { get; set; }
        private static string[] Assets = new string[] { "EURUSD", "GBPUSD", "AUDUSD", "NZDUSD", "USDJPY", "EURNZD", "EURAUD", "EURCAD", "EURGBP", "EURJPY" };
        public static double MessagesSent = 0;
        public static int ccn = 1000;
        static readonly ConcurrentBag<OpenDeal>_openDeals= new ConcurrentBag<OpenDeal>();
        static  Telegram AllTimeFramesMessenger;

        private static dynamic settings;
        public static readonly string Base = AppDomain.CurrentDomain.BaseDirectory;

        public static int GetNext()
        {
            Next++;
            return Next;
        }
        static void Main(string[] args)
        {
            AllTimeFramesMessenger=new Telegram(Settings.telegramBotKey, "Name Of My Bot");
            Terminal = new MtApi5Client();
            Terminal.ConnectionStateChanged+= new EventHandler<Mt5ConnectionEventArgs>(Terminal_ConnectionStateChanged);
            Terminal.BeginConnect("127.0.0.1", 8228);
            var aServer = new WebSocketServer(8080, IPAddress.Any)
            {
                OnReceive = OnReceive,
                OnSend = OnSend,
                OnConnected = OnConnect,
                OnDisconnect = OnDisconnect,
                TimeOut = new TimeSpan(0, 30, 0)
            };
            aServer.Start();
            // Accept commands on the console and keep it alive
            var command = string.Empty;
            while (command != "exit")
            {
                command = Console.ReadLine();
            }

            aServer.Stop();
            
        }
        public static dynamic Settings
        {
            get
            {
                var fsSource = new FileStream($"{Base}settings.json", FileMode.Open, FileAccess.Read);
                var sr = new StreamReader(fsSource);
                settings = JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd());
                sr.Close();
                sr.Dispose();
                fsSource.Close();
                fsSource.Dispose();
                return settings;
            }
        }
        private static void Terminal_ConnectionStateChanged(object sender, Mt5ConnectionEventArgs e)
        {
            if (e.Status== Mt5ConnectionState.Connected)
            {
                IsConnected = true;
                Broadcast("Quote server connected!!");
                Console.WriteLine("Quote server connected!!");
            }
            else if(e.Status==Mt5ConnectionState.Disconnected || e.Status==Mt5ConnectionState.Failed)
            {
                IsConnected = false;
                Broadcast("Quote server was disconnected");
                Console.WriteLine("MT5 Failed to connect or MT5 was disconnected");
            }
            else
            {
                Console.WriteLine("Connecting to MT5");
            }
        }

        private static void OnDisconnect(UserContext context)
        {
            try
            {
                //Console.WriteLine("Client Disconnected : " + context.ClientAddress);
                var user = OnlineUsers.Keys.Single(o => o.Context.ClientAddress == context.ClientAddress);
                OnlineUsers.TryRemove(user, out _);
            }
            catch(Exception ex)
            {
                //Console.WriteLine($"An error occured while running OnDisconnect, MSG: {ex.Message}");
            }
        }

        /// <summary>
        /// Broadcasts a message to all users, or if users is populated, a select list of users
        /// </summary>
        /// <param name="message">Message to be broadcast</param>
        public static void Broadcast(string message)
        {
            foreach (var u in OnlineUsers.Keys)
            {
                u.Context.Send(message);
            }
        }
        /// <summary>
        /// Broadcasts a list of all online users to all online users
        /// </summary>
        private static void BroadcastNameList()
        {
            var r = new Response
            {
                Type = ResponseType.UserCount,
                Data = new { Users = OnlineUsers.Values.Where(o => !String.IsNullOrEmpty(o)).ToArray() }
            };
            Broadcast(JsonConvert.SerializeObject(r));
        }

        private static void OnConnect(UserContext context)
        {
            Console.WriteLine("Client Connection From : " + context.ClientAddress);

            var me = new User { Context = context };

            OnlineUsers.TryAdd(me, context.ClientAddress.ToString());
        }
       
        private static void OnSend(UserContext context)
        {
            MessagesSent++;
            if (MessagesSent > ccn)
            {                
                ccn += 1000;
                Console.WriteLine($"Messages sent: {MessagesSent}, next log {ccn}");
            }
        }

        private static void OnReceive(UserContext context)
        {
            try
            {
                if (!IsConnected) return;
                var contentString = context.DataFrame.ToString();
                dynamic request = JsonConvert.DeserializeObject(contentString);
                string type = request.type;

                switch (type)
                {
                    case "Candles":
                        string symbol = request.symbol;
                        if (!Assets.Contains(symbol))
                        {
                            context.Send("Symbol not activated!");
                            return;
                        }

                        int count = request.count;
                        string timeFrame = request.timeFrame;
                        MqlRates[] mqlRatesArray;
                        switch (timeFrame)
                        {
                            case "M1":
                                Terminal.CopyRates(symbol, ENUM_TIMEFRAMES.PERIOD_M1, 1, count, out mqlRatesArray);
                                context.Send(ToJson(FromMQLRate(mqlRatesArray, symbol, timeFrame)));
                                break;
                            case "M5":
                                Terminal.CopyRates(symbol, ENUM_TIMEFRAMES.PERIOD_M5, 1, count, out mqlRatesArray);
                                context.Send(ToJson(FromMQLRate(mqlRatesArray, symbol, timeFrame)));
                                break;
                            case "M15":
                                Terminal.CopyRates(symbol, ENUM_TIMEFRAMES.PERIOD_M15, 1, count, out mqlRatesArray);
                                context.Send(ToJson(FromMQLRate(mqlRatesArray, symbol, timeFrame)));
                                break;
                            case "M30":
                                Terminal.CopyRates(symbol, ENUM_TIMEFRAMES.PERIOD_M30, 1, count, out mqlRatesArray);
                                context.Send(ToJson(FromMQLRate(mqlRatesArray, symbol, timeFrame)));
                                break;
                            case "H1":
                                Terminal.CopyRates(symbol, ENUM_TIMEFRAMES.PERIOD_H1, 1, count, out mqlRatesArray);
                                context.Send(ToJson(FromMQLRate(mqlRatesArray, symbol, timeFrame)));
                                break;
                            case "H4":
                                Terminal.CopyRates(symbol, ENUM_TIMEFRAMES.PERIOD_H4, 1, count, out mqlRatesArray);
                                context.Send(ToJson(FromMQLRate(mqlRatesArray, symbol, timeFrame)));
                                break;
                            case "D1":
                                Terminal.CopyRates(symbol, ENUM_TIMEFRAMES.PERIOD_D1, 1, count, out mqlRatesArray);
                                context.Send(ToJson(FromMQLRate(mqlRatesArray, symbol, timeFrame)));
                                break;
                            case "W1":
                                Terminal.CopyRates(symbol, ENUM_TIMEFRAMES.PERIOD_W1, 1, count, out mqlRatesArray);
                                context.Send(ToJson(FromMQLRate(mqlRatesArray, symbol, timeFrame)));
                                break;
                            case "MN1":
                                Terminal.CopyRates(symbol, ENUM_TIMEFRAMES.PERIOD_MN1, 1, count, out mqlRatesArray);
                                context.Send(ToJson(FromMQLRate(mqlRatesArray, symbol, timeFrame)));
                                break;
                            default:
                                Terminal.CopyRates(symbol, ENUM_TIMEFRAMES.PERIOD_M1, 1, count, out mqlRatesArray);
                                context.Send(ToJson(FromMQLRate(mqlRatesArray, symbol, "M1")));
                                break;
                        }
                        break;
                    case "Signal":
                        Broadcast(request.message);
                        break;
                    case "Trade":
                        OpenTrade(request);
                        break;
                    case "ClosePosition":
                        string identifier = request.identifier;
                        CloseTrade(identifier);
                        break;
                    case "TelegramMessage":
                        string msg = request.details;
                        string durationOfCandle = request.durationOfCandle;
                        AllTimeFramesMessenger.SendMessage(msg);
                        break;
                        /*switch (durationOfCandle)
                        {
                            case "H1":
                                H1Messenger.SendMessage(msg);
                                break;
                            case "M5":
                               M5Messenger.SendMessage(msg);
                                break;
                            case "M1":
                                M1Messenger.SendMessage(msg);
                                break;
                            default:
                                AllTimeFramesMessenger.SendMessage(msg);
                                break;
                        }
                        */
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var r = new Response {Type = ResponseType.Error, Data = new {ex.Message}};
                context.Send(JsonConvert.SerializeObject(r));
            }
        }

        private static double PositionSize()
        {
            var balance = Terminal.AccountInfoDouble(ENUM_ACCOUNT_INFO_DOUBLE.ACCOUNT_BALANCE);
            return Math.Round((0.01 / 100) * balance,2);
        }

        private static double PositionSize(string durationOfCandle)
        {
            var balance = Terminal.AccountInfoDouble(ENUM_ACCOUNT_INFO_DOUBLE.ACCOUNT_BALANCE);
            switch (durationOfCandle)
            {
                case "H1":
                    return Math.Round((0.012 / 100) * balance, 2);
                case "M15":
                    return Math.Round((0.006 / 100) * balance, 2);
                case "M5":
                    return Math.Round((0.0012 / 100) * balance, 2);
                case "M1":
                    return Math.Round((0.0006 / 100) * balance, 2);
                default:
                    return Math.Round((0.012 / 100) * balance, 2);
            }
            
        }

        private static void OpenTrade(dynamic request)
        {
            string direction = request.direction;
            string symbol = request.symbol;
            string identifier = request.identifier;
            string durationOfCandle = request.durationOfCandle;
            MqlTradeResult result;
            switch (direction)
            {
                case "BUY":
                    if (_openDeals.Count(d => d.Direction == direction && d.IsOpen)<=0)
                    {
                        Terminal.PositionOpen(symbol, ENUM_ORDER_TYPE.ORDER_TYPE_BUY, PositionSize(durationOfCandle), 0.0, 0.0, 0.0, $"{durationOfCandle}.{identifier}", out result);
                        
                        Console.WriteLine($"Position {durationOfCandle}.{identifier} opened successfully, Deal {result.Order}");
                        CloseTradeWithDirection("SELL",symbol,durationOfCandle);
                        var openDeal = new OpenDeal
                        {
                            Identifier = identifier,
                            Ticket = result.Order,
                            Direction = direction,
                            IsOpen = true,
                            Symbol = symbol,
                            DurationOfCandle = durationOfCandle
                        };
                        _openDeals.Add(openDeal);
                    }
                    break;
                case "SELL":
                    if (_openDeals.Count(d => d.Direction == direction && d.IsOpen) <= 0)
                    {
                        Terminal.PositionOpen(symbol, ENUM_ORDER_TYPE.ORDER_TYPE_SELL, PositionSize(durationOfCandle), 0.0, 0.0, 0.0, $"{durationOfCandle}.{identifier}", out result);
                        Console.WriteLine($"Position {durationOfCandle}.{identifier} opened successfully, Deal {result.Order}");
                        CloseTradeWithDirection("BUY",symbol,durationOfCandle);
                        var openSellDeal = new OpenDeal
                        {
                            Identifier = identifier,
                            Ticket = result.Order,
                            Direction = direction,
                            IsOpen = true,
                            Symbol = symbol,
                            DurationOfCandle = durationOfCandle
                        };
                        _openDeals.Add(openSellDeal);
                    }
                    break;
            }
        }

        private static void CloseTrade(string identifier)
        {
            MqlTradeResult result;
            foreach (var openDeal in _openDeals)
            {
                if (openDeal.Identifier == identifier && openDeal.IsOpen)
                {
                    Terminal.PositionClose(openDeal.Ticket, out result);
                    openDeal.IsOpen = false;
                    Console.WriteLine($"Position {identifier} closed successfully, Deal {openDeal.Ticket}");
                }
            }

        }

        private static void CloseTradeWithDirection(string direction, string symbol, string durationOfCandle)
        {
            MqlTradeResult result;
            foreach (var openDeal in _openDeals)
            {
                if (openDeal.Direction == direction && openDeal.Symbol==symbol && openDeal.DurationOfCandle==durationOfCandle)
                {
                    Terminal.PositionClose(openDeal.Ticket, out result);
                    openDeal.IsOpen = false;
                    Console.WriteLine($"Position {durationOfCandle}.{openDeal.Identifier} closed successfully, Deal {openDeal.Ticket}");
                }
            }
        }

        public static Candlestick FromMQLRate(MqlRates rate, string symbol,string duration)
        {
            return new Candlestick(symbol, rate.open, rate.high, rate.low, rate.close, rate.time, duration,(int)rate.tick_volume);
        }
        public static List<Candlestick> FromMQLRate(MqlRates[] rate, string symbol, string duration)
        {
            var list = new List<Candlestick>();
            foreach(var sd in rate)
            {
                list.Add(new Candlestick(symbol, sd.open, sd.high, sd.low, sd.close, sd.time, duration,(int)sd.tick_volume));
            }
            return list;
        }
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Serialize, Formatting = Formatting.None });
        }
    }

}
