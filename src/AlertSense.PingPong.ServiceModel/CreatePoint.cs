using ServiceStack;

namespace AlertSense.PingPong.ServiceModel
{
    [Route("/Games/{Id}/Point", "POST", Summary = "Create a new point associated with a game.")]
    public class CreatePoint : IReturn<CreatePointResponse>
    {

    }

    public class CreatePointResponse : GameResponse
    {

    }
}