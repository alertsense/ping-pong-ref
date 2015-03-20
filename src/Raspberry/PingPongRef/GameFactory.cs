using AlertSense.PingPong.Raspberry.Models;
using ServiceStack;

namespace AlertSense.PingPong.Raspberry
{
    public static class GameFactory
    {
        public static GameController CreateGame()
        {
            var restClient = new JsonServiceClient(GameSettings.Default.ApiUri)
            {
                Proxy = null, 
                DisableAutoCompression = true
            };
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
                RestClient = restClient
            };
        }
    }
}
