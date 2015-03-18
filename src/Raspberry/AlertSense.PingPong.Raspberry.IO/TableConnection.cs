using System;
using Raspberry.IO.GeneralPurpose;
using AlertSense.PingPong.Raspberry.Models;

namespace AlertSense.PingPong.Raspberry.IO
{
    public class TableConnection : ITableConnection
    {
        private GpioConnection _gpioConnection;

        public event EventHandler<BounceEventArgs> Bounce;
        public event EventHandler<ButtonEventArgs> ButtonPressed;
        public Table Table { get; set; }

        public void Open()
        {
            if (Table.Settings == null)
                throw new Exception("Settings must not be null");

            var settings = Table.Settings;
            var buttonConfig = settings.ButtonPin.Input().Name(ButtonName).OnStatusChanged(Button_StatusChanged);
            var ledConfig = settings.LedPin.Output().Name(LedName);

            _gpioConnection = new GpioConnection(new GpioConnectionSettings { Driver = settings.Driver });
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
            get { return Table.Name + "_Button"; }
        }

        string LedName
        {
            get { return Table.Name + "_Led"; }
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
        
        public void Update()
        {
            Led(Table.ServiceLight);
        }
    }
}
