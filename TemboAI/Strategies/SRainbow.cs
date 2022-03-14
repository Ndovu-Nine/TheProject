using System;
using System.Collections.Generic;
using System.Linq;
using TemboAI.Models;
using TemboShared.Model;
using TemboShared.Service;
namespace TemboAI.Strategies
{
    public class SRainbow:IStrategy
    {
        /// <summary>
        /// Get absolute trend
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="redPeriod"></param>
        /// <param name="orangePeriod"></param>
        /// <param name="yellowPeriod"></param>
        /// <returns></returns>
        public IndicatorSignal GetTrend(List<Candlestick> candles, int redPeriod, int orangePeriod, int yellowPeriod)
        {
            var priceArray = candles.Closes().ToArray();
            var redValues = Indicator.EMA.Calculate(priceArray, redPeriod);
            var orangeValues = Indicator.EMA.Calculate(priceArray, orangePeriod);
            var yellowValues = Indicator.EMA.Calculate(priceArray, yellowPeriod);
            var indicatorSignal = new IndicatorSignal
            {
                IsBuy = false,
                IsSell = false,
            };
            if (redValues.Last() > orangeValues.Last() && orangeValues.Last() > yellowValues.Last())
            {
                indicatorSignal.IsBuy = true;
            }
            else if (redValues.Last() < orangeValues.Last() && orangeValues.Last() < yellowValues.Last())
            {
                indicatorSignal.IsSell = true;
            }
            return indicatorSignal;
        }
        
        /// <summary>
        /// Signal from one MA
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public IndicatorSignal GetSignal(List<Candlestick> candles, int period)
        {
            var priceArray = candles.Closes().ToArray();
            var maValues = Indicator.EMA.Calculate(priceArray, period);
            var signal = new IndicatorSignal
            {
                IsSell = false,
                IsBuy = false
            };
            var previousMa = maValues.TakeLastGreatorThan();
            var currentCandle = candles.Last();
            if (currentCandle.Open < previousMa && currentCandle.Close>previousMa)
            {
                signal.IsBuy = true;
            }

            if (currentCandle.Open > previousMa && currentCandle.Close < previousMa)
            {
                signal.IsSell = true;
            }

            return signal;
        }
        /// <summary>
        /// Signal from two MA
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="redPeriod"></param>
        /// <param name="orangePeriod"></param>
        /// <returns></returns>
        public IndicatorSignal GetSignal(List<Candlestick> candles, int redPeriod, int orangePeriod)
        {
            var priceArray = candles.Closes().ToArray();
            var redValues = Indicator.EMA.Calculate(priceArray, redPeriod);
            var orangeValues = Indicator.EMA.Calculate(priceArray, orangePeriod);
            var signal = new IndicatorSignal
            {
                IsSell = false,
                IsBuy = false
            };
            if (orangeValues[^3] > redValues[^3] && orangeValues[^1] < redValues[^1])
            {
                signal.IsBuy = true;
            }
            if (orangeValues[^3] < redValues[^3] && orangeValues[^1] > redValues[^1])
            {
                signal.IsSell = true;
            }
            return signal;
        }
        /// <summary>
        /// Signal from three MA
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="redPeriod"></param>
        /// <param name="orangePeriod"></param>
        /// <param name="yellowPeriod"></param>
        /// <returns></returns>
        public IndicatorSignal GetSignal(List<Candlestick> candles, int redPeriod, int orangePeriod, int yellowPeriod)
        {
            var priceArray = candles.Closes().ToArray();
            var redValues = Indicator.EMA.Calculate(priceArray, redPeriod);
            var orangeValues = Indicator.EMA.Calculate(priceArray, orangePeriod);
            var yellowValues= Indicator.EMA.Calculate(priceArray, yellowPeriod);
            var signal = new IndicatorSignal
            {
                IsSell = false,
                IsBuy = false
            };
            if (yellowValues[^3] > orangeValues[^3] && yellowValues[^1] < orangeValues[^1] && redValues[^1] > orangeValues[^1] && redValues[^1] > yellowValues[^1])
            {
                signal.IsBuy = true;
            }
            if (yellowValues[^3] < orangeValues[^3] && yellowValues[^1] > orangeValues[^1] && redValues[^1] < orangeValues[^1] && redValues[^1] < yellowValues[^1])
            {
                signal.IsSell = true;
            }

            return signal;
        }
        /// <summary>
        /// It is not a signal but identifies trend strength.
        /// Trend strength is either strong or nah
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="redPeriod"></param>
        /// <param name="orangePeriod"></param>
        /// <param name="yellowPeriod"></param>
        /// <param name="greenPeriod"></param>
        /// <param name="bluePeriod"></param>
        /// <param name="indigoPeriod"></param>
        /// <param name="violetPeriod"></param>
        /// <returns></returns>
        public IndicatorSignal GetSignal(List<Candlestick> candles, int redPeriod, int orangePeriod, int yellowPeriod, int greenPeriod, int bluePeriod, int indigoPeriod, int violetPeriod)
        {
            var priceArray = candles.Closes().ToArray();
            var redValues = Indicator.EMA.Calculate(priceArray, redPeriod);
            var orangeValues = Indicator.EMA.Calculate(priceArray, orangePeriod);
            var yellowValues = Indicator.EMA.Calculate(priceArray, yellowPeriod);
            var greenValues = Indicator.EMA.Calculate(priceArray, greenPeriod);
            var blueValues = Indicator.EMA.Calculate(priceArray, bluePeriod);
            var indigoValues = Indicator.EMA.Calculate(priceArray, indigoPeriod);
            var violetValues = Indicator.EMA.Calculate(priceArray, violetPeriod);
            var signal = new IndicatorSignal
            {
                IsSell = false,
                IsBuy = false
            };

            if (redValues.Last() > orangeValues.Last() && orangeValues.Last() > yellowValues.Last() && yellowValues.Last() > greenValues.Last() && greenValues.Last() > blueValues.Last() && blueValues.Last() > indigoValues.Last() && indigoValues.Last() > violetValues.Last())
            {
                signal.IsBuy = true;
            }
            if (redValues.Last() < orangeValues.Last() && orangeValues.Last() < yellowValues.Last() && yellowValues.Last() < greenValues.Last() && greenValues.Last() < blueValues.Last() && blueValues.Last() < indigoValues.Last() && indigoValues.Last() < violetValues.Last())
            {
                signal.IsSell = true;
            }
            return signal;
        }

        #region NON MA
        public IndicatorSignal GetSignal(List<Candlestick> candles, int period, double deviationUp, double deviationDown)
        {
            throw new System.NotImplementedException();
        }

        public IndicatorSignal GetSignal(List<Candlestick> candles, double setting1, double setting2)
        {
            throw new System.NotImplementedException();
        }

        //leave this here for later reference
        /*public static SResult Raw(List<Candlestick> candles, int lightBlue = 6, int green = 18, int yellow = 36, int red = 54, int orange = 72)
        {
            var priceArray = candles.Closes().ToArray();
            //var candleOfInterest = candles[candles.Count - 2];//2
            var currentCandle = candles[^1];
            var previousCandle = candles[^2];//3
            var blueMA = Indicator.EMA.Calculate(priceArray, lightBlue);
            var greenMA = Indicator.EMA.Calculate(priceArray, green);
            var yellowMA = Indicator.EMA.Calculate(priceArray, yellow);
            var redMA = Indicator.EMA.Calculate(priceArray, red);
            var orangeMA = Indicator.EMA.Calculate(priceArray, orange);
            var orangeLine = orangeMA.Last();
            var redLine = redMA.Last();
            var reading =  redLine - orangeLine;
            var satr = new SResult
            {
                IsBuy = false,
                IsSell = false,
                Reading = reading,
                RainbowValues = new double[] { redLine, orangeLine }
            };
            //in an uptrend check bounce on orange
            if (redLine > orangeLine && currentCandle.Close > orangeLine && previousCandle.Low < orangeLine)
            {
                satr.IsSell = false;
                satr.IsBuy = true;
            }
            //in up trend check bounce on red
            if (redLine > orangeLine && currentCandle.Close > redLine && previousCandle.Low < redLine)
            {
                satr.IsSell = false;
                satr.IsBuy = true;
            }
            //in a down trend check resistance on orange
            if (redLine < orangeLine && currentCandle.Close < orangeLine && previousCandle.High > orangeLine)
            {

                satr.IsSell = true;
                satr.IsBuy = false;

            }
            //in a down trend check resistance on red
            if (redLine < orangeLine && currentCandle.Close < redLine && previousCandle.High > redLine)
            {
                satr.IsSell = true;
                satr.IsBuy = false;
            }
            return satr;
        }*/
        [Obsolete("Use GetTrend")]
        public static SResult Get(List<Candlestick> candles, int blue = 9, int green = 33, int yellow = 57)
        {
            var priceArray = candles.Closes().ToArray();
            //var lastCandle = candles.LastOrDefault();
            var blueMA = Indicator.EMA.Calculate(priceArray, blue);
            var greenMA = Indicator.EMA.Calculate(priceArray, green);
            var yellowMA = Indicator.EMA.Calculate(priceArray, yellow);
            var satr = new SResult
            {
                IsBuy = false,
                IsSell = false,
                Reading = blueMA.Last() - yellowMA.Last(),
                RainbowValues = new double[] { blueMA.Last(), greenMA.Last(), yellowMA.Last() }
            };
            if (blueMA.Last() > greenMA.Last() && greenMA.Last() > yellowMA.Last())
            {
                satr.IsBuy = true;
                satr.IsSell = false;
            }
            else if (blueMA.Last() < greenMA.Last() && greenMA.Last() < yellowMA.Last())
            {
                satr.IsBuy = false;
                satr.IsSell = true;
            }
            return satr;
        }
        #endregion



    }
}
