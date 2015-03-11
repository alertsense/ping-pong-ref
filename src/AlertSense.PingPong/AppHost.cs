using AlertSense.PingPong.ServiceInterface;
using Funq;
using ServiceStack;

namespace AlertSense.PingPong
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes.
        /// </summary>
        public AppHost()
            : base("AlertSense.PingPong", typeof(AppHost).Assembly)
        {
        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            // register our game service, includes registering any dependencies that it needs within our plugin's configuration
            Plugins.Add(new GameServicePlugin());
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
        }
    }
}