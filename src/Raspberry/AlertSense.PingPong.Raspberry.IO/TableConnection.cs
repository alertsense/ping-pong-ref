using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Raspberry.IO.GeneralPurpose;

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
        public TableSides Side { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }

    public enum TableSides
    {
        Left,
        Right
    }

    public class TableConnection : ITableConnection
    {
        //TODO: Add left and right bounce events

        private GpioConnection _gpioConnection;

        public event EventHandler<BounceEventArgs> Bounce;
        public TableSettings Settings { get; set; }

        public void Open()
        {
            if (Settings == null)
                throw new Exception("Settings must not be null");

            //Settings.Driver = GpioConnectionSettings.DefaultDriver;
            //var driver = GpioConnectionSettings.DefaultDriver;
            //driver.Allocate(Settings.LeftReadyPin, PinDirection.Output);
            //driver.Allocate(Settings.LeftButtonPin, PinDirection.Input);
            ////for (int i = 0; i < 10; i++)
            //while(true)
            //{
            //    driver.Wait(Settings.LeftButtonPin, true, 1000 * 60);
            //    driver.Write(Settings.LeftReadyPin, true);
            //    Thread.Sleep(200);
            //    driver.Wait(Settings.LeftButtonPin, false, 1000 * 60);
                
            //    driver.Wait(Settings.LeftButtonPin, true, 1000 * 60);
            //    driver.Write(Settings.LeftReadyPin, false);
            //    Thread.Sleep(200);
            //    driver.Wait(Settings.LeftButtonPin, false, 1000 * 60);
            //}

            //driver.Release(Settings.LeftReadyPin);
            Console.WriteLine("Allocate Input Pin");
            Settings.Driver = GpioConnectionSettings.DefaultDriver;
            Settings.Driver.Allocate(Settings.LeftBouncePin, PinDirection.Input);
            Console.WriteLine("Pin Ready");
            var cnt = 0;
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a bounce...");
                    Settings.Driver.Wait(Settings.LeftBouncePin, true, 1000 * 60);
                    cnt++;
                    OnBounce(new BounceEventArgs{Side = TableSides.Left, ElapsedMilliseconds = -1});
                }
            }
            finally
            {
                Settings.Driver.Release(Settings.LeftBouncePin);
            }
            
            //_gpioConnection = new GpioConnection(new GpioConnectionSettings { Driver = Settings.Driver, PollInterval = 1});

            //var leftBouncePinConfig = Settings.LeftBouncePin.Input().OnStatusChanged(b => { if (b) Console.WriteLine("Left Bounce"); });
            ////TODO: Configure GPIO Pins from TableSettings
            //_gpioConnection.Add(leftBouncePinConfig);
            //_gpioConnection.Open();

        }

        //private static async 

        //private static Task<bool> WaitForLeftBounceAsync(IGpioConnectionDriver driver, ProcessorPin pin, decimal timeout)
        //{
        //    return Task<bool>.Factory.StartNew(() =>
        //        {
        //            driver.Wait(pin, true, timeout);
        //            return true;
        //        });
        //}

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
    }
}
