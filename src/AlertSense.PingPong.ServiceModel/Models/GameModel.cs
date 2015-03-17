using System;
using System.Collections.Generic;
using AlertSense.PingPong.ServiceModel.Enums;

namespace AlertSense.PingPong.ServiceModel.Models
{
    public class GameModel
    {
        public GameModel()
        {
            Players = new List<PlayerModel>();
            Points = new List<PointModel>();
           
        }

        public Guid Id { get; set; }
        public List<PlayerModel> Players { get; set; }
        public List<PointModel> Points { get; set; }

        private PointModel currentPoint;

        public PointModel CurrentPoint
        {
            get { return currentPoint; }
            set { currentPoint = value; }
        }

        public Side InitialServer { get; set; }
        public Side CurrentServer { get; set; }

        public bool IsServe { get; set; }
        public GameState GameState { get; set; }

        // Side Hitting the ball
        public Side Striker { get; set; }

        public Side NotStriker
        {
            get {
                return Striker == Side.One ? Side.Two : Side.One;
            }
        }


        /// <summary>
        /// Desigante the next player to hit the ball which then specifies a valid bounce.
        /// </summary>
        public void ChangeStriker()
        {
            Striker = Striker == Side.One ? Side.Two : Side.One;
        }


        /// <summary>
        /// Remove last awarded point and adjsut corresponding player's score
        /// </summary>
        public void RemoveLastPoint()
        {
            if (Points.Count > 0)
            {
                var lastPoint = Points[Points.Count - 1];
                Points.RemoveAt(Points.Count - 1);

                //Adjust score to account for point removal
                Players[(int)lastPoint.SideToAward].Score--;  
            }
        }
    }
}