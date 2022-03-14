namespace TickServer
{
    /// <summary>
    /// Defines the response object to send back to the client
    /// </summary>
    public class Response
    {
        public ResponseType Type { get; set; }
        public dynamic Data { get; set; }
    }
}
