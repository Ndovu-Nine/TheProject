using MtApi5;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TemboShared.Model;

namespace TemboShared.Service
{
    public static class Extension
    {
        public static List<double> Highs(this List<Candlestick> sticks)
        {
            return sticks.Select(atlasCandlestick => atlasCandlestick.High).ToList();
        }

        public static List<double> Lows(this List<Candlestick> sticks)
        {
            return sticks.Select(atlasCandlestick => atlasCandlestick.Low).ToList();
        }

        public static List<double> Opens(this List<Candlestick> sticks)
        {
            return sticks.Select(atlasCandlestick => atlasCandlestick.Open).ToList();
        }

        public static List<double> Closes(this List<Candlestick> sticks)
        {
            return sticks.Select(atlasCandlestick => atlasCandlestick.Close).ToList();
        }

        public static List<DateTime> Times(this List<Candlestick> sticks)
        {
            return sticks.Select(atlasCandlestick => atlasCandlestick.Time).ToList();
        }

        public static List<double> Volumes(this List<Candlestick> sticks)
        {
            return sticks.Select(atlasCandlestick => atlasCandlestick.Volume).ToList();
        }

        public static List<double> Normalize(this List<double> source, double scaleMin = 0.0, double scaleMax = 1.0)
        {
            var num = source.Max();
            var valueMin = source.Min();
            var valueRange = num - valueMin;
            var scaleRange = scaleMax - scaleMin;
            return source.Select(i => scaleRange * (i - valueMin) / valueRange + scaleMin).ToList();
        }
        public static bool IsTouch(double line, double price, int acceptable = 2)
        {
            var num = (line - price) / line * 100.0;
            if (num < 0.0)num *= -1.0;
            return num < acceptable;
        }
        public static List<double> Normalize(this IEnumerable<double> source, double scaleMin = 0.0,double scaleMax = 1.0)
        {
            var numArray = source as double[] ?? source.ToArray();
            var num = numArray.Max();
            var valueMin = numArray.Min();
            var valueRange = num - valueMin;
            var scaleRange = scaleMax - scaleMin;
            return numArray.Select(i => scaleRange * (i - valueMin) / valueRange + scaleMin).ToList();
        } 
        public static TimeSeries ToTimeSeries(this List<Candlestick> sticks)
        {
            return new TimeSeries()
            {
                Open = sticks.Opens().ToArray(),
                High = sticks.Highs().ToArray(),
                Low = sticks.Lows().ToArray(),
                Close = sticks.Closes().ToArray(),
                Time = sticks.Times().ToArray()
            };
        }
        /// <summary>
        /// True is day,Hour and minute match
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="mode">0 for minute, 1 for hour,2 for h4, 3 for day, 4 for week, 5 for month</param>
        /// <returns></returns>
        public static bool IsTimeMatch(this DateTime d1, DateTime d2, int mode=0)
        {
            switch (mode)
            {
                case 0:
                    return d1.Day == d2.Day && d1.Hour == d2.Hour && d1.Minute == d2.Minute;
                case 1:
                    return d1.Day == d2.Day && d1.Hour == d2.Hour;
                case 2:
                    return d1.Day == d2.Day && (d1.Hour + 4) > d2.Hour;
                case 3:
                    return d1.Day == d2.Day;
                case 5:
                    return d1.Month == d2.Month;
                default:
                {
                    var lastDayOfWeek = d1.StartOfWeek(d1.DayOfWeek).AddDays(7);
                    return lastDayOfWeek >= d2;
                }
            }
        }

        

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj,new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Serialize,Formatting=Formatting.Indented });
        }

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static Candlestick FromMQLRate(this MqlRates rate, string symbol)
        {
            return new Candlestick(symbol, rate.open, rate.high, rate.low, rate.close, rate.time, (int)rate.tick_volume);
        }
        public static List<Candlestick> FromMQLRate(this MqlRates[] rate, string symbol)
        {
            var list = new List<Candlestick>();
            foreach (var sd in rate)
            {
                list.Add(new Candlestick(symbol, sd.open, sd.high, sd.low, sd.close, sd.time, (int)sd.tick_volume));
            }
            return list;
        }

        public static string ToTimer(this double timeSpanInSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(timeSpanInSeconds);
            if (timeSpan.Days > 0)
            {
                var strArray = new string[7];
                var num = timeSpan.Days;
                strArray[0] = num.ToString("00");
                strArray[1] = ":";
                num = timeSpan.Hours;
                strArray[2] = num.ToString("00");
                strArray[3] = ":";
                num = timeSpan.Minutes;
                strArray[4] = num.ToString("00");
                strArray[5] = ":";
                num = timeSpan.Seconds;
                strArray[6] = num.ToString("00");
                return string.Concat(strArray);
            }
            if (timeSpan.Hours > 0)
            {
                var strArray = new string[5];
                var num = timeSpan.Hours;
                strArray[0] = num.ToString("00");
                strArray[1] = ":";
                num = timeSpan.Minutes;
                strArray[2] = num.ToString("00");
                strArray[3] = ":";
                num = timeSpan.Seconds;
                strArray[4] = num.ToString("00");
                return string.Concat(strArray);
            }
            if (timeSpan.Minutes > 0)
                return timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
            return timeSpan.Seconds.ToString("00");
        }

        public static double[] FromInt(this int[] lemon)
        {
            var numArray = new double[lemon.Length];
            for (var index = 0; index < lemon.Length; ++index)
                numArray[index] = lemon[index];
            return numArray;
        }
        public static DateTime ToDateTime(this double unixTimeStamp, int addHours = 0)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToUniversalTime().AddHours(addHours);
        }

        public static DateTime ToDateTime(this int unixTimeStamp, int addHours = 0)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToUniversalTime().AddHours(addHours);
        }
        /*public static DateTime GetEndTime(this string duration)
        {
            DateTime now = DateTime.Now;
            string str = duration;
            if (!(str == "5 Mins"))
            {
                if (!(str == "15 Mins"))
                {
                    if (str == "30 Mins")
                    {
                        DateTime dateTime = now.AddMinutes(30.0);
                        while (dateTime.Minute != 0 && (uint)(dateTime.Minute % 30) > 0U)
                            dateTime = dateTime.AddMinutes(1.0);
                        TimeSpan timeSpan = new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
                        return dateTime.Date + timeSpan;
                    }
                    DateTime dateTime1 = now.AddMinutes(2.0);
                    TimeSpan timeSpan1 = new TimeSpan(dateTime1.Hour, dateTime1.Minute, 0);
                    return dateTime1.Date + timeSpan1;
                }
                DateTime dateTime2 = now.AddMinutes(15.0);
                while (dateTime2.Minute != 0 && (uint)(dateTime2.Minute % 15) > 0U)
                    dateTime2 = dateTime2.AddMinutes(1.0);
                TimeSpan timeSpan2 = new TimeSpan(dateTime2.Hour, dateTime2.Minute, 0);
                return dateTime2.Date + timeSpan2;
            }
            DateTime dateTime3 = now.AddMinutes(5.0);
            TimeSpan timeSpan3 = new TimeSpan(dateTime3.Hour, dateTime3.Minute, 0);
            return dateTime3.Date + timeSpan3;
        }*/

        public static DateTime GetEndTime(this string duration, DateTime s)
        {
            var str = duration;
            if (str == "M30")
            {
                s = s.AddMinutes(90.0);
                if (s.Minute < 30)
                {
                    var timeSpanA = new TimeSpan(s.Hour, 0, 0);
                    return s.Date + timeSpanA;
                }
                else
                {
                    while (s.Minute != 0)
                        s = s.AddMinutes(1.0);
                    var timeSpan = new TimeSpan(s.Hour, s.Minute, 0);
                    return s.Date + timeSpan;
                }
            }
            if (str == "M15")
            {
                s = s.AddMinutes(45.0);
                if (s.Minute < 15)
                {
                    var timeSpanA = new TimeSpan(s.Hour, 0, 0);
                    return s.Date + timeSpanA;
                }
                else
                {
                    while (s.Minute != 0)
                        s = s.AddMinutes(1.0);
                    var timeSpan = new TimeSpan(s.Hour, s.Minute, 0);
                    return s.Date + timeSpan;
                }
            }
            if (str == "M5")
            {
                s = s.AddMinutes(15.0);
                while (s.Minute != 0 && (uint)(s.Minute % 15) > 0U)
                    s = s.AddMinutes(1.0);
                var timeSpan3 = new TimeSpan(s.Hour, s.Minute, 0);
                return s.Date + timeSpan3;
            }
            if (str == "M1")
            {
                s = s.AddMinutes(5.0);
                //TimeSpan timeSpan3 = new TimeSpan(s.Hour, s.Minute, 0);
                return s;
            }
            if (str == "H1")
            {
                s = s.AddHours(3);
                if (s.Minute < 30)
                {
                    var timeSpanA = new TimeSpan(s.Hour, 0, 0);
                    return s.Date + timeSpanA;
                }
                else
                {
                    while (s.Minute != 0)
                        s = s.AddMinutes(1.0);
                    var timeSpan = new TimeSpan(s.Hour, s.Minute, 0);
                    return s.Date + timeSpan;
                }
            }
            if (str == "H4")
            {
                s = s.AddHours(12);
                if (s.Minute < 30)
                {
                    var timeSpanA = new TimeSpan(s.Hour, 0, 0);
                    return s.Date + timeSpanA;
                }
                else
                {
                    while (s.Minute != 0)
                        s = s.AddMinutes(1.0);
                    var timeSpan = new TimeSpan(s.Hour, s.Minute, 0);
                    return s.Date + timeSpan;
                }
            }
            if (str == "D1")
            {
                s = s.AddDays(3);
                if (s.Minute < 30)
                {
                    var timeSpanA = new TimeSpan(s.Hour, 0, 0);
                    return s.Date + timeSpanA;
                }
                else
                {
                    while (s.Minute != 0)
                        s = s.AddMinutes(1.0);
                    var timeSpan = new TimeSpan(s.Hour, s.Minute, 0);
                    return s.Date + timeSpan;
                }
            }
            if (str == "W1")
            {
                s = s.AddDays(15);
                if (s.Minute < 30)
                {
                    var timeSpanA = new TimeSpan(s.Hour, 0, 0);
                    return s.Date + timeSpanA;
                }
                else
                {
                    while (s.Minute != 0)
                        s = s.AddMinutes(1.0);
                    var timeSpan = new TimeSpan(s.Hour, s.Minute, 0);
                    return s.Date + timeSpan;
                }
            }
            if (str == "MN1")
            {
                s = s.AddDays(60);
                if (s.Minute < 30)
                {
                    var timeSpanA = new TimeSpan(s.Hour, 0, 0);
                    return s.Date + timeSpanA;
                }
                else
                {
                    while (s.Minute != 0)
                        s = s.AddMinutes(1.0);
                    var timeSpan = new TimeSpan(s.Hour, s.Minute, 0);
                    return s.Date + timeSpan;
                }
            }
            s = s.AddMinutes(2.0);
            var timeSpan1 = new TimeSpan(s.Hour, s.Minute, 0);
            return s.Date + timeSpan1;
        }
        /// <summary>
        /// Returns N number of elements starting from last
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int n)
        {
            var takeLast = source.ToList();
            var count = takeLast.Count();
            if (n >= count)
            {
                return takeLast;
            }
            var countList = count - n;
            var objList = new List<T>(n);
            for(var i = countList; i < count; i++)
            {
                objList.Add(takeLast.ElementAt(i));
            }
            return objList;
            //return source.ToList().TakeLastN(n);           
            /*T[] objArray = source as T[] ?? source.ToArray<T>();
            return ((IEnumerable<T>)objArray).Skip<T>(System.Math.Max(0, ((IEnumerable<T>)objArray).Count<T>() - n));*/
        }
        /// <summary>
        /// Returns  N number of elements starting from last
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<T> TakeLast<T>(this List<T> source, int n)
        {
            var count = source.Count;
            if (n >= count)
                return source;
            var objList = new List<T>(n);
            for (var index = n; index >= 0; --index)
                objList.Add(source[count - 1 - index]);
            return objList;
        }
        /// <summary>
        /// Returns specified elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="start">index at which to start</param>
        /// <param name="count">elements to pick from start</param>
        /// <param name="reverse">start at 0 or start at last element</param>
        /// <returns></returns>
        public static List<T> Take<T>(this List<T> source, int start, int count, bool reverse=false)
        {
            var gdf = new List<T>();
            if (!reverse)
            {
                if (source.Count <= (start + count))
                {
                    return source;
                }                
                for (var i = start; i < count; i++)
                {
                    gdf.Add(source[i]);
                }
                return gdf;
            }
            else
            {
                if ((start - count) <= 0)
                {
                    return source.Take(count).ToList();
                }
                else
                {
                    var con = start - count;
                    for (var i =con ; i <start; i++)
                    {
                        gdf.Add(source[i]);
                    }
                    return gdf;
                }
            }
        }
        /// <summary>
        /// Removes top elements from a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static List<T> Trim<T>(this List<T> source, int max)
        {
            while (source.Count > max)
                source.RemoveAt(0);
            return source;
        }
        
        /// <summary>
        /// Takes values starting from last
        /// </summary>
        /// <param name="values"></param>
        /// <param name="value">the minimum value you need picked</param>
        /// <param name="distance">how far back you want to go</param>
        /// <returns></returns>
        public static double TakeLastGreatorThan(this double[] values, double minima = 0.0, int distance = 0)
        {
            for (var index = values.Length - 1; index > 0; --index)
            {
                if (values[index] > minima)
                {
                    if (index >= distance)
                        return values[index - distance];
                    return 0.0;
                }
            }
            return 0.0;
        }
        /// <summary>
        /// Takes values starting from last
        /// </summary>
        /// <param name="values"></param>
        /// <param name="value">the minimum value you need picked</param>
        /// <param name="distance">how far back you want to go</param>
        /// <returns></returns>
        public static double TakeLastGreatorThan(this decimal[] values, double minima = 0.0, int distance = 0)
        {
            for (var index = values.Length - 1; index > 0; --index)
            {
                if ((double)values[index] > minima)
                {
                    if (index >= distance)
                        return (double)values[index - distance];
                    return 0.0;
                }
            }
            return 0.0;
        }
        /// <summary>
        /// Takes values starting from last
        /// </summary>
        /// <param name="values"></param>
        /// <param name="value">the minimum value you need picked</param>
        /// <param name="distance">how far back you want to go</param>
        /// <returns></returns>
        public static double TakeLastGreatorThan(this List<double> values, double minima = 0.0, int distance = 0)
        {
            var num1 = values.Count - 1;
            var num2 = num1 - distance;
            for (var index = num1; index > 0; --index)
            {
                if (values[index] > minima)
                {
                    if (index == num2)
                        return values[index];
                    return 0.0;
                }
            }
            return 0.0;
        }

        public static double? Median<TColl, TValue>(this IEnumerable<TColl> source, Func<TColl, TValue> selector)
        {
            return source.Select(selector).Median();
        }

        public static double? Median<T>(this IEnumerable<T> source)
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
                source = source.Where(x => x != null).ToList();
            var enumerable = source.ToList();
            var num = enumerable.Count();
            if (num == 0)
                return new double?();
            source = enumerable.OrderBy(n => n);
            var index = num / 2;
            if (num % 2 == 0)
                return (Convert.ToDouble(source.ElementAt(index - 1)) + Convert.ToDouble(source.ElementAt(index))) / 2.0;
            return Convert.ToDouble(source.ElementAt(index));
        }
        /// <summary>
        /// Returns unique elements
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var source1 in source.Where(element => seenKeys.Add(keySelector(element))))
            {
                var element = source1;
                yield return element;
            }
        }
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var num = dt.DayOfWeek - startOfWeek;
            if (num < 0)
                num += 7;
            return dt.AddDays(-1 * num).Date;
        }
    }
}
