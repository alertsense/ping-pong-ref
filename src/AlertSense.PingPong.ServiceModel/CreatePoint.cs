using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack;
using System;

namespace AlertSense.PingPong.ServiceModel
{
    [Route("/Games/{GameId}/Point", "POST", Summary = "Create a new point associated with a game.")]
    public class CreatePointRequest : IReturn<CreatePointResponse>
    {
        public Guid GameId { get; set; }
        public Side ScoringSide { get; set; }
    }

    public class CreatePointResponse : GameModel
    {

    }
}