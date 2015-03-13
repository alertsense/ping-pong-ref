using AlertSense.PingPong.ServiceModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public void SimpleGameSequenceTest()
        {
            var game = Client.Post(new CreateGameRequest { });
            Assert.That(game, Is.Not.Null);

            var gameState = Client.Post(new CreatePointRequest { GameId = game.Id });

            //var gameState = Client.Post(new CreateBounceRequest { GameId = game.Id });

            Assert.That(gameState, Is.Not.Null);
            Assert.That(gameState.Points, Is.Not.Empty);

        }
    }
}
