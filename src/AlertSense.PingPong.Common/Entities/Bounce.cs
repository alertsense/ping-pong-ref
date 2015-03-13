using System;
using AlertSense.PingPong.ServiceModel.Enums;
using ServiceStack.Model;
using ServiceStack.DataAnnotations;

namespace AlertSense.PingPong.Common.Entities
{
    public class Bounce : IHasId<Guid>
    {
        public Guid Id { get; set; }

        [References(typeof(Game))]
        public Guid GameId { get; set; }

        [References(typeof(Player))]
        public Guid PlayerId { get; set; }

        public Side SideBounced { get; set; }

        public ulong Ticks { get; set; }
    }
}