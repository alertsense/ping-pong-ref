using AlertSense.PingPong.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IGameService
    {
        //[Route("/Games", "GET")]        
        GameSummaryResponse Post(GetGameSummaryRequest request);

        // [Route("/Games", "POST", Summary = "Start a new Ping Pong game.")]
        GameResponse Post(GetGameRequest request);

        // [Route("/Games/{Id}/Point", "POST", Summary = "Create a new point associated with a game.")]
        CreatePointResponse Post(CreatePoint request);

        // [Route("/Games/{Id}/Bounce", "POST", Summary = "Create a new bounce associated with a game.")]
        CreateBounceResponse Post(CreateBounceRequest request);
    }
}
