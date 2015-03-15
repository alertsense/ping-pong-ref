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

            model.Players = game.Players.ConvertAll<PlayerModel>(u => u.ConvertTo<PlayerModel>());
            model.Points = game.Players.ConvertAll<PointModel>(u => u.ConvertTo<PointModel>());
          //  model.Bounces = game.Players.ConvertAll<BounceModel>(u => u.ConvertTo<BounceModel>());

            return model;
        }
    }
}
