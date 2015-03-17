using AlertSense.PingPong.Common.Extensions;
using AlertSense.PingPong.Common.Interfaces;
using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Domain.Factories
{    public class GameManagerFactory : IGameManagerFactory
    {
        public IGameRepository GameRepository { get; set; }
        public IGameManager GetGameManagerByGameId(Guid gameId)
        {
            var game = GameRepository.GetGameById(gameId, IncludeReferences: true).ToGameModel();
            var gameManager = new GameManager
            {
                Game = game,
                GameRepository = GameRepository
            };
            return gameManager;
        }

        public IGameManager GetNewGameManager()
        {
            return new GameManager
            {
                GameRepository = GameRepository
            };
        }
    }
}
