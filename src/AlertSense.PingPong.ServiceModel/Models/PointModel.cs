using System;
using System.Collections.Generic;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class PointModel
    {
        public PointModel()
        {
            Bounces = new List<BounceModel>();

            Id = Guid.NewGuid();
            Ticks = DateTime.UtcNow.Ticks;
        }
        public Guid Id { get; set; }

        public Guid GameId { get; set; }
        public Side SideToAward { get; set; }
        public long Ticks { get; set; }
        public List<BounceModel> Bounces { get; set; }
    }
}