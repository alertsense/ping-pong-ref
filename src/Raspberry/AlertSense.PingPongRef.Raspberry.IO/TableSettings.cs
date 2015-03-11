using Raspberry.IO.GeneralPurpose;

namespace AlertSense.PingPongRef.Raspberry.IO
{
    public class TableSettings
    {
        private IGpioConnectionDriver _driver;

        public IGpioConnectionDriver Driver
        {
            get { return _driver; }
            set { _driver = value ?? GpioConnectionSettings.DefaultDriver; }
        }

        public ProcessorPin LeftBouncePin { get; set; }
        public ProcessorPin RightBouncePin { get; set; }

        public static TableSettings Default 
        {
            get
            {
                return new TableSettings 
                { 
                    LeftBouncePin = ProcessorPin.Pin17, 
                    RightBouncePin = ProcessorPin.Pin23
                };
            }
        }
    }
}