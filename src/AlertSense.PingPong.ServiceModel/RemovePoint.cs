﻿using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.ServiceModel
{

    [Route("/Games/{Id}/Point", "DELETE", Summary = "Removes the last point from a side.")]
    public class RemoveLastPointRequest : IReturn<RemoveLastPointResponse>
    {

    }

    public class RemoveLastPointResponse : GameResponse
    {

    }

}
