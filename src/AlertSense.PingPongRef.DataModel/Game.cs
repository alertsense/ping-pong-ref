using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPongRef.DataModel
{
    public class Game : IHasId<Guid>
    {
        public Guid Id { get; set; }
        
        public DateTime DateStarted { get; set; }

        [Reference]
        public List<Player> Players { get; set; }

        [Reference]
        public List<Point> Points { get; set; }

    }
}
