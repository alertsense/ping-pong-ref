namespace AlertSense.PingPong.Raspberry.Models
{
    public class GameSettings
    {
        public int ButtonClickTime { get; set; }
        public int PressAndHoldTime { get; set; }
        public int IgnoreBounceTime { get; set; }
        public string ApiUri { get; set; }
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
                   IgnoreBounceTime = 50000,
                   ApiUri = "http://pingpong.dev/api/",
                   RabbitMqHostName = "amqp://pingpong.dev:5672/",
                   RabbitMqUsername = "guest",
                   RabbitMqPassword = "guest"
                };
            }
        }
    }
}
