using System.Collections.Generic;
using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack;

namespace AlertSense.PingPong.ServiceModel
{
    [Route("/Games","GET")]
    public class GetGameSummaryRequest : IReturn<GameSummaryResponse>
    {

    }

    public class GameSummaryResponse : List<GameSummary>
    {

    }
}