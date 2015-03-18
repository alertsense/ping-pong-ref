using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Raspberry.Models
{
    public class GameSettings
    {
        public int ButtonClickTime { get; set; }
        public int PressAndHoldTime { get; set; }
        public string ApiUri { get; set; }

        public static GameSettings Default
        {
            get
            {
                return new GameSettings
                {
                   ButtonClickTime = 500,
                   PressAndHoldTime = 2000,
                   ApiUri = "http://192.168.1.19/api/"
                };
            }
        }
    }
}
