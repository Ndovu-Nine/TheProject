using Microsoft.ML.Data;

using System;

namespace TemboAI.Models
{
    public class PositionFeature
    {
        [ColumnName("DayOfWeek")]
        public float DayOfWeek { get; set; }
        [ColumnName("Hour")]
        public float Hour { get; set; }

        [ColumnName("Direction")]
        public float Direction { get; set; }
        [ColumnName("DurationOfTrade")]
        public float DurationOfTrade { get; set; }

        [ColumnName("Fractal")]
        public float Fractal { get; set; }
        [ColumnName("Macd")]
        public float Macd { get; set; }
        
        [ColumnName("Rainbow")]
        public float Rainbow { get; set; }
        [ColumnName("Rsi")]
        public float Rsi { get; set; }
        [ColumnName("Stoch")]
        public float Stoch { get; set; }
        [ColumnName("Wpr")]
        public float Wpr { get; set; }
        [ColumnName("TrendA")]
        public float TrendA { get; set; }
        [ColumnName("TrendB")]
        public float TrendB { get; set; }
        [ColumnName("TrendC")]
        public float TrendC { get; set; }
        [ColumnName("TrendD")]
        public float TrendD { get; set; }
        [ColumnName("TrendE")]
        public float TrendE { get; set; }
        [ColumnName("TrendF")]
        public float TrendF { get; set; }
        [ColumnName("Outcome")]
        public bool Outcome { get; set; }
        
        /**/
    }

    public enum CollectionMode
    {
        M1=0,
        H1=1,
        H4=2,
        D1=3,
        W1=4,
        MN1=5
    }
}
