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
        public BounceEventArgs(long elapsed, int count)
        {
            Elapsed = elapsed;
            Count = count;
        }
        
        public BounceEventArgs(bool timeout)
        {
            Timeout = timeout;
        }
        public bool Timeout { get; set; }
        public long Elapsed { get; set; }
        public int Count { get; set; }
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
