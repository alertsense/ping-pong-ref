using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AlertSense.PingPong.Domain
{
    [HubName("spectator")]
    public class SpectateHub : Hub
    {
        // This class is empty because the client will not be calling any methods on the server.
    }
}