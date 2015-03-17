using System.Collections.Generic;
using AlertSense.PingPong.ServiceModel.Models;


namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IGameManager
    {
        GameModel GetGameModel();

        void AwardPoint(PointModel point);

        void ProcessBounce(BounceModel bounce);

        ScoreModel GetScore();

        IList<PointModel> GetPoints();

        void RemoveLastPoint();
    }
}