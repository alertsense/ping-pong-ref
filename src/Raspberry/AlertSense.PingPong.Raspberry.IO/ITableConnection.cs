using AlertSense.PingPong.Raspberry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Raspberry.IO
{
    public interface ITableConnection : IDisposable
    {
        Table Table { get; set; }
        void Led(bool on);
        void Open();
        void Close();
        void Update();

        event EventHandler<BounceEventArgs> Bounce;
        event EventHandler<ButtonEventArgs> ButtonPressed;
    }

    public class BounceEventArgs : EventArgs
    {

    }

    public class ButtonEventArgs : EventArgs
    {
        public ButtonEventArgs(bool enabled)
        {
            Enabled = enabled;
        }
        public bool Enabled { get; set; }
    }
}
