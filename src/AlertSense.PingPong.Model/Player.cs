using System.Collections.Generic;

namespace AlertSense.PingPong.Model
{
    public class Player
    {
        public string Name { get; set; }
        public ushort Score { get; set; }
        public List<Point> History { get; set; }
    }
}