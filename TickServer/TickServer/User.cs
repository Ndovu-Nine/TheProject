using System;
using Alchemy.Classes;


namespace TickServer
{
    /// <summary>
    /// Holds the name and context instance for an online user
    /// </summary>
    public class User
    {
        public string Name = String.Empty;
        public UserContext Context { get; set; }
    }
}
