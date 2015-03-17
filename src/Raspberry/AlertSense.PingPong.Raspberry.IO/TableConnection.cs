using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Raspberry.IO.GeneralPurpose;
using System.ComponentModel;

namespace AlertSense.PingPong.Raspberry.IO
{
    public class TableConnection : ITableConnection
    {
        private GpioConnection _gpioConnection;

        public event EventHandler<BounceEventArgs> Bounce;
        public event EventHandler<ButtonEventArgs> ButtonPressed;
        public TableSettings Settings { get; set; }

        public string Name { get; set; }

        public void Open()
        {
            if (Settings == null)
                throw new Exception("Settings must not be null");

            _gpioConnection = new GpioConnection(new GpioConnectionSettings { Driver = Settings.Driver });

            var buttonConfig = Settings.LeftButtonPin.Input().Name(ButtonName).OnStatusChanged(Button_StatusChanged);
            Console.WriteLine("Pin {0} configured for input.", Settings.LeftButtonPin);
            var ledConfig = Settings.LeftLedPin.Output().Name(LedName);
            Console.WriteLine("Pin {0} configured for output.", Settings.LeftLedPin);
            _gpioConnection.Add(buttonConfig);
            _gpioConnection.Add(ledConfig);
            _gpioConnection.Open();

        }

        void Button_StatusChanged(bool value)
        {
            OnButtonPressed(new ButtonEventArgs(value));
        }

        string ButtonName
        {
            get { return Name + "_Button"; }
        }

        string LedName
        {
            get { return Name + "_Led"; }
        }
        
        public void Close()
        {
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
    }
}
