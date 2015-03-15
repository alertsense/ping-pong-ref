using System;
using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSense.PingPong.Domain.Tests
{
    [TestClass]
    public class GameManagerBounceTests
    {

        // Serve Bounce Tests -------------------------------------------------------------

        /// <summary>
        /// Server bounces ball on server side.
        /// Play continues.
        /// No score change.
        /// </summary>
        [TestMethod]
        public void ServerBouncesServerSide()
        {
            var manager = new GameManager();
            var bounce = new BounceModel {Side = manager.Game.InitialServer};
            Assert.IsTrue(manager.Game.IsServe);
            manager.ProcessBounce(bounce);
            Assert.IsFalse(manager.Game.IsServe);
            Assert.AreEqual(manager.Game.Players[0].Score, manager.Game.Players[0].Score);
            Assert.AreEqual(manager.Game.Players[0].Score,0);

            Assert.AreEqual(manager.Game.CurrentPoint.Bounces.Count, 1);
        }


        /// <summary>
        /// Server bounces ball on receiver side without bouncing on server side first
        /// Point awarded to receiver.
        /// </summary>
        [TestMethod]
        public void ServerBouncesReceiverSide()
        {
            var manager = new GameManager();
            var bounce = new BounceModel { Side = manager.Game.NotStriker };
            Assert.IsTrue(manager.Game.IsServe);
            manager.ProcessBounce(bounce);
            // Next bounce will be a serve
            Assert.IsTrue(manager.Game.IsServe);
            Assert.AreEqual(manager.Game.Players[(int)manager.Game.NotStriker].Score, 1);

            var points = manager.Game.Points;
            Assert.AreEqual(points[0].Bounces.Count, 1);
        }
        


        /// <summary>
        /// Server serves ball off table
        /// Point awarded to receiver.
        /// </summary>
        [TestMethod]
        public void ServerBouncesOffTable()
        {
            var manager = new GameManager();
            var bounce = new BounceModel { Side = Side.None };
            Assert.IsTrue(manager.Game.IsServe);
            Assert.AreEqual(manager.Game.InitialServer, manager.Game.Striker);
            manager.ProcessBounce(bounce);
            // Next bounce will be a serve
            Assert.IsTrue(manager.Game.IsServe);
            // intial server and receiver do not change after 1st point
            Assert.AreEqual(manager.Game.Players[(int)manager.Game.NotStriker].Score, 1);

            var points = manager.Game.Points;
            // off the table bounce is not counted
            Assert.AreEqual(points[0].Bounces.Count, 0);


        }


        // Service Return Bounce Tests  ----------------------------------------------------

        [TestMethod]
        public void ReceiverSuccessfullyReturnsServe()
        {
            var manager = new GameManager();
            var intialReceiver = manager.Game.NotStriker;
            var bounce = new BounceModel { Side = manager.Game.InitialServer };
            Assert.IsTrue(manager.Game.IsServe);
            // the serve - bounce server side 
            manager.ProcessBounce(bounce);
            // bounce receiver side
            Assert.IsFalse(manager.Game.IsServe);
            bounce.Side = intialReceiver;
            manager.ProcessBounce(bounce);
            Assert.IsFalse(manager.Game.IsServe);
            Assert.AreEqual(manager.Game.Striker, intialReceiver);
            // receiver successfully returns serve
            bounce.Side = manager.Game.InitialServer;
            manager.ProcessBounce(bounce);
            Assert.AreEqual(manager.Game.Striker, manager.Game.InitialServer);
            Assert.IsFalse(manager.Game.IsServe);
            Assert.AreEqual(manager.Game.Players[0].Score, manager.Game.Players[0].Score);
            Assert.AreEqual(manager.Game.Players[0].Score, 0);



        }

        [TestMethod]
        public void ReceiverReturnBouncesSameSide()
        {
            var manager = new GameManager();
            var intialReceiver = manager.Game.NotStriker;
            var bounce = new BounceModel { Side = manager.Game.InitialServer };
            Assert.IsTrue(manager.Game.IsServe);
            // the serve - bounce server side 
            manager.ProcessBounce(bounce);
            // bounce receiver side
            Assert.IsFalse(manager.Game.IsServe);
            bounce.Side = intialReceiver;
            manager.ProcessBounce(bounce);

            Assert.IsFalse(manager.Game.IsServe);
            Assert.AreEqual(manager.Game.Striker, intialReceiver);
            // receiver unsuccessfully returns serve
            bounce.Side = intialReceiver;
            manager.ProcessBounce(bounce);
            // server remains the same
            Assert.AreEqual(manager.Game.Striker, manager.Game.InitialServer);

            // next bounce will be a new point
            Assert.IsTrue(manager.Game.IsServe);

            // server leads 1-0
            Assert.AreEqual(manager.Game.Players[(int)manager.Game.InitialServer].Score, 1);
            Assert.AreEqual(manager.Game.Players[(int)intialReceiver].Score, 0);

            var points = manager.Game.Points;
            // off the table bounce is not counted
            Assert.AreEqual(points[0].Bounces.Count, 3);

        }

        [TestMethod]
        public void ReceiverReturnsOffTable()
        {
            var manager = new GameManager();
            var intialReceiver = manager.Game.NotStriker;
            var bounce = new BounceModel { Side = manager.Game.InitialServer };
            Assert.IsTrue(manager.Game.IsServe);
            // the serve - bounce server side 
            manager.ProcessBounce(bounce);
            // bounce receiver side
            Assert.IsFalse(manager.Game.IsServe);
            bounce.Side = intialReceiver;
            manager.ProcessBounce(bounce);

            Assert.IsFalse(manager.Game.IsServe);
            Assert.AreEqual(manager.Game.Striker, intialReceiver);
            // receiver unsuccessfully returns serve
            bounce.Side = Side.None;
            manager.ProcessBounce(bounce);
            // server remains the same
            Assert.AreEqual(manager.Game.Striker, manager.Game.InitialServer);

            // next bounce will be a new point
            Assert.IsTrue(manager.Game.IsServe);

            // server leads 1-0
            Assert.AreEqual(manager.Game.Players[(int)manager.Game.InitialServer].Score, 1);
            Assert.AreEqual(manager.Game.Players[(int)intialReceiver].Score, 0);

            var points = manager.Game.Points;
            // off the table bounce is not counted
            Assert.AreEqual(points[0].Bounces.Count, 2);

        }


        // Ralley Tests (Post Serve)------------------------------------------------------------
       
        [TestMethod]
        public void StrikerBouncesSameSide()
        {
            var manager = new GameManager();
            var striker = manager.Game.Striker;
            var receiver = manager.Game.NotStriker;
            manager.Game.IsServe = false;
            var bounce = new BounceModel { Side = manager.Game.Striker };
            manager.ProcessBounce(bounce);

            // receiver is awarded point
            // next bounce will be a new point
            Assert.IsTrue(manager.Game.IsServe);

            // receiver leads 1-0
            Assert.AreEqual(manager.Game.Players[(int)receiver].Score, 1);
            Assert.AreEqual(manager.Game.Players[(int)striker].Score, 0);
            var points = manager.Game.Points;
          
            Assert.AreEqual(points[0].Bounces.Count, 1);
        }


    }
}
