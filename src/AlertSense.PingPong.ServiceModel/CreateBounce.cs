using ServiceStack;

namespace AlertSense.PingPong.ServiceModel
{
    [Route("/Games/{Id}/Bounce", "POST", Summary = "Create a new bounce associated with a game.")]
    public class CreateBounceRequest : IReturn<CreateBounceResponse>
    {

    }

    public class CreateBounceResponse : GameResponse
    {

    }
}