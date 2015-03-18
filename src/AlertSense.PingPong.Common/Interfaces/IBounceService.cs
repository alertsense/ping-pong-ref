using AlertSense.PingPong.Common.Messages;

namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IBounceService
    {
        void Post(BounceMessage message);
    }
}