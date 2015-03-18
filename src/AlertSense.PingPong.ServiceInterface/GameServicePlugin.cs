using AlertSense.PingPong.Common.Entities;
using AlertSense.PingPong.Common.Interfaces;
using AlertSense.PingPong.Data;
using AlertSense.PingPong.Domain;
using AlertSense.PingPong.Domain.Factories;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace AlertSense.PingPong.ServiceInterface
{
    public class GameServicePlugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            var container = appHost.GetContainer();

            container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));
            //container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(@"C:\git\ping-pong-ref\src\AlertSense.PingPong.db", SqliteDialect.Provider));

            using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                db.CreateTableIfNotExists<Game>();
                db.CreateTableIfNotExists<Point>();
                db.CreateTableIfNotExists<Bounce>();
                db.CreateTableIfNotExists<Player>();
            }

            // register our dependencies for the service
            container.RegisterAs<GameRepository, IGameRepository>();
            container.RegisterAs<GameManager, IGameManager>();
            container.RegisterAs<GameManagerFactory, IGameManagerFactory>();
            container.RegisterAs<SpectateManager, ISpectateManager>();

            // register our service with our app host
            appHost.RegisterService<GameService>();

            // Register the Bounce Service with the AppHost
            appHost.RegisterService<BounceService>();
        }
    }
}