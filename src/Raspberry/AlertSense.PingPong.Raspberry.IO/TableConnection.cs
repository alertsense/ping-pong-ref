using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Raspberry.IO.GeneralPurpose;
using System.ComponentModel;

namespace AlertSense.PingPong.Raspberry.IO
{
    public interface ITableConnection: IDisposable
    {
        event EventHandler<BounceEventArgs> Bounce;
        TableSettings Settings { get; set; }
        void Open();
        void Close();
    }

    public class BounceEventArgs : EventArgs
    {
        public BounceType Type { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }

    public enum BounceType
    {
        Left,
        Right,
        Missing
    }

    public class TableConnection : ITableConnection
    {
        //private GpioConnection _gpioConnection;

        public event EventHandler<BounceEventArgs> Bounce;
        public TableSettings Settings { get; set; }

        System.ComponentModel.BackgroundWorker bounceWorker;


        public void Open()
        {
            if (Settings == null)
                throw new Exception("Settings must not be null");


            bounceWorker = new System.ComponentModel.BackgroundWorker();
            bounceWorker.WorkerSupportsCancellation = true;
            bounceWorker.WorkerReportsProgress = true;
            bounceWorker.DoWork += bounceWorker_DoWork;
            bounceWorker.ProgressChanged += bounceWorker_ProgressChanged;

            Settings.Driver = GpioConnectionSettings.DefaultDriver;


            bounceWorker.RunWorkerAsync();
            Console.WriteLine("Bounce worker running...");
            //_gpioConnection = new GpioConnection(new GpioConnectionSettings { Driver = Settings.Driver, PollInterval = 1});

            //var leftBouncePinConfig = Settings.LeftBouncePin.Input().OnStatusChanged(b => { if (b) Console.WriteLine("Left Bounce"); });
            ////TODO: Configure GPIO Pins from TableSettings
            //_gpioConnection.Add(leftBouncePinConfig);
            //_gpioConnection.Open();

        }

        void bounceWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            OnBounce((BounceEventArgs)e.UserState);
        }

        void bounceWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Console.WriteLine("Begin bounceWorker_DoWork");
            var worker = sender as BackgroundWorker;
            Settings.Driver.Allocate(Settings.LeftBouncePin, PinDirection.Input);
            Console.WriteLine("Allocated pin {0} for input.", Settings.LeftBouncePin);
            try
            {
                while (!worker.CancellationPending)
                {
                    if (Settings.Driver.Read(Settings.LeftBouncePin))
                        worker.ReportProgress(0, new BounceEventArgs { Type = BounceType.Left, ElapsedMilliseconds = -1 });
                    Thread.Sleep(100);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            finally
            {
                Settings.Driver.Release(Settings.LeftBouncePin);
                Console.WriteLine("Released pin {0}.", Settings.LeftBouncePin);
            }

            if (worker.CancellationPending)
                e.Cancel = true;
            Console.WriteLine("End bounceWorker_DoWork");
        }

        public void Close()
        {
            //_gpioConnection.Close();
            Console.WriteLine("Closing TableConnection");
            bounceWorker.CancelAsync();
        }

        public void Dispose()
        {
            Close();
        }

        private void OnBounce(BounceEventArgs e)
        {
            if (Bounce != null)
                Bounce(this, e);
        }
    }
}
