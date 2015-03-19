using AlertSense.PingPong.ServiceModel;
using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;
using NUnit.Framework;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        [Ignore]
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
                Assert.That(gameState.Players[0].Score, Is.EqualTo(i + 1));
            }

            Assert.That(gameState, Is.Not.Null);
            Assert.That(gameState.GameState, Is.EqualTo(GameState.Complete));


            //var gameState = Client.Post(new CreateBounceRequest { GameId = game.Id });


        }

        [Test]
        public void CreateNewGame()
        {
            var game = Client.Post(new CreateGameRequest { });
            Assert.That(game, Is.Not.Null);
            Debug.WriteLine("New GameId: {0}", game.Id);

        }

        [Test]
        public  void CreateNewGameAndBounce()
        {
            var newGame = Client.Post(new CreateGameRequest { });

            var game = Client.Get(new GetGameRequest { GameId = newGame.Id });
            Assert.That(game, Is.Not.Null);
            Debug.WriteLine("Existing GameId: {0}", game.Id);

            GameModel gameState = game as GameModel;
            gameState = BounceBall(gameState, 15);

            Assert.That(gameState.Players.Count, Is.EqualTo(2));
            Assert.That(gameState.Players[(int)newGame.InitialServer].Score, Is.EqualTo(1));
        }

        bool localhost = true;

        [Test]
        //[Ignore]
        public void GameBounceSequenceTest()
        {
            if (localhost)
            {
                //Client = new JsonServiceClient("http://localhost/api/");
                Client = new JsonServiceClient("http://localhost:60461/api/");
            }

            var game = Client.Post(new CreateGameRequest { });
            Assert.That(game, Is.Not.Null);

            Assert.That(game.GameState, Is.EqualTo(GameState.InProgress));

            GameModel gameState = game as GameModel;

            int serverPlayerIndex = (int)game.InitialServer;

            // player 1 score
            gameState = BounceBall(gameState, 4);
            // player 1 score
            gameState = BounceBall(gameState, 4);
            // player 2 score
            gameState = BounceBall(gameState, 3);
            // player 1 score
            gameState = BounceBall(gameState, 4);
            // 3 - 1
            // player 1 score
            gameState = BounceBall(gameState, 4);
            // player 1 score
            gameState = BounceBall(gameState, 4);
            // player 2 score
            gameState = BounceBall(gameState, 3);
            // player 1 score
            gameState = BounceBall(gameState, 4);
            // 6 - 2
            // player 1 score
            gameState = BounceBall(gameState, 4);
            // player 1 score
            gameState = BounceBall(gameState, 4);
            // player 2 score
            gameState = BounceBall(gameState, 3);
            // player 1 score
            gameState = BounceBall(gameState, 4);
            // 9 - 3
            // player 1 score
            gameState = BounceBall(gameState, 4);
            // player 1 score
            gameState = BounceBall(gameState, 4);

            // make sure the score we intend to get is correct
            Assert.That(gameState, Is.Not.Null);
            Assert.That(gameState.Players[serverPlayerIndex].Score, Is.EqualTo(11));

            // player 2 score
            gameState = BounceBall(gameState, 3);
            // player 1 score
            gameState = BounceBall(gameState, 4);
            // 11 - 4
            // verify that we can't go over a winning score
            Assert.That(gameState.Players[serverPlayerIndex].Score, Is.EqualTo(11));
            Assert.That(gameState, Is.Not.Null);
            Assert.That(gameState.GameState, Is.EqualTo(GameState.Complete));


            //var gameState = Client.Post(new CreateBounceRequest { GameId = game.Id });


        }
        Random bounceTime = new Random();

        public GameModel BounceBall(GameModel game, int bounces)
        {
            Debug.WriteLine("Serving from side {0}", game.CurrentServer);

            GameModel gameState = Client.Post(new CreateBounceRequest { GameId = game.Id, Side = game.CurrentServer });

            for (int i = 1; i <= bounces; i++)
            {
                if (i != bounces)
                    gameState = Client.Post(new CreateBounceRequest { GameId = game.Id, Side = gameState.NotStriker });
                else
                    gameState = Client.Post(new CreateBounceRequest { GameId = game.Id, Side = Side.None });

                if (localhost)
                    //Thread.Sleep(bounceTime.Next(40, 100));
                Thread.Sleep(bounceTime.Next(340, 450));
            }

            Debug.WriteLine("\tPlayer 1: {0} to Player 2: {1}", gameState.Players[0].Score, gameState.Players[1].Score);


            return gameState;


        }
    }
}
