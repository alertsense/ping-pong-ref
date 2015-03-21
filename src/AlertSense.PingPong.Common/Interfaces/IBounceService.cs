using AlertSense.PingPong.Common.Messages;
using AlertSense.PingPong.ServiceModel;

namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IBounceService
    {
        CreateBounceResponse Post(BounceMessage message);
    }
}