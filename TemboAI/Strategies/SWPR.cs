using System;
using System.Collections.Generic;
using System.Linq;
using TemboAI.Models;
using TemboShared.Model;
using TemboShared.Service;
namespace TemboAI.Strategies
{
    public  class SWPR:IStrategy
    {
        /// <summary>
        /// Calculate WPR based on period provided
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="period"></param>
        /// <param name="overbought"></param>
        /// <param name="oversold"></param>
        /// <returns></returns>
        public IndicatorSignal GetSignal(List<Candlestick> candles, int period, int overbought, int oversold)
        {
            var wprDoubles = Indicator.WPR.Calculate(candles.Closes().ToArray(), candles.ToTimeSeries(), period);
            double currentWPRValue = wprDoubles[^1];// ((IEnumerable<double>)wprDoubles).Last<double>();
            double previousWPRValue = wprDoubles[^4];
            if (Math.Abs(currentWPRValue) < CN.Threshold || Math.Abs(currentWPRValue - (-100)) < CN.Threshold)
            {
                var wprhd = new IndicatorSignal
                {
                    IsBuy = false,
                    IsSell = false,
                    Series = wprDoubles
                };
                return wprhd;
            }
            var wprh = new IndicatorSignal
            {
                IsBuy = false,
                IsSell = false,
                Series = wprDoubles
            };
            if (currentWPRValue > oversold && previousWPRValue <= oversold)
            {
                wprh.IsBuy = true;
            }
            if (currentWPRValue < overbought && previousWPRValue >= overbought)
            {
                wprh.IsSell = true;
            }

            return wprh;
        }

        #region NON WPR

        public IndicatorSignal GetSignal(List<Candlestick> candles, int period)
        {
            throw new NotImplementedException();
        }

        public IndicatorSignal GetSignal(List<Candlestick> candles, int period, double deviationUp, double deviationDown)
        {
            throw new NotImplementedException();
        }

        public IndicatorSignal GetSignal(List<Candlestick> candles, int setting1, int setting2)
        {
            throw new NotImplementedException();
        }

        public IndicatorSignal GetSignal(List<Candlestick> candles, double setting1, double setting2)
        {
            throw new NotImplementedException();
        }

        public IndicatorSignal GetSignal(List<Candlestick> candles, int setting1, int setting2, int setting3, int setting4, int setting5, int setting6, int setting7)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculate WPR based on price provided
        /// </summary>
        /// <param name="period"></param>
        /// <param name="overbought"></param>
        /// <param name="oversold"></param>
        /// <param name="candles"></param>
        /// <returns></returns>
        [Obsolete("Use GetSignal")]
        public static SResult Get(List<Candlestick> candles, int period, int overbought = -10, int oversold = -90)
        {
            var wprDoubles = Indicator.WPR.Calculate(candles.Closes().ToArray(), candles.ToTimeSeries(), period);
            double currentWPRValue = wprDoubles[^1];// ((IEnumerable<double>)wprDoubles).Last<double>();
            if (Math.Abs(currentWPRValue) < CN.Threshold || Math.Abs(currentWPRValue - (-100)) < CN.Threshold)
            {
                var wprhd = new SResult
                {
                    IsBuy = false,
                    IsSell = false,
                    Reading = currentWPRValue
                };
                return wprhd;
            }
            var wprh = new SResult
            {
                IsBuy = currentWPRValue <= (double)oversold ? true : false,
                IsSell = currentWPRValue >= (double)overbought ? true : false,
                Reading = currentWPRValue
            };
            return wprh;
        }
        #endregion

    }
}
