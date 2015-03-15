using System.Collections;
using System.Collections.Generic;
using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;


namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IGameManager
    {
        GameModel GetGameModel();

        void AwardPoint(PointModel point);

     //   Side GetNextToServe();

        void ProcessBounce(BounceModel bounce);

        ScoreModel GetScore();

        IList<PointModel> GetPoints();
    }
}