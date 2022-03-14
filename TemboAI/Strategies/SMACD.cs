using System;
using System.Collections.Generic;
using System.Linq;
using TemboAI.Models;
using TemboShared.Model;
using TANet.Core;

namespace TemboAI.Strategies
{
    public class SMACD:IStrategy
    {

        public IndicatorSignal GetSignal(List<Candlestick> candles, int fast, int slow, int signalLine)
        {
            var jfk = Indicators.Macd(candles.FromCandles(), fast, slow, signalLine);
            var currValue = jfk.Histogram[^1];
            var prevValue = jfk.Histogram[^4];
            var satr = new IndicatorSignal
            {
                IsBuy = false,
                IsSell = false,
            };

            if (currValue > 0 && prevValue <= 0)
            {
                satr.IsBuy = true;
            }

            if (currValue <= 0 && prevValue > 0)
            {
                satr.IsSell = true;
            }
            /*if (jfk.Success)
            {
                var signal = jfk.IndicatorSignal;
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Buy)
                {
                    satr.IsBuy = true;
                }
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Sell)
                {
                    satr.IsSell = true;
                }
            }*/
            return satr;
        }

        #region NON MACD
        [Obsolete("Use GetSignal")]
        public static SResult Get(List<Candlestick> candles, int fast, int slow, int signalLine)
        {
            var jfk = Indicators.Macd(candles.FromCandles(), fast, slow, signalLine);
            var satr = new SResult
            {
                IsBuy = false,
                IsSell = false,
                Reading = 0
            };
            if (jfk.Success)
            {
                var signal = jfk.IndicatorSignal;
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Buy)
                {
                    satr.IsBuy = true;
                    satr.IsSell = false;
                    satr.Reading = (double)jfk.Macd.LastOrDefault();
                }
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Sell)
                {
                    satr.IsBuy = false;
                    satr.IsSell = true;
                    satr.Reading = (double)jfk.Macd.LastOrDefault();
                }
            }
            return satr;
        }

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
        #endregion
    }
}
