using AlertSense.PingPongRef.Model;

namespace AlertSense.PingPongRef.Common.Interfaces
{
    public interface IGameManager
    {
        void AwardPoint(Point point);

        Side GetNextToServe();
    }
}