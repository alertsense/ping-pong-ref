using AlertSense.PingPong.Common.Messages;
using AlertSense.PingPong.Raspberry.IO;
using AlertSense.PingPong.Raspberry.Models;
using AlertSense.PingPong.ServiceModel;
using AlertSense.PingPong.ServiceModel.Enums;
using AlertSense.PingPong.ServiceModel.Models;
using RabbitMQ.Client;
using ServiceStack;
using ServiceStack.Messaging;
using System;

namespace AlertSense.PingPong.Raspberry
{

    public class GameController : IDisposable
    {
        public Table Table1 { get; set; }
        public Table Table2 { get; set; }
        public GameSettings Settings { get; set; }
        public GameBoard Board { get; set; }
        public IRestClient RestClient { get; set; }
        public Game Game { get; set; }

        private ITableConnection _table1;
        private ITableConnection _table2;

        private RabbitMqWorker<CreateBounceResponse> _queueWorker;

        public void Start()
        {
            Board.DrawInititalScreen(Table1, Table2);

            Board.LogDebug("Connecting to table components...");
            _table1 = OpenTableConnection(Table1);
            _table2 = OpenTableConnection(Table2);

            Board.LogDebug("Connecting to the mq:BounceMessage.inq...");
            ConnectToBounceQueue();

            Board.LogDebug("Subscribing to the mq:CreateBounceResponse.inq...");
            _queueWorker = new RabbitMqWorker<CreateBounceResponse>(Settings);
            _queueWorker.MessageReceived += MessageReceived;
            _queueWorker.MessageError += MessageError;
            _queueWorker.Start();

            Board.LogDebug("Requesting a new game from the server...");
            var gameModel = CreateGame();
            Game.Id = gameModel.Id;

            Board.LogDebug("Updating table components...");
            UpdateTables(gameModel);
            Board.UpdateGame(Game);

            Board.LogInfo("Ready!");
        }

        void MessageError(object sender, MessageErrorEventArgs e)
        {
            Board.LogDebug("Queue Worker Error: "  + e.Exception.ToString());
        }

        void MessageReceived(object sender, MessageReceivedEventArgs<CreateBounceResponse> e)
        {
            Board.LogDebug("CreateBounceResponse Received");
            UpdateTables((GameModel)e.Message);
        }

        private GameModel CreateGame()
        {
            try
            {
                return (GameModel) RestClient.Post(new CreateGameRequest());
            }
            catch (Exception ex)
            {
                Board.LogError("Failed to create a new game via the server.");
                Board.LogDebug(ex.ToString());
                throw;
            }
        }

        private void UpdateTables(GameModel model)
        {
            Table1.ServiceLight = model.CurrentServer == Side.One;
            Table2.ServiceLight = model.CurrentServer == Side.Two;

            // Update table components
            _table1.Update();
            _table2.Update();

            // Update the console
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
                              GameId = Game.Id,
                              ScoringSide = conn.Table.Name == "Table1" ? Side.One : Side.Two
                          });

                        UpdateTables((GameModel)response);
                    }
                    catch (Exception ex)
                    {
                        Board.LogError("Failed to add a point. " + ex.Message);
                    }

                    
                }
                else if (conn.Table.ButtonDuration > Settings.PressAndHoldTime)
                {
                    conn.Table.Message = "Button held down.";
                    Board.LogDebug("Long Button Press: " + conn.Table.Name);
                    try
                    {

                        var response = RestClient.Delete(new RemoveLastPointRequest()
                        {
                            GameId = Game.Id
                        });
                        UpdateTables((GameModel)response);
                    }
                    catch (Exception ex)
                    {
                        Board.LogError("Failed to remove last point. " + ex.Message);
                    }
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
                if (e.Elapsed > Settings.IgnoreBounceTime)
                {      
                    SendBounce(conn.Table);
                    sent = true;
                }
                Board.LogDebug(String.Format("[{0}] Bounce [{1}] {2} {3}", conn.Table.Name, e.Count, e.Elapsed, sent ? "SENT" : "IGNORED"));
            }
            else
            {
               // Board.LogDebug(String.Format("{0}_Timeout", conn.Table.Name));
               // SendMissingBounce(table.Table);
            }
        }

        private void SendMissingBounce(Table table)
        {
            try
            {
                var response = RestClient.Post(new CreateBounceRequest()
                {
                    GameId = Game.Id,
                    Side = Side.None
                });

                UpdateTables((GameModel) response);
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
                    GameId = Game.Id,
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

            var mqFactory = new ConnectionFactory { HostName = Settings.RabbitMqHostName, UserName = Settings.RabbitMqUsername, Password = Settings.RabbitMqPassword };
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
