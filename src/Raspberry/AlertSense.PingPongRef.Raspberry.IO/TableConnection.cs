using System;
using Raspberry.IO.GeneralPurpose;

namespace AlertSense.PingPongRef.Raspberry.IO
{
    public interface ITableConnection: IDisposable
    {
        TableSettings Settings { get; set; }
        void Open();
        void Close();
    }

    public class TableConnection : ITableConnection
    {
        //TODO: Add left and right bounce events

        private GpioConnection _gpioConnection;

        public TableSettings Settings { get; set; }

        public void Open()
        {
            if (Settings == null)
                throw new Exception("Settings must not be null");

            _gpioConnection = new GpioConnection(new GpioConnectionSettings { Driver = Settings.Driver });

            //TODO: Configure GPIO Pins from TableSettings
        }

        public void Close()
        {
            _gpioConnection.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
