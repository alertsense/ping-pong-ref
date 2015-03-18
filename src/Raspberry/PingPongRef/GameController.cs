using AlertSense.PingPong.Raspberry.IO;
using System;
using ServiceStack;
using AlertSense.PingPong.Raspberry.Models;

namespace AlertSense.PingPong.Raspberry
{

    public class GameController : IDisposable
    {
        public Table Table1 { get; set; }
        public Table Table2 { get; set; }
        public GameSettings Settings { get; set; }
        public GameBoard Board { get; set; }
        public IRestClient RestClient { get; set; }

        private ITableConnection _table1;
        private ITableConnection _table2;

        private Game _game;

        public void Start()
        {
            Board.DrawInititalScreen(Table1, Table2);

            _game = CreateGame();
            Board.UpdateGame(_game);
            _table1 = OpenTableConnection(Table1);
            _table2 = OpenTableConnection(Table2);
        }

        private Game CreateGame()
        {
            Game game = null;
            try
            {
                var response = RestClient.Post(new AlertSense.PingPong.ServiceModel.CreateGameRequest());
                game = new Game
                {
                    Id = response.Id,
                    CurrentServingTable = response.CurrentServer == ServiceModel.Enums.Side.One ? "Table1" : "Table2"
                };
            }
            catch (Exception ex)
            {
                Board.ShowWarning("Failed to create a new game via the server.  Creating a new local game.");
                game = new Game { Id = Guid.NewGuid(), CurrentServingTable = "Table1" };
            }
            return game;
        }

        private ITableConnection OpenTableConnection(Table table)
        {
            var conn = ConnectionFactory.GetTableConnection(table);
            conn.Bounce += Table_Bounce;
            conn.ButtonPressed += Table_ButtonPressed;
            conn.Open();
            return conn;
        }

        void Table_ButtonPressed(object sender, ButtonEventArgs e)
        {
            var conn = (ITableConnection)sender;
            conn.Table.ButtonState = e.Enabled;

            if (!e.Enabled && conn.Table.ButtonLastPressed.HasValue)
            {
                conn.Table.ButtonDuration = DateTime.Now.Subtract(conn.Table.ButtonLastPressed.Value).TotalMilliseconds;

                if (conn.Table.ButtonDuration < Settings.ButtonClickTime)
                {
                    conn.Table.Message = "Button pressed once";

                    try
                    {
                        var response = RestClient.Post(new AlertSense.PingPong.ServiceModel.CreatePointRequest
                          {
                              GameId = _game.Id,
                              ScoringSide = conn.Table.Name == "Table1" ? ServiceModel.Enums.Side.One : ServiceModel.Enums.Side.Two
                          });
                        Table1.ServiceLight = response.CurrentServer == ServiceModel.Enums.Side.One;
                        Table2.ServiceLight = response.CurrentServer == ServiceModel.Enums.Side.Two;
                    }
                    catch (Exception ex)
                    {
                        Board.ShowWarning("Failed to add a point. " + ex.Message);
                    }
                    
                    _table1.Update();
                    _table2.Update();
                }
                else if (conn.Table.ButtonDuration > Settings.PressAndHoldTime)
                {
                    conn.Table.Message = "Button held down.";
                    try
                    {

                        var response = RestClient.Post(new AlertSense.PingPong.ServiceModel.RemoveLastPointRequest()
                        {
                            GameId = _game.Id
                        });
                        Table1.ServiceLight = response.CurrentServer == ServiceModel.Enums.Side.One;
                        Table2.ServiceLight = response.CurrentServer == ServiceModel.Enums.Side.Two;
                    }
                    catch (Exception ex)
                    {
                        Board.ShowWarning("Failed to remove last point. " + ex.Message);
                    }

                    _table1.Update();
                    _table2.Update();
                }
                conn.Table.ButtonLastPressed = null;
            }
            else
            {
                conn.Table.ButtonLastPressed = DateTime.Now;
            }
            
            Board.UpdateTable(conn.Table);
        }

        void Table_Bounce(object sender, BounceEventArgs e)
        {
            var table = (ITableConnection)sender;
            Console.WriteLine("{0}_Bounce", table.Table.Name);
        }

        public void Dispose()
        {
            if (_table1 != null)
                _table1.Close();
            if (_table2 != null)
                _table2.Close();
        }
    }
}
