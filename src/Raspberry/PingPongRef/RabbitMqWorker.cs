using AlertSense.PingPong.Raspberry.Models;
using RabbitMQ.Client;
using ServiceStack;
using ServiceStack.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AlertSense.PingPong.Raspberry
{
    public class RabbitMqWorker<T>
    {
        public event EventHandler<MessageReceivedEventArgs<T>> MessageReceived;
        public event EventHandler<MessageErrorEventArgs> MessageError;

        private BackgroundWorker _worker;
        private GameSettings _settings;

        public RabbitMqWorker(GameSettings settings)
        {
            _settings = settings;
        }

        public void Start()
        {
            _worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            _worker.DoWork += DoWork;
            _worker.ProgressChanged += ProgressChanged;
            _worker.RunWorkerCompleted += RunWorkerCompleted;
            _worker.RunWorkerAsync();
        }

        public void Stop()
        {
            if (_worker != null)
                _worker.CancelAsync();
        }

        void DoWork(object sender, DoWorkEventArgs e)
        {
            var mqFactory = new ConnectionFactory { HostName = _settings.RabbitMqHostName, UserName = _settings.RabbitMqUsername, Password = _settings.RabbitMqPassword };
            using (var connection = mqFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", "mx.servicestack"},
                        {"x-dead-letter-routing-key", QueueNames<T>.Dlq}
                    };
                    var queueName = QueueNames<T>.In;
                    channel.QueueDeclare(queueName, true, false, false, parameters);
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName, false, consumer);

                    while (true && !_worker.CancellationPending)
                    {
                        var ea = consumer.Queue.Dequeue();
                        var message = Encoding.UTF8.GetString(ea.Body).FromJson<T>();

                        _worker.ReportProgress(0, new MessageReceivedEventArgs<T>(message));

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            }

            if (_worker != null && _worker.CancellationPending)
                e.Cancel = true;
        }

        void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                OnMessageException(new MessageErrorEventArgs(e.Error));
            if (e.Cancelled)
                Console.Write("RabbitMqConsumer Stopped");
        }

        void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var args = e.UserState as MessageReceivedEventArgs<T>;
            if (args != null)
                OnMessageReceived(args);
        }

        private void OnMessageReceived(MessageReceivedEventArgs<T> eventArgs)
        {
            if (MessageReceived != null)
                MessageReceived(this, eventArgs);
        }

        private void OnMessageException(MessageErrorEventArgs eventArgs)
        {
            if (MessageError != null)
                MessageError(this, eventArgs);
        }
    }

    public class MessageReceivedEventArgs<T> : EventArgs
    {
        public MessageReceivedEventArgs(T message)
        {
            Message = message;
        }
        public T Message { get; set; }
    }

    public class MessageErrorEventArgs : EventArgs
    {
        public MessageErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
        public Exception Exception { get; set; }
    }
}
