using System.Collections.Generic;
using TemboShared.Model;

namespace TemboAI.Models
{
    public class SResult
    {
        public bool IsBuy { get; set; }
        public bool IsSell { get; set; }
        public double Reading { get; set; }
        public double[] RainbowValues { get; set; }
    }

    public class IndicatorSignal
    {
        public bool IsBuy { get; set; }
        public bool IsSell { get; set; }
        public double[] Series { get; set; }
    }

    public interface IStrategy
    {
        IndicatorSignal GetSignal(List<Candlestick> candles, int period);
        IndicatorSignal GetSignal(List<Candlestick> candles, int period, double deviationUp, double deviationDown);
        IndicatorSignal GetSignal(List<Candlestick> candles, int setting1, int setting2);
        IndicatorSignal GetSignal(List<Candlestick> candles, double setting1, double setting2);
        IndicatorSignal GetSignal(List<Candlestick> candles, int setting1, int setting2, int setting3);
        IndicatorSignal GetSignal(List<Candlestick> candles, int setting1, int setting2, int setting3, int setting4, int setting5, int setting6, int setting7);
    }
}
