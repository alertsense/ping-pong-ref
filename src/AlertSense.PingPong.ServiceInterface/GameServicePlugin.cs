using AlertSense.PingPong.Common.Entities;
using AlertSense.PingPong.Common.Interfaces;
using AlertSense.PingPong.Data;
using AlertSense.PingPong.Domain;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
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

            container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

            using(var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                db.CreateTableIfNotExists<Game>();
                db.CreateTableIfNotExists<Point>();
                db.CreateTableIfNotExists<Bounce>();
                db.CreateTableIfNotExists<Player>();
            }

            // register our dependencies for the service
            container.RegisterAs<GameRepository, IGameRepository>();
            container.RegisterAs<GameManager, IGameManager>();

            // register our service with our app host
            appHost.RegisterService<GameService>();
        }
    }
}
