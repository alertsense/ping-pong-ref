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

        public void Start()
        {
            _table1 = ConnectionFactory.GetTableConnection("Table1");
            _table1.Bounce += Table_Bounce;
            _table1.ButtonPressed += Table_ButtonPressed;
            _table1.Open();

        }

        void Table_ButtonPressed(object sender, ButtonEventArgs e)
        {
            if (!e.Enabled) return;

            var table = (ITableConnection)sender;
            _t1LedOn = !_t1LedOn;
            _table1.Led(_t1LedOn);
            Console.WriteLine("ButtonPressed");
        }

        void Table_Bounce(object sender, BounceEventArgs e)
        {
            Console.WriteLine("Bounce");
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
