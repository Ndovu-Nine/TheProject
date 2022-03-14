using System;
using TemboShared.Model;
using TemboShared.Service;

namespace TemboAI.Indicator
{
    public class ATRP : IndicatorBase
    {
        public ATRP()
        {
            this.Name = "Average True Range Percentage";
            this.ShortName = nameof(ATRP);
        }

        public static double[] Calculate(int period, TimeSeries timeSeries)
        {
            CN.Assert(period > 0, "Invalid period");
            CN.Assert(timeSeries != null, "Timeseries must not be null");
            double[] numArray1 = ATR.Calculate(period, timeSeries);
            double[] numArray2 = new double[numArray1.Length];
            for (int index = period; index < timeSeries.Close.Length; ++index)
                numArray2[index] = numArray1[index] * 100.0 / timeSeries.Close[index];
            return numArray2;
        }
    }
}
