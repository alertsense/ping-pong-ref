using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPongRef.DataModel
{
    public class Point : IHasId<Guid>
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public Guid PlayerId { get; set; }

        public Side SideToAward { get; set; }
        public ulong Ticks { get; set; }

    }
}
