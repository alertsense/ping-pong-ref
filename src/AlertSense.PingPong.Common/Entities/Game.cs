using System;
using AlertSense.PingPong.ServiceModel.Enums;
using ServiceStack.Model;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace AlertSense.PingPong.Common.Entities
{
    public class Game : IHasId<Guid>
    {
        public Game()
        {
            Players = new List<Player>();
            Points = new List<Point>();
            Bounces = new List<Bounce>();
        }

        public Guid Id { get; set; }

        [Reference]
        public List<Player> Players { get; set; }

        [Reference]
        public List<Point> Points { get; set; }

        [Reference]
        public List<Bounce> Bounces { get; set; }

        public Side InitialServer { get; set; }

        public GameState GameState { get; set; }
    }
}