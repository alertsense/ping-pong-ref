//#define GameFactoryManager
using AlertSense.PingPong.Common.Interfaces;
using AlertSense.PingPong.Domain;
using AlertSense.PingPong.ServiceModel;
using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack;

using System;

namespace AlertSense.PingPong.ServiceInterface
{
    public class GameService : Service, IGameService
    {
#if GameFactoryManager
        public IGameManagerFactory GameManagerFactory { get; set; }
#else
        public IGameManager GameManager { get; set; }
#endif
        public ISpectateManager SpectateManager { get; set; }

        public GameSummaryResponse Post(GetGameSummaryRequest request)
        {
            throw new NotImplementedException();
        }

        public GameResponse Get(GetGameRequest request)
        {
#if GameFactoryManager
            var GameManager = GameManagerFactory.GetGameManagerByGameId(request.GameId);
#endif
            return GameManager.GetGameModel().ConvertTo<GameResponse>();
        }

        public CreateGameResponse Post(CreateGameRequest request)
        {
#if GameFactoryManager
            var GameManager = GameManagerFactory.GetNewGameManager();
#endif
            var gameModel = GameManager.CreateGame();
            SpectateManager.Update(gameModel);
            return gameModel.ConvertTo<CreateGameResponse>();
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
            var gameModel = GameManager.GetGameModel();
            SpectateManager.Update(gameModel);

            return gameModel.ConvertTo<CreatePointResponse>();
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
            var gameModel = GameManager.GetGameModel();
            SpectateManager.Update(gameModel);

            return gameModel.ConvertTo<RemoveLastPointResponse>();
        }

        public CreateBounceResponse Post(CreateBounceRequest request)
        {
#if GameFactoryManager
            var GameManager = GameManagerFactory.GetGameManagerByGameId(request.GameId);
#endif

            GameManager.ProcessBounce(request.ConvertTo<BounceModel>());
            var gameModel = GameManager.GetGameModel();
            SpectateManager.Update(gameModel);

            return gameModel.ConvertTo<CreateBounceResponse>();
        }
    }
}
