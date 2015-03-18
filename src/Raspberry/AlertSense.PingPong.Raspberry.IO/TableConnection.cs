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
        System.ComponentModel.BackgroundWorker _bounceWorker;

        public event EventHandler<BounceEventArgs> Bounce;
        public event EventHandler<ButtonEventArgs> ButtonPressed;
        public Table Table { get; set; }

        public void Open()
        {
            if (Table.Settings == null)
                throw new Exception("Settings must not be null");

            var settings = Table.Settings;
            var buttonConfig = settings.ButtonPin.Input().Name(ButtonName).OnStatusChanged(Button_StatusChanged);
            var bounceConfig = settings.BouncePin.Input().Name(BounceName).OnStatusChanged(Bounce_StatusChanged);
            var ledConfig = settings.LedPin.Output().Name(LedName);

            _gpioConnection = new GpioConnection(new GpioConnectionSettings { Driver = settings.Driver, PollInterval = 0.01m});
            _gpioConnection.Add(buttonConfig);
            _gpioConnection.Add(ledConfig);
            _gpioConnection.Add(bounceConfig);
            _gpioConnection.Open();


            //_bounceWorker = new System.ComponentModel.BackgroundWorker
            //{
            //    WorkerSupportsCancellation = true,
            //    WorkerReportsProgress = true
            //};
            //_bounceWorker.DoWork += bounceWorker_DoWork;
            //_bounceWorker.ProgressChanged += bounceWorker_ProgressChanged;
            //_bounceWorker.RunWorkerAsync();
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
            //_bounceWorker.CancelAsync();
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

        void bounceWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            Console.WriteLine("Bounce");
            var args = e.UserState as BounceEventArgs;
            if (args != null)
                OnBounce(args);
        }

        void bounceWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var pin = Table.Settings.BouncePin;
            Table.Settings.Driver.Allocate(pin, PinDirection.Input);
            Console.WriteLine("Allocating pin " + pin);
            try
            {
                while (!worker.CancellationPending)
                {
                    if (Table.Settings.Driver.Read(pin))
                        worker.ReportProgress(0, new BounceEventArgs());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            finally
            {
                Table.Settings.Driver.Release(pin);
            }

            if (worker.CancellationPending)
                e.Cancel = true;;
        }
    }
}
