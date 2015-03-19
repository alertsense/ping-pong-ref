using AlertSense.PingPong.Common.Extensions;
using AlertSense.PingPong.Common.Interfaces;
using AlertSense.PingPong.Domain.Factories;
using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlertSense.PingPong.Domain
{
	public class GameManager : IGameManager
	{
		public IGameRepository GameRepository { get; set; }

		// Lazily instantiate a Game if one is not provided
		private GameModel _game;

		public GameModel Game
		{
			get
			{
				if (_game == null)
					_game = GameFactory.Create();
				return _game;
			}
			set { _game = value; }
		}

		public GameModel CreateGame()
		{
			Game = GameFactory.Create();

			if (GameRepository != null)
				GameRepository.SaveGame(Game.ToGameEntity());

			return Game;
		}

		/// <summary>
		/// Return the game score
		/// </summary>
		/// <returns></returns>
		public ScoreModel GetScore()
		{
			var score = new ScoreModel
			{
				SideOne = Game.Players[(int)Side.One].Score,
				SideTwo = Game.Players[(int)Side.Two].Score
			};

			return score;
		}

		/// <summary>
		/// Return list of game points
		/// </summary>
		/// <returns></returns>
		public IList<PointModel> GetPoints()
		{
			return Game.Points;
		}

		/// <summary>
		/// Process the bounce data delivered from Pi
		/// </summary>
		/// <param name="bounce"></param>
		public void ProcessBounce(BounceModel bounce)
		{
			// don't process anymore bounces after the game is considered complete
			if (Game.GameState == GameState.Complete)
				return;

			Debug.WriteLine("Bounce, GameId: {0}", Game.Id);

			bounce.Ticks = DateTime.UtcNow.Ticks;

			if (bounce.Side != Side.None)
			{
				Game.CurrentPoint.Bounces.Add(bounce);
			}
			Debug.WriteLine("Bounce on side {0}", bounce.Side);
			// Special case the serve
			if (Game.IsServe)
			{
				// check for off the table bounce or receiver side bounce
				if (bounce.Side != Game.Striker)
				{
					// award point to the receiver
					Game.CurrentPoint.SideToAward = Game.NotStriker;
					AwardPoint(Game.CurrentPoint);
				}
				else
				{
					// otherwise the ball bounced on the server side as expected and play continues
					Game.IsServe = false;
				}

				// striker does not change
			}
			else  // Not the serve
			{
				// Handle off the table bounce
				if (bounce.Side == Side.None || bounce.Side == Game.Striker)
				{
					// award point to the non-striker
					Game.CurrentPoint.SideToAward = Game.NotStriker;
					AwardPoint(Game.CurrentPoint);
				}
				else //play continues
				{
					Game.Striker = Game.Striker == Side.One ? Side.Two : Side.One;
				}
			}
			//if (GameRepository != null)
			//    GameRepository.SaveGame(Game.ToGameEntity());
		}

		/// <summary>
		/// Determine who servers next based upon game score and who served first in the game
		/// </summary>
		/// <returns></returns>
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

		public GameModel GetGameModel()
		{
			if (GameRepository != null)
				GameRepository.SaveGame(Game.ToGameEntity());

			return Game;
		}

		public void AwardPoint(PointModel point)
		{
			// don't process anymore points after the game is considered complete
			if (Game.GameState == GameState.Complete)
				return;

			point.Ticks = DateTime.UtcNow.Ticks;

			var playerAwarded = (int)point.SideToAward;

			var player = Game.Players[playerAwarded];

			player.Score++;
			player.History.Add(point);
			point.PlayerId = player.Id;

			Game.Points.Add(point);
			if (!IsGameOver())
			{
				Game.CurrentServer = GetNextToServe();
				Game.Striker = Game.CurrentServer;
				// next bounce is a serve
				Game.IsServe = true;
				Game.CurrentPoint = new PointModel { GameId = Game.Id };
			}
		}

		/// <summary>
		/// Remove last awarded point and adjust corresponding player's score
		/// </summary>
		public void RemoveLastPoint()
		{
			if (Game.Points.Count > 0)
			{
				var lastPoint = Game.Points[Game.Points.Count - 1];
				Game.Points.RemoveAt(Game.Points.Count - 1);

				//Adjust score to account for point removal
				Game.Players[(int)lastPoint.SideToAward].Score--;

				if (lastPoint != null && GameRepository != null)
					GameRepository.DeletePoint(lastPoint.ToPoint());
			}
		}

		/// <summary>
		/// Determine if end of game conditions are met.
		/// </summary>
		/// <returns></returns>
		private bool IsGameOver()
		{
			var playerOneScore = Game.Players[(int)Side.One].Score;
			var playerTwoScore = Game.Players[(int)Side.Two].Score;

			// throw exception if scores are invalid
			if ((playerOneScore > 10) && (playerTwoScore > 10) && (Math.Abs(playerOneScore - playerTwoScore) > 2))
			{
				throw new ArgumentOutOfRangeException();
			}

			if (Math.Abs(playerOneScore - playerTwoScore) < 2) return false;
			if ((playerOneScore < 11) && (playerTwoScore < 11)) return false;
			Game.GameState = GameState.Complete;
			return true;
		}

		private static bool IsOdd(int value)
		{
			return value % 2 > 0;
		}
	}
}