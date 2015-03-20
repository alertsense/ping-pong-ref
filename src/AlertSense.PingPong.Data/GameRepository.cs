using AlertSense.PingPong.Common.Entities;
using AlertSense.PingPong.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using ServiceStack.Data;
using System.Diagnostics;

namespace AlertSense.PingPong.Data
{
    public class GameRepository : IGameRepository, IDisposable
    {
        public IDbConnectionFactory ConnectionFactory { get; set; }

        private IDbConnection _db;

        protected IDbConnection DbConnection
        {
            get { return _db ?? (_db = ConnectionFactory.OpenDbConnection()); }
            set { _db = value; }
        }
        
        public void Dispose()
        {
            if (DbConnection != null && DbConnection.State != ConnectionState.Closed)
            {
                DbConnection.Close();
                DbConnection.Dispose();
            }
        }

        public List<Game> GetAllGames()
        {
            return DbConnection.Select<Game>();
        }

        public Game GetGameById(Guid id)
        {
            var game = DbConnection.SingleById<Game>(id);
            return game;
        }

        public Game GetGameById(Guid id, bool IncludeReferences = true)
        {
            var game = DbConnection.LoadSingleById<Game>(id);
            return game;
        }

        public Game SaveGame(Game game)
        {
            Debug.WriteLine("Saving game with Id: {0}", game.Id);

            DbConnection.Save<Game>(game, references: true);
            DbConnection.Save<Point>(game.CurrentPoint, references: true);

            foreach (var player in game.Players)
                DbConnection.Save<Player>(player, references: true);

            foreach (var point in game.Points)
                DbConnection.Save<Point>(point, references: true);

            return game;
        }

        public bool DeletePoint(Point point)
        {
            foreach(var bounce in point.Bounces)
                DbConnection.Delete<Bounce>(bounce);

            return DbConnection.Delete<Point>(point) > 0;
        }

        public bool DeleteGame(Game game)
        {
            throw new NotImplementedException();
        }

        public Point SavePoint(Point point)
        {
            throw new NotImplementedException();
        }

        public Bounce SaveBounce(Bounce bounce)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBounce(Bounce bounce)
        {
            throw new NotImplementedException();
        }


    }
}
