using ServiceStack;

namespace AlertSense.PingPong.ServiceModel
{
    [Route("/Games", "POST", Summary = "Start a new Ping Pong game.")]
    public class CreateGame : IReturn<CreateGameResponse>
    {

    }

    public class CreateGameResponse : GameResponse
    {
        
    }
}