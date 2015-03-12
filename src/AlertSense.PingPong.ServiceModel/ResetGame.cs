using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.ServiceModel
{
    [Route("/Games/{Id}/Reset", "POST", Summary = "Reset an existing Ping Pong game.")]
    public class ResetGameRequest : IReturn<ResetGameResponse>
    {

    }

    public class ResetGameResponse : GameResponse
    {

    }
}
