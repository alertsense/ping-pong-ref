using AlertSense.PingPong.ServiceModel.Enums;
using ServiceStack;
using System;

namespace AlertSense.PingPong.Common.Messages
{
    /// <summary>
    /// This is the message that will be queued in RabbitMq.
    /// These queued messages are consumed by the BounceService.
    /// </summary>
    [Route("/internal/bounce", HttpMethods.Post)]
    public class BounceMessage : IReturnVoid
    {
        public Guid GameId { get; set; }
        public Side Side { get; set; }
        public long Ticks { get; set; }
    }
}