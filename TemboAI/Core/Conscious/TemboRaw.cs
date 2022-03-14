using NdovuTisa;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using TemboAI.Core.Communication;
using TemboAI.Core.Subconscious;
using TemboAI.Models;
using TemboAI.Strategies;
using TemboContext.Models;
using TemboShared.Model;
using TemboShared.Service;

namespace TemboAI.Core.Conscious
{
    public class TemboRaw
    {
        public Asset Asset { get; set; }
        public string DurationOfCandle { get; set; }
        public List<int> DurationOfTrade { get; set; }
        private readonly TSC tickServerConnector;
        private List<Candlestick> CoolBars { get; set; }
        private TemboMLNET NetPredictor { get; }
        private List<Position> Positions { get; }
        private List<Position> ClosedPositions { get; }
        private int Boosting { get; }
        private int Leaves { get; }

        private readonly SFractal fractalStrategy= new SFractal();
        private readonly SMACD macdStrategy= new SMACD();
        private readonly SRainbow rainStrategy= new SRainbow();
        private readonly SRSI rsiStrategy= new SRSI();
        private readonly SStoch stochStrategy= new SStoch();
        private readonly SWPR wprStrategy= new SWPR();

        private bool hasNewCandle;
        /*RUN FROM FILE*/
        private bool IsRunningFromFile { get; set; }
        //public double lastKnowCandle;
        private double wins;
        private double winso;
        private double total;
        private double totalo;

        public void GetDurationOfTrades()
        {
            switch (DurationOfCandle)
            {
                case "M1":
                    DurationOfTrade = new List<int> {2, 3, 4, 5};
                    break;
                case "M5":
                    DurationOfTrade = new List<int> {10, 15, 20, 30};
                    break;
                case "M15":
                    DurationOfTrade = new List<int> {30, 45, 60, 75};
                    break;
                case "H1":
                    DurationOfTrade = new List<int> {120, 180, 240, 300};
                    break;
                default:
                    DurationOfTrade = new List<int> {2, 3, 4, 5};
                    break;
            }
        }

        public TemboRaw(string symbol, string durationOfCandle)
        {
            Asset = CN.Db.Asset.GetByNameIndex(symbol);
            DurationOfCandle = durationOfCandle;
            GetDurationOfTrades();
            Boosting = 6000;
            Leaves = 1800;
            tickServerConnector = new TSC();
            tickServerConnector.PropertyChanged += OnReceiveCandles;
            tickServerConnector.Connect();
            CoolBars = new List<Candlestick>();
            NetPredictor = new TemboMLNET
            {
                Key = $"{Asset.Name}.{DurationOfCandle}"
            };
            Positions = new List<Position>();
            ClosedPositions = new List<Position>();
        }
        public TemboRaw(string symbol, string durationOfCandle,int boosting,int leaves)
        {
            Asset = CN.Db.Asset.GetByNameIndex(symbol);
            DurationOfCandle = durationOfCandle;
            GetDurationOfTrades();
            Boosting = boosting;
            Leaves = leaves;
            tickServerConnector = new TSC();
            tickServerConnector.PropertyChanged += OnReceiveCandles;
            tickServerConnector.Connect();
            CoolBars = new List<Candlestick>();
            NetPredictor = new TemboMLNET
            {
                Key = $"{Asset.Name}.{DurationOfCandle}"
            };
            Positions = new List<Position>();
            ClosedPositions = new List<Position>();
        }
        private void OnReceiveCandles(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "RecentCandlesticks":
                    AssimilateNewCandles();
                    if (hasNewCandle)
                    {
                        hasNewCandle = false;
                        AfterNewCandle();
                    }
                    break;
            }
        }
        public void PrepInteligence()
        {
            var positions = CN.Db.Position.GetByAssetAndDuration(Asset.SYSID, DurationOfCandle);
            var training = Convert.ToInt32(positions.Count * 0.8);
            var testing = Convert.ToInt32(positions.Count * 0.2);
            NetPredictor.Init(positions.TakeLast(training), positions.Take(testing).ToList(), Boosting, Leaves);
        }
        public void Run()
        {
            IsRunningFromFile = false;
            //return;
            PrepInteligence();
            var date = DateTime.Now;
            while (true)
            {
                if (!date.IsTimeMatch(DateTime.Now))
                {
                    date = DateTime.Now;
                    TickGatherer();
                }
                Thread.Sleep(500);
            }
        }
        private void TickGatherer()
        {
            dynamic tickRequest = new ExpandoObject();
            tickRequest.type = "Candles";
            tickRequest.timeFrame = "M1";//DurationOfCandle was always present --the one that relearns
            tickRequest.symbol = Asset.Name;
            tickRequest.count = CoolBars.Count < 100 ? GetCount() : 1;
            tickRequest.message = "hello!";
            tickServerConnector.Send(JsonConvert.SerializeObject(tickRequest));
        }

        private int GetCount()
        {
            switch (DurationOfCandle)
            {
                case "M1":
                    return 500;
                case "M5":
                    return 2500;
                case "M15":
                    return 7500;
                case "M30":
                    return 15000;
                case "H1":
                    return 30000;
                case "H4":
                    return 120000;
                default://D1
                    return 720000;

            }
        }
        private void AssimilateNewCandles()
        {
            if (tickServerConnector.RecentCandlesticks == null) return;
            foreach (var dsc in tickServerConnector.RecentCandlesticks)
            {
                hasNewCandle = SubHelpers.MakeCandle(DurationOfCandle,dsc, CoolBars);
                if (CoolBars.Count > 500)
                {
                    CoolBars.RemoveAt(0);
                }
            }
        }

        public void AfterNewCandle()
        {
            ClosePosition();
            if (ClosedPositions.Count > 0)
            {
                foreach (var closedPosition in ClosedPositions)
                {
                    CN.Db.Position.Insert(closedPosition);
                }
                ClosedPositions.Clear();
            }
            if (PlayStrategy(out var position)) return;

            var sentCount = Positions.Count(p => p.Outcome == null);
            if (sentCount <= 0)
            {
                position.Outcome = true;
                var mnetipred = NetPredictor.Predict(position);

                foreach (var dot in DurationOfTrade)
                {
                    var pc = position;
                    pc.EndTime = pc.OpenTime.AddMinutes(dot);
                    pc.DurationOfTrade = dot;
                    var sp = NetPredictor.Predict(pc);
                    if (!sp.PredictedLabel) continue;
                    if (!mnetipred.PredictedLabel)
                    {
                        mnetipred = sp;
                        position = pc;
                    }
                    else if (mnetipred.PredictedLabel && sp.Probability > mnetipred.Probability)
                    {
                        mnetipred = sp;
                        position = pc;
                    }
                }

                position.Identifier = CN.InMemoryId();

                dynamic signal = new ExpandoObject();
                signal.asset = Asset.Name;
                signal.trade = position.Direction ? "Higher" : "Lower";
                signal.endTime = position.EndTime.ToString("HH:mm");
                signal.duration = DurationOfCandle;
                signal.target = position.OpenPrice.ToTemboDecimals();
                signal.netProbability = mnetipred.Probability.ToTemboDecimals().ToString("p");
                signal.netScore = mnetipred.Score.ToTemboDecimals();
                signal.netLabel = mnetipred.PredictedLabel ? "WIN" : "LOSS";
                

                var msg = $"{signal.asset} - {signal.trade}\r\nDuration - {(position.EndTime - position.OpenTime).TotalMinutes.ToInt()} Mins\r\n" +
                          $"EndTime - {signal.endTime}\r\nProbability - {mnetipred.Probability:p}\r\nID - {position.Identifier}";

                

                //send pos to telegram
                if (mnetipred.PredictedLabel  &&  mnetipred.Probability>=0.56) 
                {
                    position.IsSent = true;
                    Thread.Sleep(3000);
                    dynamic signalMsg= new ExpandoObject();
                    signalMsg.type = "TelegramMessage";
                    signalMsg.details = msg;
                    signalMsg.durationOfCandle = DurationOfCandle;
                    tickServerConnector.Send(JsonConvert.SerializeObject(signalMsg));


                    dynamic tickRequest = new ExpandoObject();
                    tickRequest.type = "Trade";
                    tickRequest.identifier = position.Identifier;
                    tickRequest.symbol = Asset.Name;
                    tickRequest.direction = position.Direction ? "BUY" : "SELL";
                    tickRequest.durationOfCandle = DurationOfCandle;
                    tickServerConnector.Send(JsonConvert.SerializeObject(tickRequest));
                    /*if (DurationOfCandle != "M1" && DurationOfCandle != "M5")
                    {
                        
                    }*/
                }
                             
                position.Outcome = null;
                
                Positions.Add(position);
            }
            position.Dispose();
        }

        // readjust your trends
        private bool PlayStrategy(out Position position)
        {
            var candles = CoolBars.TakeLast(80);
            position = null;
            if (candles.Count <= 80) return true;

            //trigger signal
            var rainbow = rainStrategy.GetSignal(candles, 6, 18);

            if (!rainbow.IsBuy && !rainbow.IsSell)
            {
                return true;
            }

            
            var fractalSignal = fractalStrategy.GetSignal(candles, 12);
            var macdSignal = macdStrategy.GetSignal(candles, 6, 12, 18);
            var rsiSignal = rsiStrategy.GetSignal(candles, 12, 78, 24);
            var stochSignal = stochStrategy.GetSignal(candles, 6, 12, 18);
            var wprSignal = wprStrategy.GetSignal(candles, 12, -18, -84);

            /*these are the MA's im talking about*/
            var trendA = rainStrategy.GetTrend(candles, 6, 12, 18);
            var trendB = rainStrategy.GetTrend(candles, 24, 30, 36);
            var trendC = rainStrategy.GetTrend(candles, 42, 48, 54);
            var trendD = rainStrategy.GetSignal(candles, 6, 12, 18, 24, 30, 36, 42);
            var trendE = rainStrategy.GetSignal(candles, 24, 30, 36, 42, 48, 54, 60);
            var trendF = rainStrategy.GetSignal(candles, 42, 48, 54, 60, 66, 72, 78);
            /*make these runtime adjustable settings*/

            var lastCandle = candles.Last();
            var tm = lastCandle.Time;
            position = new Position
            {
                AssetSYSID = Asset.SYSID,
                StrategySYSID = 5,
                DurationOfCandle = DurationOfCandle,
                OpenTime = tm,
                EndTime = DurationOfCandle.GetEndTime(tm),
                OpenPrice = lastCandle.Close,
                Direction = rainbow.IsBuy,
                Fractal = ToInt(fractalSignal),
                Macd = ToInt(macdSignal),
                Rainbow = ToInt(rainbow),
                Rsi = rsiSignal.Series.Last() > 50,
                Stoch = stochSignal.Series.Last() > 78 ? -1 : stochSignal.Series.Last() < 24 ? 1 : 0,
                Wpr = wprSignal.Series.Last() < -18 ? -1 : wprSignal.Series.Last() > -84 ? 1 : 0,
                TrendA = ToInt(trendA),
                TrendB = ToInt(trendB),
                TrendC = ToInt(trendC),
                TrendD = ToInt(trendD),
                TrendE = ToInt(trendE),
                TrendF = ToInt(trendF),
                IsSent = false,
                DateCreated = DateTime.Now,
                LastUpdate = DateTime.Now
            };
            position.DurationOfTrade =(position.EndTime - position.OpenTime).TotalMinutes.ToInt();
            return false;
        }

        private void ClosePosition()
        {
            for (int i = 0; i < Positions.Count; i++)
            {
                
                if (Positions[i].Outcome != null)
                {
                    ClosedPositions.Add(Positions[i]);
                    Positions.RemoveAt(i);
                    i--;
                    continue;
                }
                for (var v = CoolBars.Count - 1; v > 0; v--)
                {
                    var candle = CoolBars[v];
                    if (Positions[i].EndTime > candle.Time)
                    {
                        break;
                    }
                    if (Positions[i].EndTime.IsTimeMatch(candle.Time) || Positions[i].EndTime < candle.Time)
                    {
                        if (Positions[i].Direction && Positions[i].OpenPrice < candle.Close)
                        {
                            Positions[i].Outcome = true;
                            if (Positions[i].IsSent || IsRunningFromFile)
                            {
                                wins++;
                                total++;
                            }
                            Positions[i].ClosePrice = candle.Close;
                            if (Positions[i].IsSent)
                            {
                                //send result
                                dynamic signalMsg = new ExpandoObject();
                                signalMsg.type = "TelegramMessage";
                                signalMsg.details = $"{Asset.Name} - {Positions[i].Identifier}\r\nResult - WIN {DurationOfCandle}\r\nWinRate - {wins / total:p}";
                                signalMsg.durationOfCandle = DurationOfCandle;
                                tickServerConnector.Send(JsonConvert.SerializeObject(signalMsg));
                            }
                            SendCloseCommand(i);
                        }
                        else if (!Positions[i].Direction && Positions[i].OpenPrice > candle.Close)
                        {
                            Positions[i].Outcome = true;
                            if (Positions[i].IsSent || IsRunningFromFile)
                            {
                                wins++;
                                total++;
                            }
                            Positions[i].ClosePrice = candle.Close;
                            if (Positions[i].IsSent)
                            {
                                //send result
                                dynamic signalMsg = new ExpandoObject();
                                signalMsg.type = "TelegramMessage";
                                signalMsg.details = $"{Asset.Name} - {Positions[i].Identifier}\r\nResult - WIN {DurationOfCandle}\r\nWinRate - {wins / total:p}";
                                signalMsg.durationOfCandle = DurationOfCandle;
                                tickServerConnector.Send(JsonConvert.SerializeObject(signalMsg));
                            }
                            SendCloseCommand(i);
                        }
                        else
                        {
                            Positions[i].Outcome = false;
                            if (Positions[i].IsSent || IsRunningFromFile)
                            {
                                total++;
                            }

                            Positions[i].ClosePrice = candle.Close;
                            if (Positions[i].IsSent)
                            {
                                //send result
                                dynamic signalMsg = new ExpandoObject();
                                signalMsg.type = "TelegramMessage";
                                signalMsg.details = $"{Asset.Name} - {Positions[i].Identifier}\r\nResult - LOSS {DurationOfCandle}\r\nWinRate - {wins / total:p}";
                                signalMsg.durationOfCandle = DurationOfCandle;
                                tickServerConnector.Send(JsonConvert.SerializeObject(signalMsg));
                            }

                            SendCloseCommand(i);
                        }
                    }
                }
            }
        }

        private void SendCloseCommand(int i)
        {
            dynamic tickRequest = new ExpandoObject();
            tickRequest.type = "ClosePosition";
            tickRequest.identifier = Positions[i].Identifier;
            tickRequest.symbol = Asset.Name;
            tickRequest.direction = Positions[i].Direction ? "BUY" : "SELL";
            tickServerConnector.Send(JsonConvert.SerializeObject(tickRequest));
        }

        public void RunFromFile(string file, string startDate)
        {
            var coolBarCount = 0;

            IsRunningFromFile = true;
            var symbol = file.Split('.')[0].Split('\\').Last();
            var last = startDate.ToDateTime();
            $"{symbol}.{DurationOfCandle} streaming started".Log(3);
            var lastp = CN.Db.Position.GetByAssetAndDuration(Asset.SYSID, DurationOfCandle, 10).OrderByDescending(d => d.OpenTime).FirstOrDefault();
            if (lastp != null)
            {
                last = lastp.OpenTime;
            }

            foreach (var dOT in DurationOfTrade)
            {
                winso = totalo = 0;
                CoolBars = new List<Candlestick>();
                var fsSource = new FileStream(file, FileMode.Open, FileAccess.Read);
                var sr = new StreamReader(fsSource);
                var hasSeconds = false;
                var ischeck = false;
                DateTime prev = DateTime.Now;
                var start = DateTime.Now;
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();
                    if (line == null) continue;
                    if (line.Trim() == "") continue;
                    if (line.Contains("<")) continue;
                    var columns = line.Split(',');
                    if (columns.Length == 1)
                    {
                        columns = columns[0].Split('\t');
                    }

                    var date = columns[0] + " " + columns[1];
                    if (!ischeck)
                    {
                        date.HasSeconds(out hasSeconds);
                        ischeck = true;
                    }

                    var tickTime = DateTime.ParseExact(date, hasSeconds ? "yyyy.MM.dd HH:mm:ss" : "yyyy.MM.dd HH:mm", CultureInfo.InvariantCulture);
                    if (tickTime <= last)
                    {
                        continue;
                    }

                    if (prev.Month != tickTime.Month)
                    {
                        if (wins > 0)
                        {
                            $"{symbol}.{DurationOfCandle}.{dOT} Timeline: {prev:MMMM yyyy}, TimeSpent: {Math.Round((DateTime.Now - start).TotalMinutes, 4)}".Log(4);
                            double xp = wins / total;
                            $"ITM: {wins} TOTAL: {total} WINRATE: {Math.Round(xp, 2):p}".Log(6);
                            winso += wins;
                            totalo += total;
                            xp = winso / totalo;
                            $"Overall ITM: {winso} Overall TOTAL: {totalo} Overall WINRATE: {Math.Round(xp, 2):p}".Log(5, true);
                            $"Flash Saving {ClosedPositions.Count} to database".Log(2, true);
                            foreach (var closedposition in ClosedPositions)
                            {
                                CN.Db.Position.Persist(closedposition);
                            }

                            ClosedPositions.Clear();
                            $"Flash Saving complete".Log(2, true);
                        }

                        wins = total = 0;
                        start = DateTime.Now;
                        prev = tickTime;
                    }

                    var stick = new Candlestick(symbol, double.Parse(columns[2]), double.Parse(columns[3]), double.Parse(columns[4]), double.Parse(columns[5]), tickTime, DurationOfCandle, int.Parse(columns[6]));
                    SubHelpers.MakeCandle(DurationOfCandle, stick, CoolBars);

                    if (CoolBars.Count >= 300)
                    {
                        CoolBars.RemoveAt(0);
                    }

                    ClosePosition();
                    

                    if (PlayStrategy(out var position)) continue;
                    var sentCount = Positions.Count(p => p.Outcome == null && p.Direction == position.Direction);
                    if (sentCount <= 0)
                    {
                        position.DurationOfTrade = dOT;
                        position.EndTime = CoolBars.Last().Time.AddMinutes(dOT);
                        Positions.Add(position);
                    }

                    /*if (CoolBars.Count > coolBarCount)
                    {
                        coolBarCount = CoolBars.Count;

                    }*/

                }

                if (ClosedPositions.Count > 0)
                {
                    $"Flushing {ClosedPositions.Count} to database".Log(2, true);
                    foreach (var position in ClosedPositions)
                    {
                        CN.Db.Position.Persist(position);
                    }

                    ClosedPositions.Clear();
                }

            }

            $"{Asset.Name}.{DurationOfCandle} simulation completed".Log(3);
        }

        public static int ToInt(IndicatorSignal signal)
        {
            return signal.IsBuy ? 1 : signal.IsSell ? -1 : 0;
        }
    }
}
