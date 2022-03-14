using System;

namespace TemboShared.Model
{
    public class TimeSeries
    {
        public double[] Open { get; set; }

        public double[] High { get; set; }

        public double[] Low { get; set; }

        public double[] Close { get; set; }

        public double[] Vol { get; set; }

        public double[] Median { get; set; }

        public double[] Typical { get; set; }

        public double[] Weighted { get; set; }

        public DateTime[] Time { get; set; }
    }
}
