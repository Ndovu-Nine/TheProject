using System;
using TemboShared.Model;
using TemboShared.Service;

namespace TemboAI.Indicator
{
    public class ATR : IndicatorBase
    {
        public ATR()
        {
            this.Name = "Welles Wilder's Average True Range";
            this.ShortName = nameof(ATR);
        }

        public static double[] Calculate(int period, TimeSeries timeSeries)
        {
            CN.Assert(period > 0, "Invalid period");
            double[] price = new double[timeSeries.Close.Length];
            price[0] = 0.0;
            for (int index = 1; index < timeSeries.Close.Length; ++index)
            {
                double num1 = Math.Abs(timeSeries.Close[index - 1] - timeSeries.High[index]);
                double num2 = Math.Abs(timeSeries.Close[index - 1] - timeSeries.Low[index]);
                double num3 = timeSeries.High[index] - timeSeries.Low[index];
                double num4 = num1 > num2 ? num1 : num2;
                price[index] = num4 > num3 ? num4 : num3;
            }
            return EMA.Calculate(price, period);
        }
    }
}
