using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using AlertSense.PingPong.Common.Interfaces;
using AlertSense.PingPong.ServiceModel;

namespace AlertSense.PingPong.ServiceInterface
{
    public class GameService : Service, IGameService
    {
        public IGameManager GameManager { get; set; }

        public GameSummaryResponse Post(GetGameSummaryRequest request)
        {
            throw new NotImplementedException();
        }

        public GameResponse Post(GetGameRequest request)
        {
            throw new NotImplementedException();
        }

        public CreateGameResponse Post(CreateGameRequest request)
        {
            throw new NotImplementedException();
        }

        public ResetGameResponse Post(ResetGameRequest request)
        {
            throw new NotImplementedException();
        }

        public CreatePointResponse Post(CreatePointRequest request)
        {
            throw new NotImplementedException();
        }

        public RemoveLastPointResponse Delete(RemoveLastPointRequest request)
        {
            throw new NotImplementedException();
        }

        public CreateBounceResponse Post(CreateBounceRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
