using AlertSense.PingPong.Raspberry.Models;
using System;

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
        public BounceEventArgs()
        {
        }
        
        public BounceEventArgs(bool timeout)
        {
            Timeout = timeout;
        }
        public bool Timeout { get; set; }
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
