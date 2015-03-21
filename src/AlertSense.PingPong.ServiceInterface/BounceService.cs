using AlertSense.PingPong.Common.Interfaces;
using AlertSense.PingPong.Common.Messages;
using AlertSense.PingPong.ServiceModel;
using ServiceStack;

namespace AlertSense.PingPong.ServiceInterface
{
    /// <summary>
    /// The Bounce Service is our queue consumer for BounceMessages.
    /// </summary>
    public class BounceService : Service, IBounceService
    {
        public IGameManager GameManager { get; set; }

        public CreateBounceResponse Post(BounceMessage message)
        {
            var createBounceRequest = new CreateBounceRequest
            {
                GameId = message.GameId,
                Side = message.Side
            };
            return (CreateBounceResponse) HostContext.ServiceController.Execute(createBounceRequest, Request);
        }
    }
}