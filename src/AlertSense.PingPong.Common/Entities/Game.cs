﻿using System;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.Common.Entities
{
    public class Game
    {
        public Guid GameId { get; set; }
        public Player[] Players { get; set; }
        public Side InitialServer { get; set; }
        public GameState GameState { get; set; }
    }
}