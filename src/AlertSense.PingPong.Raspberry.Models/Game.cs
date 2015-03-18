using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Raspberry.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public string  CurrentServingTable { get; set; }
    }
}
