using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Raspberry.Models
{
    public class Table
    {
        public string Name { get; set; }

        public TableSettings Settings { get; set; }
        
        public bool ServiceLight { get; set; }

        public bool ButtonState { get; set; }
        public double ButtonDuration { get; set; }
        public DateTime? ButtonLastPressed { get; set; }

        public long LastBounce { get; set; }

        public string Message { get; set; }
    }
}
