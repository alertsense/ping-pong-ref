using AlertSense.PingPong.ServiceModel.Enums;
using System;
using System.Collections.Generic;
using AlertSense.PingPong.ServiceModel.Models;
using System.Diagnostics;

namespace AlertSense.PingPong.Domain.Factories
{
    public class GameFactory
    {
        /// <summary>
        /// Initial Game State Definition
        ///    Two Players are initialized
        ///    InitialServer is designated
        ///    GameState is InProgress
        ///    Striker is the server
        ///    First bounce is the serve
        ///    Game Score is 0-0
        /// </summary>
        /// <returns></returns>
        public static GameModel Create()
        {

            var game = new GameModel
            {
                Id = Guid.NewGuid(),
                InitialServer = ChooseInitialServer(),
                Players = new List<PlayerModel>
                        {
                            new PlayerModel { Id = Guid.NewGuid(), Name = "Player One", Score = 0, History = new List<PointModel>() },
                            new PlayerModel { Id = Guid.NewGuid(), Name = "Player Two", Score = 0, History = new List<PointModel>() }
                        },
                Points = new List<PointModel>(),
                GameState = GameState.InProgress,
            };
            game.Striker = game.InitialServer;
            game.IsServe = true;
            game.CurrentPoint = new PointModel { GameId = game.Id };

            game.CurrentServer = game.InitialServer;
            Debug.WriteLine("Creating new game model with Id: {0}", game.Id);
            return game;
        }

        private static readonly Random Coin = new Random();

        private static Side ChooseInitialServer()
        {
            var choice = Side.One;
            var toss = Coin.Next(int.MaxValue) % 2;
            if (toss == 1)
            {
                choice = Side.Two;
            }

            return choice;
        }
    }
}