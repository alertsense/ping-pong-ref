using AlertSense.PingPong.Common.Messages;
using AlertSense.PingPong.Raspberry.IO;
using System;
using AlertSense.PingPong.ServiceModel;
using AlertSense.PingPong.ServiceModel.Enums;
using ServiceStack;
using AlertSense.PingPong.Raspberry.Models;
using RabbitMQ.Client;
using ServiceStack.Messaging;

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

        private RabbitMqWorker<CreateBounceResponse> _queueWorker;

        public void Start()
        {
            Board.DrawInititalScreen(Table1, Table2);
            Board.LogDebug("Connecting to Table1...");
            _table1 = OpenTableConnection(Table1);
            Board.LogDebug("Connecting to Table2...");
            _table2 = OpenTableConnection(Table2);

            Board.LogDebug("Connecting to the BounceMessage queue on " + Settings.RabbitMqHostName);
            ConnectToBounceQueue();

            Board.LogDebug("Connecting to the BounceMessageReceived queue...");
            _queueWorker = new RabbitMqWorker<CreateBounceResponse>(Settings);
            _queueWorker.MessageReceived += _queueWorker_MessageReceived;
            _queueWorker.MessageError += _queueWorker_MessageError;
            _queueWorker.Start();

            Board.LogDebug("Requesting a new game from the server...");
            _game = CreateGame();
            Board.UpdateGame(_game);

            UpdateTables();

            Board.LogInfo("Ready!");
        }

        void _queueWorker_MessageError(object sender, MessageErrorEventArgs e)
        {
            Board.LogDebug("Queue Worker Error: "  + e.Exception.ToString());
        }

        void _queueWorker_MessageReceived(object sender, MessageReceivedEventArgs<CreateBounceResponse> e)
        {
            Board.LogDebug("CreateBounceResponse Received");
            Table1.ServiceLight = e.Message.CurrentServer == Side.One;
            Table2.ServiceLight = e.Message.CurrentServer == Side.Two;
            UpdateTables();
        }

        private Game CreateGame()
        {
            Game game = null;
            try
            {
                var response = RestClient.Post(new CreateGameRequest());
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
                Board.LogError("Failed to create a new game via the server.  Creating a new local game.");
                Board.LogDebug(ex.ToString());
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
                    Board.LogDebug("Single Button Press: " + conn.Table.Name);
                    try
                    {
                        var response = RestClient.Post(new CreatePointRequest
                          {
                              GameId = _game.Id,
                              ScoringSide = conn.Table.Name == "Table1" ? Side.One : Side.Two
                          });
                        Table1.ServiceLight = response.CurrentServer == Side.One;
                        Table2.ServiceLight = response.CurrentServer == Side.Two;
                    }
                    catch (Exception ex)
                    {
                        Board.LogError("Failed to add a point. " + ex.Message);
                    }

                    UpdateTables();
                }
                else if (conn.Table.ButtonDuration > Settings.PressAndHoldTime)
                {
                    conn.Table.Message = "Button held down.";
                    Board.LogDebug("Long Button Press: " + conn.Table.Name);
                    try
                    {

                        var response = RestClient.Delete(new RemoveLastPointRequest()
                        {
                            GameId = _game.Id
                        });
                        Table1.ServiceLight = response.CurrentServer == Side.One;
                        Table2.ServiceLight = response.CurrentServer == Side.Two;
                    }
                    catch (Exception ex)
                    {
                        Board.LogError("Failed to remove last point. " + ex.Message);
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
                Board.LogDebug(String.Format("[{0}] Bounce [{1}] {2} {3}", conn.Table.Name, e.Count, e.Elapsed, sent ? "SENT" : "IGNORED"));
            }
            else
            {
               // Board.LogDebug(String.Format("{0}_Timeout", conn.Table.Name));
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
                Board.LogError("Failed to add a point. " + ex.Message);
            }
        }

        private void SendBounce(Table table)
        {
            if (!IsBounceQueueOpen())
            {
                Board.LogError("Failed to send BounceMessage.  Queue is not open.");
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
                Board.LogError("Failed to send the bounce message to the server.");
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
            if (_queueWorker != null)
                _queueWorker.Stop();
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
