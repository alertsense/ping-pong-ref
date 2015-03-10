using System;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class PointModel
    {
        public Guid GameId { get; set; }
        public Side SideToAward { get; set; }
        public ulong Ticks { get; set; }
    }
}