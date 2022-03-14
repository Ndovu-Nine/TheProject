using TemboShared.Service;

namespace TemboAI.Indicator
{
    public class SMA : IndicatorBase
    {
        public SMA()
        {
            this.Name = "Simple Moving Average";
            this.ShortName = nameof(SMA);
        }

        public static double[] Calculate(double[] price, int period)
        {
            CN.Assert(price.Length > period, "Invalid period");
            CN.Assert(price.Length > 0, "Empty prices");
            CN.Assert(period > 0, "Invalid period");
            double[] numArray = new double[price.Length];
            double num1 = 0.0;
            for (int index = 0; index < period; ++index)
            {
                num1 += price[index];
                numArray[index] = num1 / (double)(index + 1);
            }
            for (int index1 = period; index1 < price.Length; ++index1)
            {
                double num2 = 0.0;
                for (int index2 = index1; index2 > index1 - period; --index2)
                    num2 += price[index2];
                numArray[index1] = num2 / (double)period;
            }
            return numArray;
        }
    }
}
