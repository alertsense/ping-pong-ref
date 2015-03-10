using AlertSense.PingPong.ServiceModel.Enums;
using System;

namespace AlertSense.PingPong.ServiceModel
{
    public class GameModel
    {
        public Guid GameId { get; set; }
        public PlayerModel[] Players { get; set; }
        public Side InitialServer { get; set; }
        public GameState GameState { get; set; }
    }
}