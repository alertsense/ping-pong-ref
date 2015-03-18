using System;
using System.Configuration;
using AlertSense.PingPong.ServiceInterface;
using Funq;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;
using AlertSense.PingPong.Common.Messages;

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
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());

            // Configure RabbitMQ as our messaging service
            bool useRabbitMq;
            Boolean.TryParse(ConfigurationManager.AppSettings["UseRabbitMq"], out useRabbitMq);
            if (useRabbitMq)
            {
                container.Register<IMessageService>(x => new RabbitMqServer(
                        ConfigurationManager.AppSettings["RabbitMqConnectionString"],
                        ConfigurationManager.AppSettings["RabbitMqUsername"],
                        ConfigurationManager.AppSettings["RabbitMqPassword"]
                    ));

                var mqServer = container.Resolve<IMessageService>();
                mqServer.RegisterHandler<BounceMessage>(ServiceController.ExecuteMessage);
                mqServer.Start();
            }

            // register our game service, includes registering any dependencies that it needs within our plugin's configuration
            Plugins.Add(new GameServicePlugin());
        }
    }
}