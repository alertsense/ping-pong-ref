using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using AlertSense.PingPong.Common.Messages;
using AlertSense.PingPong.Raspberry.IO;
using System;
using AlertSense.PingPong.ServiceModel;
using AlertSense.PingPong.ServiceModel.Enums;
using ServiceStack;
using AlertSense.PingPong.Raspberry.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using ServiceStack.Messaging;
using ServiceStack.Text;

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
            Board.ShowMessage("Connecting to Table1...");
            _table1 = OpenTableConnection(Table1);
            Board.ShowMessage("Connecting to Table2...");
            _table2 = OpenTableConnection(Table2);

            Board.ShowMessage("Connecting to the bounce queue...");
            ConnectToBounceQueue();

            Board.ShowMessage("Requesting a new game from the server...");
            _game = CreateGame();
            Board.UpdateGame(_game);

            UpdateTables();

            Board.ShowMessage("Ready!");
        }

        private Game CreateGame()
        {
            Game game = null;
            try
            {
                var response = RestClient.Post(new CreateGameRequest());
                //Console.WriteLine(response.Dump());
                game = new Game
                {
                    Id = response.Id,
                    CurrentServingTable = response.CurrentServer == Side.One ? "Table1" : "Table2"
                };
                Table1.ServiceLight = response.CurrentServer == Side.One;
                Table2.ServiceLight = response.CurrentServer == Side.Two;
            }
            catch (Exception ex)
            {
                Board.ShowWarning("Failed to create a new game via the server.  Creating a new local game.");
                Console.WriteLine(ex);
                game = new Game { Id = Guid.NewGuid(), CurrentServingTable = "Table1" };
                Table1.ServiceLight = true;
                Table2.ServiceLight = false;
            }
            return game;
        }

        private void UpdateTables()
        {
            _table1.Update();
            _table2.Update();
            Board.UpdateTable(Table1);
            Board.UpdateTable(Table2);
        }

        private ITableConnection OpenTableConnection(Table table)
        {
            var conn = TableConnectionFactory.GetTableConnection(table);
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

                    UpdateTables();
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

                    UpdateTables();
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
            var conn = (ITableConnection)sender;
            if (!e.Timeout)
            {
                var sent = false;
                if (e.Elapsed > 50000)
                {                   
                    SendBounce(conn.Table);
                    sent = true;
                }
                Console.WriteLine("[{0}] Bounce [{1}] {2} {3}", conn.Table.Name, e.Count, e.Elapsed, sent ? "SENT" : "IGNORED");
            }
            else
            {
                Console.WriteLine("{0}_Timeout", conn.Table.Name);
                //SendMissingBounce(table.Table);
            }
        }

        
        

        private void SendMissingBounce(Table table)
        {
            try
            {
                var response = RestClient.Post(new CreateBounceRequest()
                {
                    GameId = _game.Id,
                    Side = Side.None
                });
                Table1.ServiceLight = response.CurrentServer == Side.One;
                Table2.ServiceLight = response.CurrentServer == Side.Two;

                UpdateTables();
            }
            catch (Exception ex)
            {
                Board.ShowWarning("Failed to add a point. " + ex.Message);
            }
        }

        private void SendBounce(Table table)
        {
            if (!IsBounceQueueOpen())
            {
                Board.ShowWarning("Failed to send BounceMessage.  Queue is not open.");
                return;
            }
                
            try
            {
                var message = new BounceMessage()
                {
                    GameId = _game.Id,
                    Side = table.Name == "Table1" ? Side.One : Side.Two
                };
                var payload = message.ToJson().ToUtf8Bytes();

                var props = _mqChannel.CreateBasicProperties();
                props.SetPersistent(true);

                _mqChannel.BasicPublish(
                    exchange: Exchange,
                    routingKey: QueueNames<BounceMessage>.In, 
                    basicProperties: props, 
                    body: payload);
            }
            catch (Exception)
            {
                Board.ShowWarning("Failed to send the bounce message to the server.");
            }            
        }

        private IConnection _mqConnection;
        private IModel _mqChannel;
        private const string Exchange = "mx.servicestack";

        private void ConnectToBounceQueue()
        {
            if (IsBounceQueueOpen())
                return;
            var mqFactory = new ConnectionFactory { HostName = Settings.RabbitMqHostName };
            _mqConnection = mqFactory.CreateConnection();
            _mqChannel = _mqConnection.CreateModel();   
            
        }

        private bool IsBounceQueueOpen()
        {
            return _mqChannel != null && _mqChannel.IsOpen;
        }

        public void Dispose()
        {
            if (_table1 != null)
                _table1.Close();
            if (_table2 != null)
                _table2.Close();
            if (_mqChannel != null && _mqChannel.IsOpen)
                _mqChannel.Close();
            if (_mqConnection != null && _mqConnection.IsOpen)
                _mqConnection.Close();
        }
    }
}
