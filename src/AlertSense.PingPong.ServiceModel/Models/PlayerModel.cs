﻿using System.Collections.Generic;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public ushort Score { get; set; }
        public List<PointModel> History { get; set; }
    }
}