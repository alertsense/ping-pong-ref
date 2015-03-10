using System;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.Model
{
    public class Point
    {
        public Guid GameId { get; set; }
        public Side SideToAward { get; set; }
        public ulong Ticks { get; set; }
    }
}