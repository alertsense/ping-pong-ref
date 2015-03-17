//#define GameFactoryManager
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
using AlertSense.PingPong.Domain;

namespace AlertSense.PingPong.ServiceInterface
{
    public class GameService : Service, IGameService
    {
#if GameFactoryManager
        public IGameManagerFactory GameManagerFactory { get; set; }
#else
        public IGameManager GameManager { get; set; }
#endif
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
#if GameFactoryManager
            var GameManager = GameManagerFactory.GetNewGameManager();
#endif

            return GameManager.GetGameModel().ConvertTo<CreateGameResponse>();
        }

        public ResetGameResponse Post(ResetGameRequest request)
        {
            throw new NotImplementedException();
        }

        public CreatePointResponse Post(CreatePointRequest request)
        {
#if GameFactoryManager
            var GameManager = GameManagerFactory.GetGameManagerByGameId(request.GameId);
#endif

            GameManager.AwardPoint(new PointModel { GameId = request.GameId, SideToAward = request.ScoringSide });

            return GameManager.GetGameModel().ConvertTo<CreatePointResponse>();
        }

        /// <summary>
        /// Remove last point that was awarded.
        /// Do nothing if no points have been awarded.
        /// Adjust player score impacted by point removal.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RemoveLastPointResponse Delete(RemoveLastPointRequest request)
        {
#if GameFactoryManager
            var GameManager = GameManagerFactory.GetGameManagerByGameId(request.GameId);
#endif

            GameManager.RemoveLastPoint();

            return GameManager.GetGameModel().ConvertTo<RemoveLastPointResponse>();
        }

        public CreateBounceResponse Post(CreateBounceRequest request)
        {
#if GameFactoryManager
            var GameManager = GameManagerFactory.GetGameManagerByGameId(request.GameId);
#endif

            GameManager.ProcessBounce(request.ConvertTo<BounceModel>());

            return GameManager.GetGameModel().ConvertTo<CreateBounceResponse>();
        }
    }
}
