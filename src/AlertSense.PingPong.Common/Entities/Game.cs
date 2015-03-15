using System;
using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack.Model;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace AlertSense.PingPong.Common.Entities
{
    public class Game : IHasId<Guid>
    {
        public Game()
        {
            Players = new List<PlayerModel>();
            Points = new List<PointModel>();
            Bounces = new List<BounceModel>();
        }

        public Guid Id { get; set; }

        [Reference]
        public List<PlayerModel> Players { get; set; }

        [Reference]
        public List<PointModel> Points { get; set; }

        [Reference]
        public List<BounceModel> Bounces { get; set; }

        public Side InitialServer { get; set; }

       

        public GameState GameState { get; set; }
    }
}