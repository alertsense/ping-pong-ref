using AlertSense.PingPong.ServiceModel;
using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.ServiceInterface.IntegrationTests.IntegrationTests
{
    public class GameSequenceTest : IntegrationTestBase
    {
        public override void InitTestSession()
        {

        }

        public override void InitTest()
        {

        }


        [Test]
        public void GamePointSequenceTest()
        {
            var game = Client.Post(new CreateGameRequest { });
            Assert.That(game, Is.Not.Null);

            Assert.That(game.GameState, Is.EqualTo(GameState.InProgress));

            GameModel gameState = null;


            for (int i = 0; i < 12; i++)
            {
                gameState = Client.Post(new CreatePointRequest { GameId = game.Id, ScoringSide = Side.One });
                
                Assert.That(gameState, Is.Not.Null);
                Assert.That(gameState.Points, Is.Not.Empty);
                Assert.That(gameState.Players[0].Score, Is.EqualTo(i+1));
            }

            Assert.That(gameState, Is.Not.Null);
            Assert.That(gameState.GameState, Is.EqualTo(GameState.Complete));


            //var gameState = Client.Post(new CreateBounceRequest { GameId = game.Id });


        }

        [Test]
        public void GameBounceSequenceTest()
        {
            var game = Client.Post(new CreateGameRequest { });
            Assert.That(game, Is.Not.Null);

            Assert.That(game.GameState, Is.EqualTo(GameState.InProgress));

            GameModel gameState = game as GameModel;

            while (game.GameState != GameState.Complete)
            { 
                gameState = BounceBall(game, 4);
            }            

            Assert.That(gameState, Is.Not.Null);
            Assert.That(gameState.GameState, Is.EqualTo(GameState.Complete));


            //var gameState = Client.Post(new CreateBounceRequest { GameId = game.Id });


        }

        public GameModel BounceBall(GameModel game, int bounces)
        {
            var startingSide = game.InitialServer;
            Side oppositeSide = Side.One;

            if (startingSide == Side.One)
                oppositeSide = Side.Two;

            GameModel gameState = null;

            Debug.WriteLine("Serving from side {0}", startingSide);


            for (int i = 0; i <= bounces; i++ )
            {
                var bounceSide = i % 2 == 0 ? startingSide : oppositeSide;

                if (i != bounces)
                    gameState = Client.Post(new CreateBounceRequest { GameId = game.Id, Side = bounceSide });
                else
                    gameState = Client.Post(new CreateBounceRequest { GameId = game.Id, Side = Side.None });

                Debug.WriteLine("\tPlayer 1: {0} to Player 2: {1}", gameState.Players[0].Score, gameState.Players[1].Score);

                
            }

            return gameState;


        }
    }
}
