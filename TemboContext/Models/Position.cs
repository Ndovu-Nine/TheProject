using System;
using TemboContext.Base;
using TemboContext.Operations;

namespace TemboContext.Models
{
    public class Position : BaseModel
    {

        public int SYSID { get; set; }

        public int AssetSYSID { get; set; }

        public int StrategySYSID { get; set; }

        public string DurationOfCandle { get; set; }

        public DateTime OpenTime { get; set; }

        public DateTime EndTime { get; set; }

        public int DurationOfTrade { get; set; }

        public double OpenPrice { get; set; }

        public double? ClosePrice { get; set; }

        public bool Direction { get; set; }

        public int Fractal { get; set; }

        public int Macd { get; set; }

        public int Rainbow { get; set; }

        public bool Rsi { get; set; }

        public int Stoch { get; set; }

        public int Wpr { get; set; }

        public int TrendA { get; set; }

        public int TrendB { get; set; }

        public int TrendC { get; set; }

        public int TrendD { get; set; }

        public int TrendE { get; set; }

        public int TrendF { get; set; }

        public bool? Outcome { get; set; }

        public bool IsSent { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastUpdate { get; set; }


        //custom fields
        public string ModelKey { get; set; }
        public double MLProbability { get; set; }
        public double MLScore { get; set; }
        public double TASProbability { get; set; }
        public double TASScore { get; set; }
        public string StrategyShortName { get; set; }
        public long Identifier { get; set; }


        /// <summary>
        /// asset - assetSYSID
        /// </summary>
        public Asset Asset()
        {
            using (var ops = new AssetOperation(DbConnection, true))
            {
                return ops.GetBySYSID(AssetSYSID);
            }
        }
        /// <summary>
        /// strategy - strategySYSID
        /// </summary>
        public Strategy Strategy()
        {
            using (var ops = new StrategyOperation(DbConnection, true))
            {
                return ops.GetBySYSID(StrategySYSID);
            }
        }
    }
}