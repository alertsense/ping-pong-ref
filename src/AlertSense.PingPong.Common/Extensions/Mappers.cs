using AlertSense.PingPong.Common.Entities;
using AlertSense.PingPong.ServiceModel.Models;
using ServiceStack;

namespace AlertSense.PingPong.Common.Extensions
{
    public static class Mappers
    {
        public static GameModel ToGameModel(this Game game)
        {
            var model = game.ConvertTo<GameModel>();

            model.Players = game.Players.ConvertAll(u => u.ConvertTo<PlayerModel>());
            model.Points = game.Points.ConvertAll(u => u.ToPointModel());
            model.CurrentPoint = game.CurrentPoint.ToPointModel();
            //  model.Bounces = game.Players.ConvertAll<BounceModel>(u => u.ConvertTo<BounceModel>());

            return model;
        }

        public static Game ToGameEntity(this GameModel gameModel)
        {
            var game = gameModel.ConvertTo<Game>();

            game.Players = gameModel.Players.ConvertAll(u => u.ConvertTo<Player>());
            game.Points = gameModel.Points.ConvertAll(u => u.ToPoint());
            game.CurrentPoint = gameModel.CurrentPoint.ToPoint();
            //  model.Bounces = game.Players.ConvertAll<BounceModel>(u => u.ConvertTo<BounceModel>());

            return game;
        }

        public static Point ToPoint(this PointModel pointModel)
        {
            var point = pointModel.ConvertTo<Point>();

            point.Bounces = pointModel.Bounces.ConvertAll(u => u.ConvertTo<Bounce>());
            return point;
        }

        public static PointModel ToPointModel(this Point point)
        {
            var pointModel = point.ConvertTo<PointModel>();

            pointModel.Bounces = point.Bounces.ConvertAll(u => u.ConvertTo<BounceModel>());
            return pointModel;
        }

    }
}
