using AlertSense.PingPong.Domain.Factories;
using AlertSense.PingPong.ServiceModel.Enums;
using NUnit.Framework;
using System.Linq;

namespace AlertSense.PingPong.Domain.Tests.Factories
{
    [TestFixture]
    public class GameFactoryTests
    {
        [Test]
        public void Create_Test()
        {
            var newGame = GameFactory.Create();

            Assert.That(newGame.Id, Is.Not.Null);
            Assert.That(newGame.GameState, Is.EqualTo(GameState.InProgress));
            Assert.That(newGame.InitialServer, Is.Not.Null);

            Assert.That(newGame.Players.Count(), Is.EqualTo(2));
            Assert.That(newGame.Players[0].Score, Is.EqualTo(0));
            Assert.That(newGame.Players[0].Score, Is.EqualTo(0));

            Assert.That(newGame.InitialServer, Is.EqualTo(newGame.Striker));

            Assert.IsTrue(newGame.IsServe);
        }
    }
}