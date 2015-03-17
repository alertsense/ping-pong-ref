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
            Players = new List<Player>();
            Points = new List<Point>();            
        }

        public Guid Id { get; set; }

        [Reference]
        public List<Player> Players { get; set; }

        [Reference]
        public List<Point> Points { get; set; }

        [References(typeof(Point))]
        public Guid CurrentPointId { get; set; }

        [Reference]
        public Point CurrentPoint { get; set; }      


        public Side InitialServer { get; set; }
        public Side CurrentServer { get; set; }

        public bool IsServe { get; set; }
        public GameState GameState { get; set; }
        public Side Striker { get; set; }
    }
}