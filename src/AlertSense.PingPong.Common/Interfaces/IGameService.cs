using AlertSense.PingPong.ServiceModel;

namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IGameService
    {
        // /Games        
        GameSummaryResponse Post(GetGameSummaryRequest request);

        // /Games/{Id}
        GameResponse Post(GetGameRequest request);

        // /Games/
        CreateGameResponse Post(CreateGameRequest request);

        // /Games/{Id}/Reset
        ResetGameResponse Post(ResetGameRequest request);


        // Used In 1st Code Camp Session To Manually Increment Point
        //Games/{Id}/Point
        CreatePointResponse Post(CreatePointRequest request);

        // /Games/{Id}/Point
        RemoveLastPointResponse Delete(RemoveLastPointRequest request);

        // /Games/{Id}/Bounce
        CreateBounceResponse Post(CreateBounceRequest request);
    }
}
