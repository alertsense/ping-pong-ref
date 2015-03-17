using AlertSense.PingPong.ServiceModel.Enums;
using System;
using System.Collections.Generic;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class GameModel
    {
        public GameModel()
        {
            Players = new List<PlayerModel>();
            Points = new List<PointModel>();
            Created = DateTime.UtcNow.Ticks;
        }

        public long Created { get; set; }

        public Guid Id { get; set; }
        public List<PlayerModel> Players { get; set; }
        public List<PointModel> Points { get; set; }

        public PointModel CurrentPoint { get; set; }

        public Side InitialServer { get; set; }
        public Side CurrentServer { get; set; }

        public bool IsServe { get; set; }
        public GameState GameState { get; set; }

        public string GameStateString
        {
            get { return GameState.ToString(); }
        }

        // Side Hitting the ball
        public Side Striker { get; set; }

        public Side NotStriker
        {
            get
            {
                return Striker == Side.One ? Side.Two : Side.One;
            }
        }
    }
}