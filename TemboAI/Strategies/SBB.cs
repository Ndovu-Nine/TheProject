using System;
using System.Collections.Generic;
using System.Linq;
using TemboAI.Models;
using TemboShared.Model;
using TANet.Core;

namespace TemboAI.Strategies
{
    public class SBB:IStrategy
    {

        public IndicatorSignal GetSignal(List<Candlestick> candles, int period, double deviationUp, double deviationDown)
        {
            var jfk = Indicators.BollingerBands(candles.FromCandles(), period, deviationUp, deviationDown);
            var currUpper = (double)jfk.UpperBand[^1];
            var currLower = (double)jfk.LowerBand[^1];
            var satr = new IndicatorSignal
            {
                IsBuy = false,
                IsSell = false,
            };

            if (candles[^4].Open < currUpper && candles[^4].Close > currUpper && candles[^3].Close>currUpper && candles[^2].Close > currUpper && candles[^1].Close > currUpper)
            {
                satr.IsBuy = true;
            }

            if (candles[^4].Open > currLower && candles[^4].Close < currLower && candles[^3].Close < currLower && candles[^2].Close < currLower && candles[^1].Close < currLower)
            {
                satr.IsSell= true;
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

        #region NON BB
        [Obsolete("Use GetSignal")]
        public static SResult Get(List<Candlestick> candles,int period,double deviationUp, double deviationDown)
        {
            var jfk = Indicators.BollingerBands(candles.FromCandles(), period, deviationUp, deviationDown);
            var satr = new SResult
            {
                IsBuy = false,
                IsSell = false,
                Reading =(double)jfk.MiddleBand.LastOrDefault()
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

        public IndicatorSignal GetSignal(List<Candlestick> candles, int period)
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

        public IndicatorSignal GetSignal(List<Candlestick> candles, int setting1, int setting2, int setting3)
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
