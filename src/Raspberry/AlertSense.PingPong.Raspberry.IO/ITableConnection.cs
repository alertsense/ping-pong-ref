using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Raspberry.IO
{
    public interface ITableConnection : IDisposable
    {
        string Name { get; set; }
        TableSettings Settings { get; set; }
        void Led(bool on);
        void Open();
        void Close();

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
