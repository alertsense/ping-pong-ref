using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Raspberry.IO.GeneralPurpose;
using System.ComponentModel;
using AlertSense.PingPong.Raspberry.Models;

namespace AlertSense.PingPong.Raspberry.IO
{
    public class TableConnectionRaw : ITableConnection
    {
        //private GpioConnection _gpioConnection;

        public event EventHandler<BounceEventArgs> Bounce;
        public event EventHandler<ButtonEventArgs> ButtonPressed;
        public TableSettings Settings { get; set; }

        System.ComponentModel.BackgroundWorker bounceWorker;

        private bool lastButtonValue;

        public void Open()
        {
            if (Settings == null)
                throw new Exception("Settings must not be null");
            Settings.Driver = GpioConnectionSettings.DefaultDriver;
            Settings.Driver.Allocate(Settings.LedPin, PinDirection.Output);
            Console.WriteLine("Allocated pin {0} for output.", Settings.LedPin);

            bounceWorker = new System.ComponentModel.BackgroundWorker();
            bounceWorker.WorkerSupportsCancellation = true;
            bounceWorker.WorkerReportsProgress = true;
            bounceWorker.DoWork += bounceWorker_DoWork;
            bounceWorker.ProgressChanged += bounceWorker_ProgressChanged;
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
            if (e.UserState is ButtonEventArgs)
                OnButtonPressed((ButtonEventArgs)e.UserState);
            else if (e.UserState is BounceEventArgs)
                OnBounce((BounceEventArgs)e.UserState);
        }

        void bounceWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Console.WriteLine("Begin bounceWorker_DoWork");
            var worker = sender as BackgroundWorker;
            Settings.Driver.Allocate(Settings.ButtonPin, PinDirection.Input);
            Console.WriteLine("Allocated pin {0} for input.", Settings.ButtonPin);
            try
            {
                while (!worker.CancellationPending)
                {
                    var buttonValue = Settings.Driver.Read(Settings.ButtonPin);

                    if (lastButtonValue != buttonValue)
                    {
                        lastButtonValue = buttonValue;
                        worker.ReportProgress(0, new ButtonEventArgs(buttonValue));
                        Thread.Sleep(100);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            finally
            {
                Settings.Driver.Release(Settings.ButtonPin);
                Console.WriteLine("Released pin {0}.", Settings.ButtonPin);
            }

            if (worker.CancellationPending)
                e.Cancel = true;
            Console.WriteLine("End bounceWorker_DoWork");
        }

        public void Close()
        {
            //_gpioConnection.Close();
            Settings.Driver.Release(Settings.LedPin); 
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

        private void OnButtonPressed(ButtonEventArgs e)
        {
            if (ButtonPressed != null)
                ButtonPressed(this, e);
        }
        
        public void Led(bool on)
        {
            Settings.Driver.Write(Settings.LedPin, on);
        }

        public string Name { get; set; }

        public Table Table
        {
            get;
            set;
        }


        public void Update()
        {
            Led(Table.ServiceLight);
        }
    }
}
