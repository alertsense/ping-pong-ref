namespace AlertSense.PingPong.Raspberry.Models
{
    public class GameSettings
    {
        /// <summary>
        /// A single button click must be less than the ButtonClickTime(milliseconds)
        /// </summary>
        public int ButtonClickTime { get; set; }

        /// <summary>
        /// Length of time required to consider the button held down(milliseconds)
        /// </summary>
        public int PressAndHoldTime { get; set; }

        /// <summary>
        /// Length of time to ignore bounces in ticks (10,000 ticks to a millisecond)
        /// </summary>
        public int IgnoreBounceTime { get; set; }

        /// <summary>
        /// URL to the PingPong REST API
        /// </summary>
        public string ApiUri { get; set; }

        /// <summary>
        /// RabbitMq Account
        /// </summary>
        public string RabbitMqHostName { get; set; }
        public string RabbitMqUsername { get; set; }
        public string RabbitMqPassword { get; set; }

        public static GameSettings Default
        {
            get
            {
                return new GameSettings
                {
                   ButtonClickTime = 500,
                   PressAndHoldTime = 2000,
                   IgnoreBounceTime = 70000,
                   ApiUri = "http://pingpong.dev/api/",
                   RabbitMqHostName = "pingpong.dev",
                   RabbitMqUsername = "pi",
                   RabbitMqPassword = "raspberry"
                };
            }
        }
    }
}
