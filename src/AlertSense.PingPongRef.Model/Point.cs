using System;

namespace AlertSense.PingPongRef.Model
{
    public class Point
    {
        public Guid GameId { get; set; }
        public Side SideToAward { get; set; }
        public ulong Ticks { get; set; }
    }
}