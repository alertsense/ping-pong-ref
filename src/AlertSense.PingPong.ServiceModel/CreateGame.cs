using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack;

namespace AlertSense.PingPong.ServiceModel
{
    [Route("/Games", "POST", Summary = "Start a new Ping Pong game.")]
    public class CreateGameRequest : IReturn<CreateGameResponse>
    {

    }

    public class CreateGameResponse : GameModel
    {
        
    }
}