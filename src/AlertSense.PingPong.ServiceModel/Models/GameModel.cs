using System;
using System.Collections.Generic;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class GameModel
    {
        public GameModel()
        {
            Players = new List<PlayerModel>();
            Points = new List<PointModel>();
            Bounces = new List<BounceModel>();
        }

        public Guid Id { get; set; }
        public List<PlayerModel> Players { get; set; }
        public List<PointModel> Points { get; set; }
        public List<BounceModel> Bounces { get; set; }
        public Side InitialServer { get; set; }
        public GameState GameState { get; set; }
    }
}