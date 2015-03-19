namespace AlertSense.PingPong.Raspberry.Models
{
    public class GameSettings
    {
        public int ButtonClickTime { get; set; }
        public int PressAndHoldTime { get; set; }
        public string ApiUri { get; set; }
        public string RabbitMqHostName { get; set; }

        public static GameSettings Default
        {
            get
            {
                return new GameSettings
                {
                   ButtonClickTime = 500,
                   PressAndHoldTime = 2000,
                   ApiUri = "http://pingpong.dev/api/",
                   RabbitMqHostName = "pingpong.dev"
                };
            }
        }
    }
}
