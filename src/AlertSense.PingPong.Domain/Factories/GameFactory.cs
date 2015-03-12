using AlertSense.PingPong.Common.Entities;
using AlertSense.PingPong.ServiceModel.Enums;
using System;
using System.Collections.Generic;

namespace AlertSense.PingPong.Domain.Factories
{
    public class GameFactory
    {
        public static Game Create()
        {
            return new Game
            {
                Id = new Guid(),
                InitialServer = ChooseInitialServer(),
                Players = new List<Player>
                        {
                            new Player { Name = "Player One", Score = 0, History = new List<Point>() },
                            new Player { Name = "Player Two", Score = 0, History = new List<Point>() }
                        },
                GameState = GameState.InProgress
            };
        }

        private static Side ChooseInitialServer()
        {
            var choice = Side.One;
            var coin = new Random();
            var toss = coin.Next(0, 1);
            if (toss == 1)
            {
                choice = Side.Two;
            }

            return choice;
        }
    }
}