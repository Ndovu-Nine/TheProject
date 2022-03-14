using System;
using System.Globalization;

namespace NdovuTisa
{
    public static class Chronos
    {
        public static double ToUnixTimestamp(this DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
        public static DateTime ToDateTime(this double unixTimeStamp, int addDays = 0)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime.AddDays(addDays);
        }
        public static string ToMonth(this double unixTimeStamp, int addDays = 0)
        {
            return unixTimeStamp.ToDateTime(addDays).ToString("MMMM");
        }
        /// <summary>
        /// turns a tring into  DateTime
        /// </summary>
        /// <param name="date">{dd/MM/yyyy, dd/M/yyyy, d/M/yyyy, d/MM/yyyy,yyyy-MM-dd,yyyy-MM-d,yyyy-M-d}</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string date)
        {
            try
            {
                if (date == null)
                {
                    return DateTime.Now;
                }
                if (date == "" || date.Length <= 0)
                {
                    return DateTime.Now;
                }
                return DateTime.ParseExact(date, new[] {"yyyy-MM-dd HH:mm:ss", "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "yyyy-MM-dd", "yyyy-MM-d", "dd-MM-yyyy", "yyyy-M-d", "DD MMMM YYYY" }, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {
                return DateTime.Now;
            }
        }
        /// <summary>
        /// Full name of month
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToMonth(this DateTime date)
        {
            return date.ToString("MMMM");
        }
        public static int ToYear(this double unixTimeStamp, int addDays = 0)
        {
            return unixTimeStamp.ToDateTime(addDays).Year;
        }
        /// <summary>
        /// Turns seconds into the format dd:hh:mm:ss (day:hours:minutes:seconds)
        /// </summary>
        /// <param name="timeSpanInSeconds"></param>
        /// <returns></returns>
        public static string ToTimer(this double timeSpanInSeconds)
        {
            var state = TimeSpan.FromSeconds(timeSpanInSeconds);
            if (state.Days > 0)
            {
                return state.Days.ToString("00") + ":" + state.Hours.ToString("00") + ":" + state.Minutes.ToString("00") + ":" + state.Seconds.ToString("00");
            }
            else if (state.Hours > 0)
            {
                return state.Hours.ToString("00") + ":" + state.Minutes.ToString("00") + ":" + state.Seconds.ToString("00");
            }
            else if (state.Minutes > 0)
            {
                return state.Minutes.ToString("00") + ":" + state.Seconds.ToString("00");
            }
            else
            {
                return state.Seconds.ToString("00");
            }
        }
        public static DateTime FirstDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }
        /// <summary>
        /// Total days in a month
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }

        public static DateTime LastDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.DaysInMonth());
        }
    }
}
