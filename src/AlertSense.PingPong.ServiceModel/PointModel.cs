using AlertSense.PingPong.ServiceModel.Enums;
using System;

namespace AlertSense.PingPong.ServiceModel
{
    public class PointModel
    {
        public Guid GameId { get; set; }
        public Side SideToAward { get; set; }
        public ulong Ticks { get; set; }
    }
}