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
