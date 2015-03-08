using AlertSense.PingPongRef.Domain.Factories;
using AlertSense.PingPongRef.Model;
using NUnit.Framework;
using System.Linq;

namespace AlertSense.PingPongRef.Domain.Tests.Factories
{
    [TestFixture]
    public class GameFactoryTests
    {
        [Test]
        public void Create_Test()
        {
            var newGame = GameFactory.Create();

            Assert.That(newGame.GameId, Is.Not.Null);
            Assert.That(newGame.GameState, Is.EqualTo(GameState.InProgress));
            Assert.That(newGame.InitialServer, Is.Not.Null);

            Assert.That(newGame.Players.Count(), Is.EqualTo(2));
            Assert.That(newGame.Players[0].Score, Is.EqualTo(0));
            Assert.That(newGame.Players[0].Score, Is.EqualTo(0));
        }
    }
}