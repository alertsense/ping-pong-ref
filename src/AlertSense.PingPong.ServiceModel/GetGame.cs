using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack;
using System;

namespace AlertSense.PingPong.ServiceModel
{
    [Route("/Games/{GameId}", "GET")]
    public class GetGameRequest : IReturn<GameResponse>
    {
        public Guid GameId { get; set; }
    }

    public class GameResponse : GameModel
    {

    }
}