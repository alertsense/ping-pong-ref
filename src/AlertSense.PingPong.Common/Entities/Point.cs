using System;
using AlertSense.PingPong.ServiceModel.Enums;
using ServiceStack.Model;
using ServiceStack.DataAnnotations;
using System.Collections.Generic;

namespace AlertSense.PingPong.Common.Entities
{
    public class Point : IHasId<Guid>
    {
        public Point()
        {
            Bounces = new List<Bounce>();
        }
        public Guid Id { get; set; }

        [References(typeof(Game))]
        public Guid GameId { get; set; }

        [References(typeof(Player))]
        public Guid PlayerId { get; set; }

        [Reference]
        public List<Bounce> Bounces { get; set; }

        public Side SideToAward { get; set; }

        public ulong Ticks { get; set; }
    }
}