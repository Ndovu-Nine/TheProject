using System.Collections.Generic;
using System.Linq;
using TemboShared.Model;
using TemboShared.Service;

namespace TemboAI.Indicator
{
    public class VSA
    {
        public static int VolumeLabel(double volume, List<Candlestick> sticks)
        {
            double averageVolume = ((IEnumerable<double>)SMA.Calculate(sticks.Volumes().ToArray(), 27)).LastOrDefault<double>();
            double degree = ((double)volume - averageVolume) / averageVolume;

            if (degree >= 1)
            {
                return 3;
            }
            if (degree >= 0.6)
            {
                return 2;
            }
            if (degree > -0.2)
            {
                return 1;
            }
            return 0;
        }

        public static int SpreadLabel(double spread, List<Candlestick> sticks)
        {
            double averageSpread = ((IEnumerable<double>)SMA.Calculate(ASV.Calculate(sticks), 27)).LastOrDefault();
            double degree = (spread - averageSpread) / averageSpread;
            if (degree >= 1)
            {
                return 3;
            }
            if (degree >= 0.6)
            {
                return 2;
            }
            if (degree > -0.2)
            {
                return 1;
            }
            return 0;
        }

        public static double Spread(List<Candlestick> sticks,int period)
        {
            var cans = sticks.TakeLast(period);
            var spreads = ASV.Calculate(cans);
            var top = spreads[0];
            var bottom = spreads.Last();
            return top - bottom;
        }

        public static int CloseType(List<Candlestick> sticks)
        {
            Candlestick candlestick = sticks.LastOrDefault<Candlestick>();
            return candlestick.Close <= (candlestick.High - candlestick.Low) * 0.3 + candlestick.Low ? 0
                : candlestick.Close >= candlestick.High - (candlestick.High - candlestick.Low) * 0.3 ? 2 : 1;
        }
    }
}
