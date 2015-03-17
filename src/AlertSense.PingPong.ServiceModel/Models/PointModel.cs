using System;
using System.Collections.Generic;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class PointModel
    {
        public PointModel()
        {
            Id = Guid.NewGuid();
            Bounces = new List<BounceModel>();
        }
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid GameId { get; set; }
        public Side SideToAward { get; set; }
        public long Ticks { get; set; }
        public List<BounceModel> Bounces { get; set; }
    }
}