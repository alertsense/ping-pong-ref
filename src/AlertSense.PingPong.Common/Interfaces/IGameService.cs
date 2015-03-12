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
        // /Games        
        GameSummaryResponse Post(GetGameSummaryRequest request);

        // /Games/{Id}
        GameResponse Post(GetGameRequest request);

        // /Games/
        CreateGameResponse Post(CreateGameRequest request);

        // /Games/{Id}/Reset
        ResetGameResponse Post(ResetGameRequest request);

        // /Games/{Id}/Point
        CreatePointResponse Post(CreatePointRequest request);

        // /Games/{Id}/Point
        RemoveLastPointResponse Delete(RemoveLastPointRequest request);

        // /Games/{Id}/Bounce
        CreateBounceResponse Post(CreateBounceRequest request);
    }
}
