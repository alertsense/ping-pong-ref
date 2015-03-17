using System;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class BounceModel
    {
        public BounceModel()
        {
            Id = Guid.NewGuid();
            Ticks = DateTime.UtcNow.Ticks;
        }
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Side Side { get; set; }
        public long Ticks { get; set; }
    }
}