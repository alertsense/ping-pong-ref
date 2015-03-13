using AlertSense.PingPong.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IGameRepository
    {
        List<Game> GetAllGames();
        Game GetGameById(Guid id, bool IncludeReferences = true);
        Game SaveGame(Game game);
        bool DeleteGame(Game game);

        Point SavePoint(Point point);
        bool DeletePoint(Point point);

        Bounce SaveBounce(Bounce bounce);
        bool DeleteBounce(Bounce bounce);
    }
}
