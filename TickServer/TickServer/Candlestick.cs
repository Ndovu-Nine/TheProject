using System;
using System.Collections.Generic;

namespace TickServer
{
    public class Candlestick
    {
        public Int64 Id { get; private set; }

        public string Symbol { get; set; }
        public string Duration { get; set; }
        public double High { get; private set; }

        public double Low { get; private set; }

        public double Open { get; private set; }

        public double Close { get; private set; }

        public List<Tick> Ticks { get; set; }

        public DateTime Time { get; set; }

        public double Volume { get; set; }

        public double Quote { get; private set; }

        public Candlestick(
          string symbol,
          double open,
          double high,
          double low,
          double close,
          DateTime time,string duration,
          int volume = 0)
        {
            //Id = CN.InMemoryId();
            Symbol = symbol;
            Time = time;
            SetOHLCV(open, high, low, close, volume,duration);
        }

        public void SetOHLCV(double open, double high, double low, double close, int volume, string duration)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            Duration = duration;
        }

        public void Update(Candlestick stick)
        {
            if (stick.High > High)
                High = stick.High;
            if (stick.Low < Low)
                Low = Low;
            Close = stick.Close;
            Volume = stick.Volume;
            Duration = stick.Duration;
        }

        public void SetQuote(double ask, double bid)
        {
            Quote = Math.Round((ask + bid) / 2.0, 6);
        }
    }

}
