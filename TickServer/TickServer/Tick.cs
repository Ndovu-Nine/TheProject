using System;

namespace TickServer
{
    public class Tick
    {
        public string Symbol { get; set; }

        public DateTime DateTime { get; set; }

        public double BidPrice { get; set; }

        public double AskPrice { get; set; }

        public double Quote { get; private set; }

        public void CalculateQuote()
        {
            this.Quote = Math.Round((this.AskPrice + this.BidPrice) / 2.0, 6);
        }

        public void SetQuote(double quote)
        {
            this.Quote = quote;
        }
    }

}
