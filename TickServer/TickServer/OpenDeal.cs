namespace TickServer
{
    public class OpenDeal
    {
        public string Identifier { get; set; }
        public ulong Ticket { get; set; }
        public string Direction { get; set; }
        public string Symbol { get; set; }
        public bool IsOpen { get; set; }
        public string DurationOfCandle { get; set; }
    }
}