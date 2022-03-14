using System.Collections.Generic;
using System.Linq;
using TemboAI.Models;
using TemboShared.Model;
using TemboShared.Service;

namespace TemboAI.Strategies
{
    public class SFractal : IStrategy
    {
        /// <summary>
        /// Gets default signal
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public IndicatorSignal GetSignal(List<Candlestick> candles, int window)
        {
            var signal = new IndicatorSignal
            {
                IsSell = false,
                IsBuy = false
            };
            if (candles.Count <= window * 2)
            {
                return signal;
            }

            var fresult = Indicator.Fractal.Calculate(candles, window);
            var currentCandle = candles[^2];
            foreach (var upperValue in fresult.UpperValues.TakeLast(2))
            {
                //all possible strategies of this indicator can be done in this loop
                if(upperValue<currentCandle.Open)continue;
                //Trade breakout
                if (currentCandle.Close > upperValue)
                {
                    signal.IsBuy = true;
                    break;
                }
            }

            foreach (var lowerValue in fresult.LowerValues.TakeLast(2))
            {
                //all possible strategies of this indicator can be done in this loop
                if (lowerValue<currentCandle.Open)continue;
                //Trade breakout
                if (currentCandle.Close < lowerValue)
                {
                    signal.IsSell = true;
                    break;
                }
            }

            return signal;
        }
        /// <summary>
        /// Same as Default - Use default
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="window"></param>
        /// <param name="lookBack"></param>
        /// <returns></returns>
        public IndicatorSignal GetSignal(List<Candlestick> candles, int window, int lookBack)
        {
            var signal = new IndicatorSignal
            {
                IsSell = false,
                IsBuy = false
            };
            if (candles.Count <= window * 2)
            {
                return signal;
            }

            var fresult = Indicator.Fractal.Calculate(candles, window);
            var currentCandle = candles.Last();
            foreach (var upperValue in fresult.UpperValues.TakeLast(lookBack))
            {
                if (upperValue < currentCandle.Open) continue;
                if (currentCandle.Close > upperValue)
                {
                    signal.IsBuy = true;
                    break;
                }
            }

            foreach (var lowerValue in fresult.LowerValues.TakeLast(lookBack))
            {
                if (lowerValue < currentCandle.Open) continue;
                if (currentCandle.Close < lowerValue)
                {
                    signal.IsSell = true;
                    break;
                }
            }

            return signal;
        }


        #region NON FRACTAL

        public IndicatorSignal GetSignal(List<Candlestick> candles, int period, double deviationUp, double deviationDown)
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


        /*private static void Basic(int lookBack, SResult satr, Candlestick currentCandle, double[] fractal)
        {
            var last = fractal.Length - 1 - lookBack;
            for (var i = fractal.Length - 1; i > 0; i--)
            {
                if (i < last) return;
                if (IsNear(fractal[i], currentCandle.Close))
                {
                    double num = (fractal[i] - currentCandle.Close) / fractal[i] * 100.0;
                    //comming from above, support
                    if (num < 0)
                    {
                        satr.IsBuy = true;
                        satr.IsSell = false;
                        satr.Reading = fractal[i];
                        break;
                    }

                    //coming from below, resistance
                    if (num > 0)
                    {
                        satr.IsBuy = false;
                        satr.IsSell = true;
                        satr.Reading = fractal[i];
                        break;
                    }
                }

                //break out from below
                if (currentCandle.Open < fractal[i] && currentCandle.Close > fractal[i])
                {
                    satr.IsBuy = true;
                    satr.IsSell = false;
                    satr.Reading = fractal[i];
                    break;
                }

                //break out from above
                if (currentCandle.Open > fractal[i] && currentCandle.Close < fractal[i])
                {
                    satr.IsBuy = false;
                    satr.IsSell = true;
                    satr.Reading = fractal[i];
                    break;
                }
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="price"></param>
        /// <param name="acceptable">% that is considered near the important area</param>
        /// <returns></returns>
        private static bool IsNear(double line, double price, double acceptable = 0.01)
        {
            double num = (line - price) / line * 100.0;
            if (num < 0.0)
                num *= -1.0;
            return num <= (double)acceptable;
        }

        #endregion
    }
}
