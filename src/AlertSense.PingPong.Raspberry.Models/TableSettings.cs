using Raspberry.IO.GeneralPurpose;

namespace AlertSense.PingPong.Raspberry.Models
{
    public class TableSettings
    {
        private IGpioConnectionDriver _driver;

        public IGpioConnectionDriver Driver
        {
            get { return _driver; }
            set { _driver = value ?? GpioConnectionSettings.DefaultDriver; }
        }

        public ProcessorPin ButtonPin { get; set; }
        public ProcessorPin LedPin { get; set; }
        public ProcessorPin BouncePin { get; set; }

        public static TableSettings Table1 
        {
            get
            {
                return new TableSettings 
                {
                    ButtonPin = ProcessorPin.Pin17,
                    BouncePin = ProcessorPin.Pin6,
                    LedPin = ProcessorPin.Pin22,
                };
            }
        }

        public static TableSettings Table2
        {
            get
            {
                return new TableSettings 
                {
                    ButtonPin = ProcessorPin.Pin18,
                    BouncePin = ProcessorPin.Pin12,
                    LedPin = ProcessorPin.Pin23,                
                };
            }
        }
    }
}