using AlertSense.PingPong.Raspberry.IO;
using System;
using ServiceStack;

namespace AlertSense.PingPong.Raspberry
{

    public class GameController : IDisposable
    {
        private bool _t1LedOn = false;
        private ITableConnection _table1;
        private ITableConnection _table2;

        private IRestClient _restClient;

        private DateTime? _buttonDownOn;

        public void Start()
        {
            _table1 = ConnectionFactory.GetTableConnection("Table1");
            _table1.Bounce += Table_Bounce;
            _table1.ButtonPressed += Table_ButtonPressed;
            _table1.Open();

            Console.Clear();
        }

        void Table_ButtonPressed(object sender, ButtonEventArgs e)
        {
            var table = (ITableConnection)sender;
            Console.WriteLine("ButtonStateChanged: {0}, Now: {1}", e.Enabled, DateTime.Now);
            
            if (!e.Enabled && _buttonDownOn.HasValue) 
            {
                var elapsed = DateTime.Now.Subtract(_buttonDownOn.Value).TotalMilliseconds;

                if (elapsed < 500)
                {
                    // Single button press
                    _t1LedOn = !_t1LedOn;
                    _table1.Led(_t1LedOn);
                    Console.WriteLine("{0}_Button pressed once", table.Name);
                } 
                else if (elapsed > 2000)
                {
                    // Held down for more than 2 seconds
                    Console.WriteLine("{0}_Button held down", table.Name);
                }
                Console.WriteLine("{0}_Button pressed and released in {1} milliseconds", table.Name, elapsed);
                _buttonDownOn = null;
                return;
            } 
            
            _buttonDownOn = DateTime.Now;
            Console.WriteLine("{0}_Button Down On: {1}", table.Name, _buttonDownOn);
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
