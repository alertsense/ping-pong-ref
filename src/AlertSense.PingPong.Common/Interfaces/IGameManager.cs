using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;
using Point = AlertSense.PingPong.Common.Entities.Point;

namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IGameManager
    {
        GameModel GetGameModel();

        void AwardPoint(PointModel point);


        void AwardPoint(Point point);

        Side GetNextToServe();
    }
}