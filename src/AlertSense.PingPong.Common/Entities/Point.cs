using System;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.Common.Entities
{
    public class Point
    {
        public Guid GameId { get; set; }
        public Side SideToAward { get; set; }
        public ulong Ticks { get; set; }
    }
}