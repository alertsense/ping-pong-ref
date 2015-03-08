using System.Collections.Generic;

namespace AlertSense.PingPongRef.Model
{
    public class Player
    {
        public string Name { get; set; }
        public ushort Score { get; set; }
        public List<Point> History { get; set; }
    }
}