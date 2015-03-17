using Raspberry.IO.GeneralPurpose;

namespace AlertSense.PingPong.Raspberry.IO
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
        public ProcessorPin RightButtonPin { get; set; }

        public ProcessorPin LeftLedPin { get; set; }
        public ProcessorPin LeftButtonPin { get; set; }
        public ProcessorPin RightLedPin { get; set; }

        public static TableSettings Default 
        {
            get
            {
                return new TableSettings 
                {
                    LeftButtonPin = ProcessorPin.Pin17,
                    LeftBouncePin = ProcessorPin.Pin6,
                    LeftLedPin = ProcessorPin.Pin22,

                    RightButtonPin = ProcessorPin.Pin18,
                    RightBouncePin = ProcessorPin.Pin12,
                    RightLedPin = ProcessorPin.Pin23,
                };
            }
        }

        public static TableSettings Left
        {
            get
            {
                return new TableSettings();
            }
        }
    }
}