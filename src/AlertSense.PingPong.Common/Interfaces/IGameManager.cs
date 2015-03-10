using AlertSense.PingPong.ServiceModel.Enums;
using Point = AlertSense.PingPong.Common.Entities.Point;

namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IGameManager
    {
        void AwardPoint(Point point);

        Side GetNextToServe();
    }
}