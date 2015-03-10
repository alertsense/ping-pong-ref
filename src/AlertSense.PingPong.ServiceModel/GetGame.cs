using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack;

namespace AlertSense.PingPong.ServiceModel
{
    [Route("/Games/{Id}","GET")]
    public class GetGameRequest : IReturn<GameResponse>
    {

    }

    public class GameResponse : GameModel
    {

    }
}