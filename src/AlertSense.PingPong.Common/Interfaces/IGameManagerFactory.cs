﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IGameManagerFactory
    {
        IGameManager GetNewGameManager();
        IGameManager GetGameManagerByGameId(Guid gameId);
    }
}
