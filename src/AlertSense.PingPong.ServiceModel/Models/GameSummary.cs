using System;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class GameSummary
    {
        public Guid Id { get; set; }
        public PlayerModel[] Players { get; set; }
    }
}