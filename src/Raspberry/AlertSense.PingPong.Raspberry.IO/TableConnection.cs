using System;
using System.Threading;
using Raspberry.IO.GeneralPurpose;

namespace AlertSense.PingPong.Raspberry.IO
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

            //Settings.Driver = GpioConnectionSettings.DefaultDriver;
            var driver = GpioConnectionSettings.DefaultDriver;
            driver.Allocate(Settings.LeftReadyPin, PinDirection.Output);
            driver.Allocate(Settings.LeftButtonPin, PinDirection.Input);
            //for (int i = 0; i < 10; i++)
            while(true)
            {
                driver.Wait(Settings.LeftButtonPin, true, 1000 * 60);
                driver.Write(Settings.LeftReadyPin, true);
                Thread.Sleep(200);
                driver.Wait(Settings.LeftButtonPin, false, 1000 * 60);
                
                driver.Wait(Settings.LeftButtonPin, true, 1000 * 60);
                driver.Write(Settings.LeftReadyPin, false);
                Thread.Sleep(200);
                driver.Wait(Settings.LeftButtonPin, false, 1000 * 60);
            }

            driver.Release(Settings.LeftReadyPin);
            //Console.WriteLine("Allocate Input Pin");
            //Settings.Driver = GpioConnectionSettings.DefaultDriver;
            //Settings.Driver.Allocate(Settings.LeftBouncePin,PinDirection.Input);
            //Console.WriteLine("Pin Ready");
            //var cnt = 0;
            //try
            //{
            //    while (true)
            //    {
            //        Console.WriteLine("Waiting for a bounce...");
            //        Settings.Driver.Wait(Settings.LeftBouncePin, true, 1000 * 60);
            //        cnt++;
            //        Console.WriteLine("Bounce {0}", cnt);
            //    }
            //}
            //finally
            //{
            //    Settings.Driver.Release(Settings.LeftBouncePin);
            //}
            
            //_gpioConnection = new GpioConnection(new GpioConnectionSettings { Driver = Settings.Driver, PollInterval = 1});

            //var leftBouncePinConfig = Settings.LeftBouncePin.Input().OnStatusChanged(b => { if (b) Console.WriteLine("Left Bounce"); });
            ////TODO: Configure GPIO Pins from TableSettings
            //_gpioConnection.Add(leftBouncePinConfig);
            //_gpioConnection.Open();

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
