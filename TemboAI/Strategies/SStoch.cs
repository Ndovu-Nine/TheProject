using System;
using System.Collections.Generic;
using System.Linq;
using TemboAI.Models;
using TemboShared.Model;
using TANet.Core;

namespace TemboAI.Strategies
{
    public class SStoch:IStrategy
    {
        public IndicatorSignal GetSignal(List<Candlestick> candles, int fast, int middle, int slow)
        {
            var jfk = Indicators.Stochsatic(candles.FromCandles(), fast, middle, slow);
            var satr = new IndicatorSignal
            {
                IsBuy = false,
                IsSell = false
            };
            if (jfk.Success)
            {
                satr.Series = Array.ConvertAll(jfk.SlowK, x => (double)x);
                var signal = jfk.IndicatorSignal;
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Buy)
                {
                    satr.IsBuy = true;
                }
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Sell)
                {
                    satr.IsSell = true;
                }
            }
            return satr;
        }

        #region NON Stoch

        [Obsolete("Use GetSignal")]
        public static SResult Get(List<Candlestick> candles,int fast,int middle, int slow)
        {
            
            var jfk = Indicators.Stochsatic(candles.FromCandles(),fast, middle, slow);
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
                    satr.Reading = (double)jfk.SlowD.Last();
                }
                if (signal == TANet.Contracts.Enums.IndicatorSignal.Sell)
                {
                    satr.IsBuy = false;
                    satr.IsSell = true;
                    satr.Reading = (double)jfk.SlowD.LastOrDefault();
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
