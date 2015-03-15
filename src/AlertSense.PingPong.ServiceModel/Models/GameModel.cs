using System;
using System.Collections.Generic;
using System.Configuration;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class GameModel
    {
        public GameModel()
        {
            Players = new List<PlayerModel>();
            Points = new List<PointModel>();
           
        }

        public Guid Id { get; set; }
        public List<PlayerModel> Players { get; set; }
        public List<PointModel> Points { get; set; }

        public PointModel CurrentPoint { get; set; }

        public Side InitialServer { get; set; }
        public bool IsServe { get; set; }
        public GameState GameState { get; set; }

        // Side Hitting the ball
        public Side Striker { get; set; }

        public Side NotStriker
        {
            get {
                return Striker == Side.One ? Side.Two : Side.One;
            }
        }

        public void ChangeStriker()
        {
            Striker = Striker == Side.One ? Side.Two : Side.One;
        }
    }
}