using Raspberry.IO.GeneralPurpose;

namespace AlertSense.PingPong.Raspberry.Models
{
    public class TableSettings
    {
        public const decimal BounceTimeoutDefault = 3600000m; // One hour

        private IGpioConnectionDriver _driver;

        public IGpioConnectionDriver Driver
        {
            get { return _driver; }
            set { _driver = value ?? GpioConnectionSettings.DefaultDriver; }
        }

        /// <summary>
        /// The Raspberry Pi GPIO Pin that the button is wired to.
        /// </summary>
        public ProcessorPin ButtonPin { get; set; }

        /// <summary>
        /// The Raspberry Pi GPIO Pin that the LED is wired to.
        /// </summary>
        public ProcessorPin LedPin { get; set; }

        /// <summary>
        /// The Raspberry Pi GPIO Pin that the bounce circuit it wired to.
        /// </summary>
        public ProcessorPin BouncePin { get; set; }

        /// <summary>
        /// Time to wait for a missing bounce in milliseconds (This feature is not fully implemented)
        /// </summary>
        public decimal BounceTimeout { get; set; }

        public static TableSettings Table1 
        {
            get
            {
                return new TableSettings 
                {
                    ButtonPin = ProcessorPin.Pin17,
                    BouncePin = ProcessorPin.Pin6,
                    LedPin = ProcessorPin.Pin22,
                    BounceTimeout = BounceTimeoutDefault
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
                    BounceTimeout = BounceTimeoutDefault
                };
            }
        }
    }
}