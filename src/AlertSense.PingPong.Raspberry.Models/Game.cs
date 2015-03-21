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
        public Table Table1 { get; set; }
        public Table Table2 { get; set; }
    }
}