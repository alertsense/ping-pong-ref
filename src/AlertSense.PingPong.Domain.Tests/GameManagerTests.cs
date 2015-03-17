using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;
using NUnit.Framework;
using System;

namespace AlertSense.PingPong.Domain.Tests
{
    [TestFixture]
    public class GameManagerTests
    {
        // Verify algorithm which determines server works correctly
        [Test]
        public void PlayerOneCrushServerTest()
        {
            var manager = new GameManager();

            // Set initial server to side one
            manager.Game.InitialServer = Side.One;
            Assert.AreEqual(Side.One, manager.GetNextToServe());

            // award 1st point Score 1-0
            var point = new PointModel { SideToAward = Side.One };
            manager.AwardPoint(point);
            // SideOne serves 2nd point
            Assert.AreEqual(Side.One, manager.GetNextToServe());

            // award 2nd point  Score 2-0
            manager.AwardPoint(new PointModel { SideToAward = Side.One });
            // SideTwo serves 3rd point
            Assert.AreEqual(Side.Two, manager.GetNextToServe());

            // award 3nd point  Score 3-0
            manager.AwardPoint(new PointModel { SideToAward = Side.One });
            // SideTwo serves 4rd point
            Assert.AreEqual(Side.Two, manager.GetNextToServe());

            // award 4th point  Score 4-0
            manager.AwardPoint(new PointModel { SideToAward = Side.One });
            // SideOne serves 4th point
            Assert.AreEqual(Side.One, manager.GetNextToServe());

            // award 5th point  Score 5-0
            manager.AwardPoint(new PointModel { SideToAward = Side.One });
            // SideOne serves 5th point
            Assert.AreEqual(Side.One, manager.GetNextToServe());

            // award points 6-10
            for (int i = 6; i < 11; i++)
            {
                manager.AwardPoint(new PointModel { SideToAward = Side.One });
            }

            // initial receiver should server 11th point
            Assert.AreEqual(Side.Two, manager.GetNextToServe());
        }

        //Verify correct server is identified for 1st deuce point
        [Test]
        public void FirstDeucePoint()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)Side.One].Score = 11;
            manager.Game.Players[(int)Side.Two].Score = 11;

            //23rd point should be served by Initial Receiver
            Assert.AreNotEqual(manager.Game.InitialServer, manager.GetNextToServe());
        }

        //Verify correct server is identified for 1st add point
        [Test]
        public void FirstAddPoint()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)Side.One].Score = 12;
            manager.Game.Players[(int)Side.Two].Score = 11;

            //24th point should be served by Initial Server
            Assert.AreEqual(manager.Game.InitialServer, manager.GetNextToServe());
        }

        //Verify correct server is identified for 2nd deuce point
        [Test]
        public void SecondDeucePoint()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)Side.One].Score = 12;
            manager.Game.Players[(int)Side.Two].Score = 12;

            //23rd point should be served by Initial Receiver
            Assert.AreNotEqual(manager.Game.InitialServer, manager.GetNextToServe());
        }

        //Verify correct server is identified for 2nd add point
        [Test]
        public void SecondAddPoint()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)Side.One].Score = 12;
            manager.Game.Players[(int)Side.Two].Score = 13;

            //24th point should be served by Initial Server
            Assert.AreEqual(manager.Game.InitialServer, manager.GetNextToServe());
        }

        //Ensure exception is thrown for invalid score adjustment
        [Test]
        public void InvalidScoreTest()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)Side.One].Score = 12;
            manager.Game.Players[(int)Side.Two].Score = 14;

            // attempt to make score 12-15
            Assert.Throws<ArgumentOutOfRangeException>(() => manager.AwardPoint(new PointModel { SideToAward = Side.Two }));
        }

        //Verify game state changes to Complete when player achieves 11th point with 2 point margin
        [Test]
        public void EndOfGameTest()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)Side.One].Score = 10;
            manager.Game.Players[(int)Side.Two].Score = 7;

            Assert.AreEqual(manager.Game.GameState, GameState.InProgress);
            manager.AwardPoint(new PointModel { SideToAward = Side.One });

            Assert.AreEqual(manager.Game.GameState, GameState.Complete);
        }

        //Verify game state remains in progress when player achieves 11th point with 1 point margin
        [Test]
        public void ContinueGameTest()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)Side.One].Score = 10;
            manager.Game.Players[(int)Side.Two].Score = 10;

            Assert.AreEqual(manager.Game.GameState, GameState.InProgress);
            manager.AwardPoint(new PointModel { SideToAward = Side.One });

            Assert.AreEqual(manager.Game.GameState, GameState.InProgress);
        }

        [Test]
        public void VerifyRemoveLastPointAfterAwardingPointTest()
        {
            var manager = new GameManager();
            //award point to side one
            manager.AwardPoint(new PointModel { SideToAward = Side.One });

            var score = manager.GetScore();
            Assert.IsTrue(score.SideOne == 1);
            Assert.IsTrue(score.SideTwo == 0);

            manager.RemoveLastPoint();
            score = manager.GetScore();
            Assert.IsTrue(score.SideOne == 0);
        }

        [Test]
        public void VerifyRemoveLastPointBeforeScoring()
        {
            var manager = new GameManager();
            var score = manager.GetScore();
            Assert.IsTrue(score.SideOne == 0);
            Assert.IsTrue(score.SideTwo == 0);
            manager.RemoveLastPoint();
            score = manager.GetScore();
            Assert.IsTrue(score.SideOne == 0);
            Assert.IsTrue(score.SideTwo == 0);

        }

        [Test]
        public void VerifyRemovalOfCorrectPoint()
        {
            var manager = new GameManager();
            var score = manager.GetScore();
            Assert.IsTrue(score.SideOne == 0);
            Assert.IsTrue(score.SideTwo == 0);

            manager.AwardPoint(new PointModel { SideToAward = Side.One });
            manager.AwardPoint(new PointModel { SideToAward = Side.Two });
            manager.AwardPoint(new PointModel { SideToAward = Side.Two });

            manager.RemoveLastPoint();
            score = manager.GetScore();
            Assert.IsTrue(score.SideOne == 1);
            Assert.IsTrue(score.SideTwo == 1);

        }
    }
}