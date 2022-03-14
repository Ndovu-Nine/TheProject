using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TemboContext.Models;
using TemboShared.Model;
using TemboShared.Service;

namespace TemboAI.Models
{
    public static class SubHelpers
    {
        public static PositionFeature FromPosition(this Position position)
        {
            var feature = new PositionFeature
            {
                DayOfWeek = (int)position.OpenTime.DayOfWeek,
                Hour = position.OpenTime.Hour,
                Direction = position.Direction.ToInt(),
                DurationOfTrade = position.DurationOfTrade,
                Fractal = position.Fractal,
                Macd = position.Macd,
                Rainbow = position.Rainbow,
                Rsi = position.Rsi.ToInt(),
                Stoch = position.Stoch,
                Wpr = position.Wpr,
                TrendA = position.TrendA,
                TrendB = position.TrendB,
                TrendC = position.TrendC,
                TrendD = position.TrendD,
                TrendE = position.TrendE,
                TrendF = position.TrendF,
                Outcome = position.Outcome ?? false
            };
            return feature;
        }
        public static List<PositionFeature> FromPosition(this List<Position> positions){
            var features = new List<PositionFeature>();
            foreach(var pos in positions)
            {
                features.Add(pos.FromPosition());
            }
            return features;
        }
        public static void HasSeconds(this string date, out bool hasSeconds)
        {
            try
            {
                // ReSharper disable once UnusedVariable
                var dateTime = DateTime.ParseExact(date, "yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture);
                hasSeconds = true;
            }
            catch (Exception)
            {
                hasSeconds = false;
            }
        }

        public static bool MakeCandle(string duration, Candlestick stick, List<Candlestick> coolBars)
        {
            if (coolBars.Count <= 0)
            {
                coolBars.Add(stick);
                return true;
            }

            //return false;//fail on purpose

            /*all candles are M1??*/
            switch (duration)
            {
                case "M1":
                    if (!coolBars.Exists(stick,CollectionMode.M1))
                    {
                        coolBars.Add(stick);
                    }
                    return true;
                case "M5":
                    return CandleByDuration( 5, stick, coolBars);
                case "M15":
                    return CandleByDuration( 15, stick, coolBars);
                case "M30":
                    return CandleByDuration( 30, stick, coolBars);
                case "H1":
                    return CandleByCollectionMode(CollectionMode.H1, stick, coolBars);
                case "H4":
                    return CandleByCollectionMode(CollectionMode.H4, stick, coolBars);
                case "D1":
                    return CandleByCollectionMode(CollectionMode.D1, stick, coolBars);
                case "W1":
                    return CandleByCollectionMode(CollectionMode.W1, stick, coolBars);
                case "MN1":
                    return CandleByCollectionMode(CollectionMode.MN1, stick, coolBars);
            }
            return false;
        }
        //[Obsolete]
        private static bool CandleByDuration(int duration, Candlestick stick,  List<Candlestick> coolBars)
        {
            if (stick.Time.Minute % duration == 0 || stick.Time.Minute==0)
            {
                coolBars.Add(stick);
                return true;
            }

            coolBars[^1].Update(stick);
            return true;
        }
        //[Obsolete]
        private static bool CandleByCollectionMode(CollectionMode mode, Candlestick stick,  List<Candlestick> coolBars)
        {
            if (coolBars.Exists(stick, mode)) return false;
            coolBars.Add(stick);
            return true;
        }
        /// <summary>
        /// Only checks the last 5 bars
        /// </summary>
        /// <param name="bars"></param>
        /// <param name="stick"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool Exists(this List<Candlestick> bars, Candlestick stick, CollectionMode mode)
        {
            for (var i = bars.Count - 1; i >= bars.Count - 5; i--)
            {
                if(i<0)continue;
                if (!bars[i].Time.IsTimeMatch(stick.Time, mode)) continue;
                bars[i].Update(stick, 1);
                return true;
            }
            return false;
        }
        public static bool IsTimeMatch(this DateTime d1, DateTime d2, CollectionMode mode)
        {
            switch (mode)
            {
                case CollectionMode.M1:
                    return d1.Day == d2.Day && d1.Hour == d2.Hour && d1.Minute == d2.Minute;
                case CollectionMode.H1:
                    return d1.Day == d2.Day && d1.Hour == d2.Hour;
                case CollectionMode.H4:
                    return d1.Day == d2.Day && (d1.Hour + 4) > d2.Hour;
                case CollectionMode.D1:
                    return d1.Day == d2.Day;
                case CollectionMode.W1:
                    var lastDayOfWeek = d1.StartOfWeek(d1.DayOfWeek).AddDays(7);
                    return lastDayOfWeek >= d2;
                case CollectionMode.MN1:
                    return d1.Month == d2.Month;
                default:
                {
                    return d1.Day == d2.Day && d1.Hour == d2.Hour && d1.Minute == d2.Minute;
                }
            }
        }
        public static double[] PositionToVector(this Position position)
        {
            return new[]
            {
                (double) position.OpenTime.DayOfWeek,
                position.OpenTime.Hour,
                position.Direction.ToInt(),
                position.DurationOfTrade,
                position.Fractal,
                position.Macd,
                position.Rainbow,
                position.Rsi.ToInt(),
                position.Stoch,
                position.Wpr,
                position.TrendA,
                position.TrendB,
                position.TrendC,
                position.TrendD,
                position.TrendE,
                position.TrendF,
            };
        }
        public static double[][] VectorFromPosition(this List<Position> positions)
        {
            double[][] mca= new double [positions.Count][];
            for(var i = 0; i < mca.Length; i++)
            {
                mca[i] = PositionToVector(positions[i]);
            }
            return mca;
        }
        public static double[] Outcome(this Position position)
        {
            return new[]{
                        position.Outcome.Value?1.0:0.0
                    };
        }
        public static double[] Outcome(this List<Position> positions)
        {
            double[] mca = new double[positions.Count];
            for (var i = 0; i < mca.Length; i++)
            {
                mca[i] = positions[i].Outcome.Value?1.0:0.0;
            }
            return mca;
        }
        public static double[] Direction(this List<Position> positions)
        {
            double[] mca = new double[positions.Count];
            for (var i = 0; i < mca.Length; i++)
            {
                if (positions[i].Outcome.Value)
                {
                    //0 is buy, 1 is sell remember?
                    mca[i] = positions[i].Direction ? 0.0 : 1.0;
                }
                else
                {
                    mca[i] = positions[i].Direction ? 1.0 : 0.0;
                }
            }
            return mca;
        }
        
        public static List<TANet.Contracts.Models.Candle> FromCandles(this List<Candlestick> source)
        {
            var tlis = new List<TANet.Contracts.Models.Candle>();
            foreach(var stick in source)
            {
                var tl = new TANet.Contracts.Models.Candle
                {
                    Close = decimal.Parse(stick.Close.ToString(CultureInfo.InvariantCulture)),
                    CloseTime = stick.Time,
                    High = decimal.Parse(stick.High.ToString(CultureInfo.InvariantCulture)),
                    Low = decimal.Parse(stick.Low.ToString(CultureInfo.InvariantCulture)),
                    Open = decimal.Parse(stick.Open.ToString(CultureInfo.InvariantCulture)),
                    OpenTime = stick.Time,
                    Volume = decimal.Parse(stick.Volume.ToString(CultureInfo.InvariantCulture))
                };
                tlis.Add(tl);
            }
            return tlis;
        }
        /// <summary>
        /// 6 decimal places
        /// </summary>
        /// <param name="dbl"></param>
        /// <returns></returns>
        public static double ToTemboDecimals(this double dbl)
        {
            return Math.Round(dbl, 6);
        }
        /// <summary>
        /// 4 decimal places
        /// </summary>
        /// <param name="dbl"></param>
        /// <returns></returns>
        public static double ToTemboDecimals(this float dbl)
        {
            return Math.Round(dbl, 4);
        }
        public static List<double> Normalize(this List<double> source, double scaleMin = 0.0, double scaleMax = 1.0)
        {
            double max = source.Max();
            double min = source.Min();
            double valueRange = max - min;
            double scaleRange = scaleMax - scaleMin;
            return source.Select((i => scaleRange * (i - min) / valueRange + scaleMin)).ToList();
        }

        public static int GetStrength(this bool buy, bool sell)
        {
            if (buy)
            {
                return 1;
            }
            if (sell)
            {
                return 0;
            }
            return 2;
        }

        #region Useful Trash

        /*public static int[] TimesAdjusted(this List<Position> positions)
        {
            int[] mca = new int[positions.Count];
            for (var i = 0; i < mca.Length; i++)
            {
                mca[i] = positions[i].TimesAdjusted;
            }
            return mca;
        }

        public static bool MakeCandle(Candlestick stick,List<Candlestick> coolBars)
        {
            if (coolBars.Count <= 0)
            {
                coolBars.Add(stick);
                return true;
            }
            switch (stick.DurationOfCandle)
            {
                case "M1":
                    if (!coolBars.Exists(stick))
                    {
                        coolBars.Add(stick);
                    }
                    return true;
                case "M5":
                    return CandleByDuration(5, stick, coolBars);
                case "M15":
                    return CandleByDuration(15, stick, coolBars);
                case "M30":
                    return CandleByDuration(30, stick, coolBars);
                case "H1":
                    return CandleByCollectionMode(CollectionMode.H1, stick, coolBars);
                case "H4":
                    return CandleByCollectionMode(CollectionMode.H4, stick, coolBars);
                case "D1":
                    return CandleByCollectionMode(CollectionMode.D1, stick, coolBars);
                case "W1":
                    return CandleByCollectionMode(CollectionMode.W1, stick, coolBars);
                case "MN1":
                    return CandleByCollectionMode(CollectionMode.MN1, stick, coolBars);
            }
            return false;
        }

        public static bool MakeCandle(Candlestick stick, Dictionary<long, Candlestick> coolBars, Dictionary<string, List<long>> cache)
        {
            var key = $"{stick.Symbol}.{stick.DurationOfCandle}";
            var cbs = cache[key];
            if (cbs.Count <= 0)
            {
                coolBars.Add(stick.Id, stick);
                cache[key].Add(stick.Id);
                return true;
            }
            var lastCandleKey = cbs[cbs.Count - 1];
            var last = coolBars[lastCandleKey];

            switch (stick.DurationOfCandle)
            {
                case "M1":
                    coolBars.Add(stick.Id, stick);
                    cache[key].Add(stick.Id);
                    return true;
                case "M5":
                    return MakeCCCandle(stick, last, 5,coolBars,cache);
                case "M15":
                    return MakeCCCandle(stick, last, 15, coolBars, cache);
                case "M30":
                    return MakeCCCandle(stick, last, 30, coolBars, cache);
                case "H1":
                    return MakeCCandle(stick, last, 1, coolBars, cache);
                case "H4":
                    return MakeCCandle(stick, last, 2, coolBars, cache);
                case "D1":
                    return MakeCCandle(stick, last, 3, coolBars, cache);
                case "W1":
                    return MakeCCandle(stick, last, 4, coolBars, cache);
                case "MN1":
                    return MakeCCandle(stick, last, 5, coolBars, cache);
            }
            return false;
        }*/

        /*private static bool MakeCCCandle(Candlestick stick, Candlestick last, int divisor, Dictionary<long, Candlestick> coolBars, Dictionary<string, List<long>> cache)
        {
            if (stick.Time.Minute % divisor == 0)
            {
                var key = $"{stick.Symbol}.{stick.DurationOfCandle}";
                coolBars.Add(stick.Id, stick);                
                cache[key].Add(stick.Id);
                return true;
            }
            else
            {
                coolBars[last.Id].Update(stick, 1);
                return false;
            }
        }
         private static bool MakeCCandle(Candlestick stick, Candlestick last, int mode, Dictionary<long, Candlestick> coolBars, Dictionary<string, List<long>> cache)
        {
            if (last.Time.IsTimeMatch(stick.Time, mode))
            {
                coolBars[last.Id].Update(stick, 1);
                return false;
            }
            else
            {
                var key = $"{stick.Symbol}.{stick.DurationOfCandle}";
                coolBars.Add(stick.Id, stick);                
                cache[key].Add(stick.Id);
                return true;
            }
        }*/
        #endregion
    }
}
