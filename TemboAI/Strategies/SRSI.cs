using System;
using System.Collections.Generic;
using System.Linq;
using TemboAI.Models;
using TemboShared.Model;
using TemboShared.Service;

namespace TemboAI.Strategies
{
    public class SRSI:IStrategy
    {
        public IndicatorSignal GetSignal(List<Candlestick> candles, int period, int overbought, int oversold)
        {
            var rsiDoubles = Indicator.RSI.Calculate(candles.Closes().ToArray(), period);
            double currentRSIValue = rsiDoubles[^1];
            double previousRSIValue= rsiDoubles[^4];
            var srsi = new IndicatorSignal
            {
                IsBuy = false,
                IsSell = false,
            };
            if (currentRSIValue > oversold && previousRSIValue <= oversold)
            {
                srsi.IsBuy = true;
            }
            if (currentRSIValue < overbought && previousRSIValue >= overbought)
            {
                srsi.IsSell = true;
            }

            srsi.Series = rsiDoubles;
            return srsi;
        }

        #region NON RSI
        public IndicatorSignal GetSignal(List<Candlestick> candles, int period)
        {
            throw new System.NotImplementedException();
        }

        public IndicatorSignal GetSignal(List<Candlestick> candles, int period, double deviationUp, double deviationDown)
        {
            throw new System.NotImplementedException();
        }

        public IndicatorSignal GetSignal(List<Candlestick> candles, int setting1, int setting2)
        {
            throw new System.NotImplementedException();
        }

        public IndicatorSignal GetSignal(List<Candlestick> candles, double setting1, double setting2)
        {
            throw new System.NotImplementedException();
        }
        public IndicatorSignal GetSignal(List<Candlestick> candles, int setting1, int setting2, int setting3, int setting4, int setting5, int setting6, int setting7)
        {
            throw new System.NotImplementedException();
        }
        [Obsolete("Use GetSignal")]
        public static SResult Get(List<Candlestick> candles, int period, int overbought = 80, int oversold = 20)
        {
            var rsiDoubles = Indicator.RSI.Calculate(candles.Closes().ToArray(), period);
            double currentRSIValue = rsiDoubles.Last();
            var srsi = new SResult
            {
                IsBuy = currentRSIValue <= (double)oversold ? true : false,
                IsSell = currentRSIValue >= (double)overbought ? true : false,
                Reading = currentRSIValue
            };
            return srsi;
        }
        #endregion

    }
}
