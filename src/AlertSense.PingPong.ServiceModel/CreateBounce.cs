using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack;
using System;

namespace AlertSense.PingPong.ServiceModel
{
    [Route("/Games/{GameId}/Bounce", "POST", Summary = "Create a new bounce associated with a game.")]
    public class CreateBounceRequest : IReturn<CreateBounceResponse>
    {
        public Guid GameId { get; set; }
        public Side BounceSide { get; set; }
    }

    public class CreateBounceResponse : GameModel
    {

    }
}