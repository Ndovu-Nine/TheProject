using System.Collections.Generic;

namespace TemboAI.Models
{
    public class FractalResult
    {
        public List<double> UpperValues { get; set; }
        public List<double> LowerValues { get; set; }
    }

    public class TimeChart
    {
        public int Hour { get; set; }
        public double RITM;
        public double RTotal;
        public double BITM;
        public double BTotal;
        public double FITM;
        public double FTotal;
        public double MITM;
        public double MTotal;
        public double WITM;
        public double WTotal;
        public double SITM;
        public double STotal;
        public double RSITM;
        public double RSTotal;

    }
}