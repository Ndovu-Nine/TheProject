using System;
using System.Collections.Generic;
using TemboShared.Service;

namespace TemboShared.Model
{
    public class Candlestick
    {
        public long Id { get; private set; }

        public string Symbol { get; set; }
        public string Duration { get; set; }
        public double High { get; set; }

        public double Low { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public List<Tick> Ticks { get; set; }

        public DateTime Time { get; set; }

        public double Volume { get; set; }

        public double Quote { get; set; }

        public Candlestick()
        {
            Id = CN.InMemoryId();
        }
        public Candlestick(
          string symbol,
          double open,
          double high,
          double low,
          double close,
          DateTime time,
          int volume = 0)
        {
            Id = CN.InMemoryId();
            Symbol = symbol;
            Time = time;
            SetOHLCV(open, high, low, close, volume);
        }
        public Candlestick(
          string symbol,
          double open,
          double high,
          double low,
          double close,
          DateTime time, string duration,
          int volume = 0)
        {
            Id = CN.InMemoryId();
            Symbol = symbol;
            Time = time;
            SetOHLCV(open, high, low, close, volume,duration);
        }

        public void SetOHLCV(double open, double high, double low, double close, int volume)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stick"></param>
        /// <param name="volumeMode">1 to add volume, 0 to set volume</param>
        public void Update(Candlestick stick,int volumeMode=0)
        {
            if (stick.High > High)
                High = stick.High;
            if (stick.Low < Low)
                Low = Low;
            Close = stick.Close;
            if (volumeMode == 0)
            {
                Volume = stick.Volume;
            }
            else
            {
                Volume += stick.Volume;
            }
        }

        public void SetQuote(double ask, double bid)
        {
            Quote = Math.Round((ask + bid) / 2.0, 6);
        }

        public void SetId(long id)
        {
            Id = id;
        }
    }
}
