using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.ServiceInterface.IntegrationTests
{
    public class IntegrationAppHost : AppHostHttpListenerBase
    {
        public IntegrationAppHost()
            : base("IntegrationAppHost", typeof(IntegrationAppHost).Assembly)
        {

        }
        public override void Configure(Funq.Container container)
        {
            Plugins.Add(new GameServicePlugin());
        }
    }
}
