using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using AlertSense.PingPong.Common.Interfaces;
using AlertSense.PingPong.ServiceModel;
using AlertSense.PingPong.ServiceModel.Models;
using AlertSense.PingPong.Common.Entities;

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
            //GameManager.CreateNewGame().ConvertTo<GameModel>();

            return GameManager.GetGameModel().ConvertTo<CreateGameResponse>();
        }

        public ResetGameResponse Post(ResetGameRequest request)
        {
            throw new NotImplementedException();
        }

        public CreatePointResponse Post(CreatePointRequest request)
        {
            GameManager.AwardPoint(new PointModel { GameId = request.GameId, SideToAward = request.ScoringSide });

            return GameManager.GetGameModel().ConvertTo<CreatePointResponse>();
        }

        public RemoveLastPointResponse Delete(RemoveLastPointRequest request)
        {
            throw new NotImplementedException();
        }

        public CreateBounceResponse Post(CreateBounceRequest request)
        {
            GameManager.ProcessBounce(request.ConvertTo<BounceModel>());
            return GameManager.GetGameModel().ConvertTo<CreateBounceResponse>();
        }
    }
}
