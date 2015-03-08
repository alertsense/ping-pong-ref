using AlertSense.PingPongRef.Common.Interfaces;
using AlertSense.PingPongRef.Domain.Factories;
using AlertSense.PingPongRef.Model;
using System;

namespace AlertSense.PingPongRef.Domain
{
    public class GameManager : IGameManager
    {
        // Lazily instantiate a Game if one is not provided
        private Game _game;

        public Game Game
        {
            get { return _game ?? (_game = GameFactory.Create()); }
            set { _game = value; }
        }

        public void AwardPoint(Point point)
        {
            var playerAwarded = (int)point.SideToAward;
            Game.Players[playerAwarded].Score++;
            Game.Players[playerAwarded].History.Add(point);
            UpdateGameState();
        }

        public Side GetNextToServe()
        {
            Side server;

            // Determine who received first
            var initialReceiver = Game.InitialServer == Side.One ? Side.Two : Side.One;
            var playerOneScore = Game.Players[(int)Side.One].Score;
            var playerTwoScore = Game.Players[(int)Side.Two].Score;

            var pointNumber = playerOneScore + playerTwoScore + 1;

            if (pointNumber > 22)
            {
                server = IsOdd(pointNumber) ? initialReceiver : Game.InitialServer;
            }
            else  // score has not reached deuce
            {
                // check for odd point number
                if (IsOdd(pointNumber))
                {
                    pointNumber++;
                }
                server = IsOdd(pointNumber / 2) ? Game.InitialServer : initialReceiver;
            }

            return server;
        }

        private void UpdateGameState()
        {
            var playerOneScore = Game.Players[(int)Side.One].Score;
            var playerTwoScore = Game.Players[(int)Side.Two].Score;

            // throw exception if scores are invalid
            if ((playerOneScore > 10) && (playerTwoScore > 10) && (Math.Abs(playerOneScore - playerTwoScore) > 2))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (Math.Abs(playerOneScore - playerTwoScore) > 2)
            {
                if ((playerOneScore >= 11) || (playerTwoScore >= 11))
                {
                    Game.GameState = GameState.Complete;
                }
            }
        }

        private static bool IsOdd(int value)
        {
            return value % 2 > 0;
        }
    }
}