using System;
using System.Collections.Generic;
using System.Linq;
using TemboShared.Model;
using TemboShared.Service;

namespace TemboAI.Indicator
{
    public class WPR : IndicatorBase
    {
        public WPR()
        {
            this.Name = "Williams Percent Range";
            this.ShortName = nameof(WPR);
        }

        public static double[] Calculate(double[] price, int period, TimeSeries timeSeries)
        {
            CN.Assert(price.Length > period, "Invalid period");
            CN.Assert(price.Length > 0, "Empty prices");
            CN.Assert(period > 0, "Invalid period");
            CN.Assert(timeSeries != null, "Timeseries must not be null");
            double[] numArray = new double[price.Length];
            for (int index1 = period; index1 < price.Length; ++index1)
            {
                double num1 = ((IEnumerable<double>)timeSeries.Close).Max();
                double num2 = ((IEnumerable<double>)timeSeries.Close).Min();
                for (int index2 = index1 - period + 1; index2 <= index1; ++index2)
                {
                    if (timeSeries.High[index2] > num1)
                        num1 = timeSeries.High[index2];
                    if (timeSeries.Low[index2] < num2)
                        num2 = timeSeries.Low[index2];
                }
                numArray[index1] = -100.0 * (num1 - timeSeries.Close[index1]) / (num1 - num2);
            }
            return numArray;
        }

        public static double[] Calculate(double[] price, TimeSeries timeSeries, int period = 30)
        {
            if (price.Length <= 0)
            {
                throw new Exception("Price cannot be empty");
            }
            if (period > price.Length)
            {
                throw new Exception("Period greator tha availlable prices");

            }
            if (timeSeries == null)
            {
                throw new Exception("Timeseries must not be null");
            }
            double[] numArray = new double[price.Length];
            for (int index1 = period; index1 < price.Length; ++index1)
            {
                double num1 = Extension.TakeLast<double>(((IEnumerable<double>)timeSeries.Close), period).Max();
                double num2 = Extension.TakeLast<double>(((IEnumerable<double>)timeSeries.Close), period).Min();
                for (int index2 = index1 - period + 1; index2 <= index1; ++index2)
                {
                    if (timeSeries.High[index2] > num1)
                        num1 = timeSeries.High[index2];
                    if (timeSeries.Low[index2] < num2)
                        num2 = timeSeries.Low[index2];
                }
                numArray[index1] = -100.0 * (num1 - timeSeries.Close[index1]) / (num1 - num2);
            }
            return numArray;
        }
    }
}
