using AlertSense.PingPong.Common.Interfaces;
using AlertSense.PingPong.Data;
using AlertSense.PingPong.Domain;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.ServiceInterface
{
    public class GameServicePlugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            var container = appHost.GetContainer();

            // register our dependencies for the service
            container.RegisterAs<GameRepository, IGameRepository>();
            container.RegisterAs<GameManager, IGameManager>();

            // register our service with our app host
            appHost.RegisterService<GameService>();
        }
    }
}
