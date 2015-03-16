using AlertSense.PingPong.ServiceModel.Models;
using System.Collections.Generic;

namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IGameManager
    {
        GameModel GetGameModel();

        GameModel CreateGame();

        void AwardPoint(PointModel point);

        void ProcessBounce(BounceModel bounce);

        ScoreModel GetScore();

        IList<PointModel> GetPoints();

        void RemoveLastPoint();
    }
}