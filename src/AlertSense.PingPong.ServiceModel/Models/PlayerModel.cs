using System.Collections.Generic;
using System;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class PlayerModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public ushort Score { get; set; }
        public List<PointModel> History { get; set; }
    }
}