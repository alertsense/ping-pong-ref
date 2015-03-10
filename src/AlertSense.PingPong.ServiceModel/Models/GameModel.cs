using System;
using System.Collections.Generic;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class GameModel
    {
        public Guid GameId { get; set; }
        public PlayerModel[] Players { get; set; }
        public List<PointModel> Points { get; set; }
        public List<BounceModel> Bounces { get; set; }
        public Side InitialServer { get; set; }
        public GameState GameState { get; set; }
    }
}