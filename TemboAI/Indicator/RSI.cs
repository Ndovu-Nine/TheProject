using TemboShared.Service;

namespace TemboAI.Indicator
{
    public class RSI : IndicatorBase
    {
        public RSI()
        {
            this.Name = "Relative Strength Index developed by J. Welles Wilder and published in a 1978 book, New Concepts in Technical Trading Systems";
            this.ShortName = nameof(RSI);
        }

        public static double[] Calculate(double[] price, int period)
        {
            CN.Assert(price.Length > period, "Invalid period");
            CN.Assert(price.Length >0, "Empty prices");
            CN.Assert(period>0, "Invalid period");
            /*ValidatorExtensions.IsNotEmpty<double[]>(Condition.Requires<double[]>((M0)price, nameof(price)));
            ValidatorExtensions.IsLessOrEqual(ValidatorExtensions.IsGreaterThan((ConditionValidator<int>)Condition.Requires<int>((M0)period, nameof(period)), 0), price.Length);*/
            double[] numArray = new double[price.Length];
            double num1 = 0.0;
            double num2 = 0.0;
            numArray[0] = 0.0;
            for (int index = 1; index <= period; ++index)
            {
                double num3 = price[index] - price[index - 1];
                if (num3 >= 0.0)
                    num1 += num3;
                else
                    num2 -= num3;
            }
            double num4 = num1 / (double)period;
            double num5 = num2 / (double)period;
            double num6 = num1 / num2;
            numArray[period] = 100.0 - 100.0 / (1.0 + num6);
            for (int index = period + 1; index < price.Length; ++index)
            {
                double num3 = price[index] - price[index - 1];
                if (num3 >= 0.0)
                {
                    num4 = (num4 * (double)(period - 1) + num3) / (double)period;
                    num5 = num5 * (double)(period - 1) / (double)period;
                }
                else
                {
                    num5 = (num5 * (double)(period - 1) - num3) / (double)period;
                    num4 = num4 * (double)(period - 1) / (double)period;
                }
                double num7 = num4 / num5;
                numArray[index] = 100.0 - 100.0 / (1.0 + num7);
            }
            return numArray;
        }
    }
}
