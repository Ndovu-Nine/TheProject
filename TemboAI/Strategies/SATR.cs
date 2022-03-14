using System;
using System.Collections.Generic;
using System.Linq;
using TemboAI.Models;
using TemboShared.Model;
using TemboShared.Service;
using TANet.Core;

namespace TemboAI.Strategies
{
    public class SATR:IStrategy
    {

        public IndicatorSignal GetSignal(List<Candlestick> candles, int period)
        {
            var jfk = Indicators.Atr(candles.FromCandles(), period);
            var satr = new IndicatorSignal
            {
                IsBuy = false,
                IsSell = false,
            };
            if (jfk.Success)
            {
                var signal = jfk.IndicatorSignal;
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Buy)
                {
                    satr.IsBuy = true;
                    satr.IsSell = false;
                }
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Sell)
                {
                    satr.IsBuy = false;
                    satr.IsSell = true;
                }
            }
            return satr;
        }

        #region NON ATR
        /// <summary>
        /// Does not give a signal but gives %change in volatility
        /// I wonder what you can do with that!
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="lastPrice"></param>
        /// <returns></returns>
        [Obsolete("Use GetSignal")]
        public static SResult Get(List<Candlestick> candles,int period)
        {
            var jfk = Indicators.Atr(candles.FromCandles(), period);
            var atrDoubles = Indicator.ATRP.Calculate(period,candles.ToTimeSeries());
            double currentATRPValue = ((IEnumerable<double>)atrDoubles).Last();
            var satr = new SResult
            {
                IsBuy = false,
                IsSell =  false,
                Reading =currentATRPValue
            };
            if (jfk.Success)
            {
                var signal = jfk.IndicatorSignal;
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Buy)
                {
                    satr.IsBuy = true;
                    satr.IsSell = false;
                }
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Sell)
                {
                    satr.IsBuy = false;
                    satr.IsSell = true;
                }
            }
            return satr;
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

        public IndicatorSignal GetSignal(List<Candlestick> candles, int setting1, int setting2, int setting3)
        {
            throw new NotImplementedException();
        }

        public IndicatorSignal GetSignal(List<Candlestick> candles, int setting1, int setting2, int setting3, int setting4, int setting5, int setting6, int setting7)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
