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

namespace AlertSense.PingPong.Data
{
    public class GameRepository : IGameRepository
    {
        public IDbConnectionFactory ConnectionFactory { get; set; }

        private IDbConnection _db;

        protected IDbConnection DbConnection
        {
            get { return _db ?? (_db = ConnectionFactory.OpenDbConnection()); }
            set { _db = value; }
        }

        public List<Game> GetAllGames()
        {
            return DbConnection.Select<Game>();
        }

        public Game GetGameById(Guid id)
        {
            return DbConnection.SingleById<Game>(id);
        }

        public Game GetGameById(Guid id, bool IncludeReferences = true)
        {
            return DbConnection.SingleById<Game>(id);
        }

        public Game SaveGame(Game game)
        {
            DbConnection.Save<Game>(game, references: true);

            foreach(var point in game.Points)
                DbConnection.Save<Point>(point, references: true);

            return game;
        }

        public bool DeleteGame(Game game)
        {
            throw new NotImplementedException();
        }

        public Point SavePoint(Point point)
        {
            throw new NotImplementedException();
        }

        public bool DeletePoint(Point point)
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
