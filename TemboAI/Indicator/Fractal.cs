/**
 * This class is complete and does not need modification
 * Spend your energies elsewhere
 * other_page
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TemboAI.Models;
using TemboShared.Model;

namespace TemboAI.Indicator
{
    /// <summary>
    /// Modified Fractal Indicator
    /// </summary>
    public class Fractal:IndicatorBase
    {
        public Fractal()
        {
            this.Name = "Fractal";
            this.ShortName = nameof(Fractal);
        }
        
        /// <summary>
        /// Returns a FractalResult with upper and lower values starting from latest to earliest
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="window">number of candlesticks(left & right) to compare with</param>
        /// <returns></returns>
        public static FractalResult Calculate(List<Candlestick> candles, int window)
        {
            var fractalResult = new FractalResult
            {
                UpperValues = new List<double>(),
                LowerValues = new List<double>()
            };
            if (candles == null)
            {
                throw new Exception("candles can not be null");
            }
            for (var i = candles.Count - 1; i >= 0; i--)
            {
                if (i < 0) break;
                var current = candles[i];
                if (i + window > candles.Count - 1) continue;
                if (i - window < 0) continue;
                if (IsFractal(window, i, candles, current,true))
                {
                    fractalResult.UpperValues.Add(current.High);
                }
                if (IsFractal(window, i, candles, current,false))
                {
                    fractalResult.LowerValues.Add(current.Low);
                }
            }

            return fractalResult;
        }

        private static bool IsFractal(int window, int i, List<Candlestick> candles, Candlestick current, bool mode)
        {
            //when j is zero it will check itself
            if (mode)
            {
                for (var j = 1; j < window; j++)
                {
                    if (current.High < candles[i + j].High)
                    {
                        return false;
                    }
                    if (current.High < candles[i - j].High)
                    {
                        return false;
                    }
                }
                return true;
            }

            for (var j = 1; j < window; j++)
            {
                if (current.Low > candles[i + j].Low)
                {
                    return false;
                }
                if (current.Low > candles[i - j].Low)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
