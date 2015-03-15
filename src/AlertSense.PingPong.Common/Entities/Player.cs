using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace AlertSense.PingPong.Common.Entities
{
    public class Player : IHasId<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ushort Score { get; set; }
        public List<PointModel> History { get; set; }
    }
}