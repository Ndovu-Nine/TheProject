using TemboShared.Service;

namespace TemboAI.Indicator
{
    public class EMA : IndicatorBase
    {
        public EMA()
        {
            this.Name = "Exponential Moving Average";
            this.ShortName = nameof(EMA);
        }

        public static double[] Calculate(double[] price, int period)
        {
            CN.Assert(price.Length > period, "Invalid period");
            CN.Assert(price.Length > 0, "Empty prices");
            CN.Assert(period > 0, "Invalid period");
            double[] numArray = new double[price.Length];
            double num1 = price[0];
            double num2 = 2.0 / (1.0 + (double)period);
            for (int index = 0; index < price.Length; ++index)
            {
                num1 += num2 * (price[index] - num1);
                numArray[index] = num1;
            }
            return numArray;
        }
    }
}
