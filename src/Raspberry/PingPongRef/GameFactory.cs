using AlertSense.PingPong.Raspberry.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Raspberry
{
    public static class GameFactory
    {
        public static GameController CreateGame()
        {
            var table1 = new Table
                {
                    Name = "Table1",
                    Settings = TableSettings.Table1,
                    ServiceLight = false
                };
            var table2 = new Table
                {
                    Name = "Table2",
                    Settings = TableSettings.Table2,
                    ServiceLight = false
                };
            return new GameController
            {
                Board = new GameBoard(),
                Settings = GameSettings.Default,
                Table1 = table1,
                Table2 = table2,
                RestClient = new JsonServiceClient(GameSettings.Default.ApiUri)
            };
        }
    }
}
