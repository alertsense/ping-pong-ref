using System;
using System.ComponentModel;
using System.Threading;
using Raspberry.IO.GeneralPurpose;
using AlertSense.PingPong.Raspberry.Models;

namespace AlertSense.PingPong.Raspberry.IO
{
    public class TableConnection : ITableConnection
    {
        private GpioConnection _gpioConnection;
        BackgroundWorker _bounceWorker;

        public event EventHandler<BounceEventArgs> Bounce;
        public event EventHandler<ButtonEventArgs> ButtonPressed;
        public Table Table { get; set; }

        public void Open()
        {
            if (Table.Settings == null)
                throw new Exception("Settings must not be null");

            var settings = Table.Settings;
            var buttonConfig = settings.ButtonPin.Input().Name(ButtonName).OnStatusChanged(Button_StatusChanged);
            //var bounceConfig = settings.BouncePin.Input().Name(BounceName).OnStatusChanged(Bounce_StatusChanged);
            var ledConfig = settings.LedPin.Output().Name(LedName);

            _gpioConnection = new GpioConnection(new GpioConnectionSettings { Driver = settings.Driver, PollInterval = 0.01m});
            _gpioConnection.Add(buttonConfig);
            _gpioConnection.Add(ledConfig);
            //_gpioConnection.Add(bounceConfig);
            _gpioConnection.Open();


            Log("Open: BounceWorker Initializing...");
            _bounceWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            _bounceWorker.DoWork += BounceWorker_DoWork;
            _bounceWorker.ProgressChanged += BounceWorker_ProgressChanged;
            _bounceWorker.RunWorkerCompleted += BounceWorkerOnRunWorkerCompleted;
            _bounceWorker.RunWorkerAsync();
            Log("Open: BounceWorker Ready.");
            
        }


        void Button_StatusChanged(bool value)
        {
            OnButtonPressed(new ButtonEventArgs(value));
        }

        void Bounce_StatusChanged(bool value)
        {
            if (value)
                OnBounce(new BounceEventArgs());
        }

        string ButtonName
        {
            get { return Table.Name + "_Button"; }
        }

        string BounceName
        {
            get { return Table.Name + "_Bounce"; }
        }
        

        string LedName
        {
            get { return Table.Name + "_Led"; }
        }
        
        public void Close()
        {
            if (_bounceWorker != null)
                _bounceWorker.CancelAsync();
            if (_gpioConnection != null)
                _gpioConnection.Close();
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
            _gpioConnection[LedName] = on;
        }
        
        public void Update()
        {
            Led(Table.ServiceLight);
        }



        private void BounceWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var bouncePin = Table.Settings.BouncePin;
            var driver = GpioConnectionSettings.DefaultDriver;
            Log("BounceWorker_DoWork: Allocating bounce pin " + bouncePin + " for input...");
            driver.Allocate(bouncePin, PinDirection.Input);
            Log("BounceWorker_DoWork: Bounce pin " + bouncePin + "  allocated.");
            try
            {
                while (worker != null && !worker.CancellationPending)
                {
                    try
                    {
                        Log("Waiting for a bounce...");
                        driver.Wait(bouncePin, true, Table.Settings.BounceTimeout);
                        Log("Bounce detected.");
                        worker.ReportProgress(0, new BounceEventArgs());
                    }
                    catch (TimeoutException)
                    {
                        Log("Timeout waiting for bounce");
                        worker.ReportProgress(0, new BounceEventArgs(true));
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
            }
            finally
            {
                driver.Release(bouncePin);
            }

            if (worker != null && worker.CancellationPending)
                e.Cancel = true;
        }

        private void BounceWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Log("BounceWorker_ProgressChanged: Bounce");
            var args = e.UserState as BounceEventArgs;
            if (args != null)
                OnBounce(args);
        }


        private void BounceWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                Log("Error: " + e.Error.Message);
            if (e.Cancelled)
                Log("Cancelled");
        }

        private void Log(string message)
        {
            //Console.WriteLine("{0}: {1}", Table.Name, message);
        }
    }
}
