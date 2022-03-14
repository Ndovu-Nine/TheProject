using System.Collections.Generic;
using TemboShared.Model;

namespace TemboAI.Indicator
{
    public class ASV : IndicatorBase
    {
        public ASV()
        {
            this.Name = "Atlas Spread Visualization";
            this.ShortName = "ATS";
        }

        public static double[] Calculate(List<Candlestick> bars)
        {
            double[] numArray = new double[bars.Count];
            for (int index = 0; index < bars.Count; ++index)
                numArray[index] = bars[index].High - bars[index].Low;
            return numArray;
        }
    }
}
