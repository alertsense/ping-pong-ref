using AlertSense.PingPong.ServiceModel.Enums;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace AlertSense.PingPong.ServiceModel
{
    public class GameModel
    {
        public Guid GameId { get; set; }
        public PlayerModel[] Players { get; set; }
        public List<PointModel> Points { get; set; }
        public List<BounceModel> Bounces { get; set; }
        public Side InitialServer { get; set; }
        public GameState GameState { get; set; }
    }

    [Route("/Games","GET")]
    public class GameSummaryRequest : IReturn<GameSummaryResponse>
    {

    }

    public class GameSummaryResponse : IList<GameSummary>
    {

    }

    // don't include points or bounces, just score summary
    public class GameSummary
    {
        public Guid Id { get; set; }
        public PlayerModel[] Players { get; set; }
    }

    [Route("/Games/{Id}","GET")]
    public class GameRequest : IReturn<GameResponse>
    {

    }




    [Route("/Games", "POST", Summary = "Start a new Ping Pong game.")]
    public class CreateGameRequest : IReturn<GameResponse>
    {

    }

    public class GameResponse : GameModel
    {

    }




    [Route("/Games/{Id}/Point", "POST", Summary = "Create a new point associated with a game.")]
    public class CreatePointRequest : IReturn<CreatePointResponse>
    {

    }

    public class CreatePointResponse : GameResponse
    {

    }




    [Route("/Games/{Id}/Bounce", "POST", Summary = "Create a new bounce associated with a game.")]
    public class CreateBounceRequest : IReturn<CreateBounceResponse>
    {

    }

    public class CreateBounceResponse : GameResponse
    {

    }






}