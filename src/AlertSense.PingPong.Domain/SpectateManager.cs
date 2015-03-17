using AlertSense.PingPong.Common.Interfaces;
using AlertSense.PingPong.ServiceModel.Models;
using Microsoft.AspNet.SignalR;

namespace AlertSense.PingPong.Domain
{
    public class SpectateManager : ISpectateManager
    {
        public void Update(GameModel model)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<SpectateHub>();
            if (hub != null)
            {
                hub.Clients.All.update(model);
            }
        }
    }
}