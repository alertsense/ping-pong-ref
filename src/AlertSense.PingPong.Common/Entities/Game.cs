using System;
using AlertSense.PingPong.ServiceModel.Enums;
using ServiceStack.Model;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace AlertSense.PingPong.Common.Entities
{
    public class Game : IHasId<Guid>
    {
        public Guid Id { get; set; }

        [Reference]
        public List<Player> Players { get; set; }

        public Side InitialServer { get; set; }

        public GameState GameState { get; set; }
    }
}