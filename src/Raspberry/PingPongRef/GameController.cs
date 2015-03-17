using AlertSense.PingPong.Raspberry.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Raspberry
{
    public class GameController : IDisposable
    {
        private bool _t1LedOn = false;
        private ITableConnection _table1;
        private ITableConnection _table2;

        private long _buttonDownOn;
        private bool _buttonDown;

        public void Start()
        {
            _table1 = ConnectionFactory.GetTableConnection("Table1");
            _table1.Bounce += Table_Bounce;
            _table1.ButtonPressed += Table_ButtonPressed;
            _table1.Open();

        }

        void Table_ButtonPressed(object sender, ButtonEventArgs e)
        {
            var table = (ITableConnection)sender;
            Console.WriteLine("_buttonDownOn: {0}, Now: {1}", _buttonDownOn, DateTime.Now.Ticks);
            var elapsedSpan = new TimeSpan(DateTime.Now.Ticks - _buttonDownOn);
            if (!e.Enabled) 
            {
                if (_buttonDown && elapsedSpan.Milliseconds < 200)
                {
                    // Single button press
                    _t1LedOn = !_t1LedOn;
                    _table1.Led(_t1LedOn);
                } 
                else
                {
                }
                Console.WriteLine("{0}_Button pressed and released in {1} milliseconds", table.Name, elapsedSpan.Milliseconds);
                _buttonDown = false;
                return;
            }

            if (!_buttonDown)
            {
                _buttonDownOn = DateTime.Now.Ticks;
                _buttonDown = true;
                Console.WriteLine("{0}_Button Down On: {1}", table.Name, _buttonDownOn);
            }
        }

        void Table_Bounce(object sender, BounceEventArgs e)
        {
            var table = (ITableConnection)sender;
            Console.WriteLine("{0}_Bounce", table.Name);
        }

        public void Dispose()
        {
            if (_table1 != null)
                _table1.Close();
            if (_table2 != null)
                _table2.Close();
        }
    }
}
